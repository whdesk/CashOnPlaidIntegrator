using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RedsysSettings>(builder.Configuration.GetSection("Redsys"));
builder.Services.AddSingleton<RedsysSignatureService>();
builder.Services.AddSingleton<RedsysState>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
var app = builder.Build();

// Firma parámetros SIS arbitrarios enviados por el cliente y devuelve Ds_* (HMAC_SHA256_V1)
app.MapPost("/redsys/sis/sign", (Dictionary<string, object?> merchantParams, IConfiguration cfg, RedsysSignatureService signer) =>
{
    var settings = cfg.GetSection("Redsys").Get<RedsysSettings>()!;
    if (!merchantParams.TryGetValue("DS_MERCHANT_ORDER", out var orderObj) || orderObj is null)
        return Results.BadRequest("Falta DS_MERCHANT_ORDER en el cuerpo");
    var order = orderObj.ToString() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(order))
        return Results.BadRequest("DS_MERCHANT_ORDER vacío");

    var dsParams = signer.BuildMerchantParametersBase64(merchantParams);
    var signature = signer.BuildSignatureUrlSafe(dsParams, settings.SecretKeyBase64!, order);

    // Log: parámetros del comercio y base64 decodificado
    try
    {
        app.Logger.LogInformation("SIS SIGN order={order} merchantParams={merchantParams}", order, JsonSerializer.Serialize(merchantParams));
        app.Logger.LogInformation("SIS SIGN decoded={decoded}", signer.Base64ToJson(dsParams));
    }
    catch { }

    return Results.Ok(new
    {
        formUrl = settings.SisUrl,
        ds_SignatureVersion = "HMAC_SHA256_V1",
        ds_MerchantParameters = dsParams,
        ds_Signature = signature,
        order
    });
});

// MIT por REST: construye JSON con parámetros específicos MIT y firma HMAC_SHA512_V1
app.MapPost("/redsys/mit/rest/start", async (MitChargeRequest req, IConfiguration cfg, RedsysSignatureService signer) =>
{
    var settings = cfg.GetSection("Redsys").Get<RedsysSettings>()!;
    if (string.IsNullOrWhiteSpace(settings.RestUrl))
        return Results.BadRequest("Redsys.RestUrl no configurado");
    if (string.IsNullOrWhiteSpace(req.Identifier))
        return Results.BadRequest("Identifier requerido");
    if (req.Amount <= 0)
        return Results.BadRequest("Amount inválido");

    var order = string.IsNullOrWhiteSpace(req.Order) ? GenerateOrder() : req.Order!;

    var merchantParams = new Dictionary<string, object?>
    {
        ["DS_MERCHANT_ORDER"] = order,
        ["DS_MERCHANT_MERCHANTCODE"] = settings.MerchantCode,
        ["DS_MERCHANT_TERMINAL"] = settings.Terminal,
        ["DS_MERCHANT_CURRENCY"] = settings.Currency,
        ["DS_MERCHANT_TRANSACTIONTYPE"] = settings.MitTransactionType, // tipo MIT según contrato
        ["DS_MERCHANT_AMOUNT"] = req.Amount.ToString(),
        ["DS_MERCHANT_IDENTIFIER"] = req.Identifier,
        ["DS_MERCHANT_COF_INI"] = "S",
        ["DS_MERCHANT_COF_TYPE"] = "R",
        ["DS_MERCHANT_DIRECTPAYMENT"] = "true",
        ["DS_MERCHANT_EXCEP_SCA"] = "MIT"
    };

    var dsParams = signer.BuildMerchantParametersBase64(merchantParams);
    var signature = signer.BuildSignatureUrlSafeSha512(dsParams, settings.SecretKeyBase64!, order);

    // Log: MIT REST parámetros
    try
    {
        app.Logger.LogInformation("MIT REST order={order} decodedMerchantParams={decoded}", order, signer.Base64ToJson(dsParams));
    }
    catch { }

    var payload = new
    {
        Ds_SignatureVersion = "HMAC_SHA512_V1",
        Ds_MerchantParameters = dsParams,
        Ds_Signature = signature
    };

    using var http = new HttpClient();
    var httpResp = await http.PostAsJsonAsync(settings.RestUrl, payload);
    var body = await httpResp.Content.ReadAsStringAsync();
    try { app.Logger.LogInformation("MIT REST response status={status} body={body}", (int)httpResp.StatusCode, body); } catch { }
    return Results.Content(body, "application/json");
});

// Propagar modo 3DES desde configuración a AppContext switch usado por la derivación
{
    var cfgSettings = app.Configuration.GetSection("Redsys").Get<RedsysSettings>();
    if (cfgSettings?.Use3DesEcb == true)
    {
        AppContext.SetSwitch("Redsys.Use3DesEcb", true);
    }
}

app.UseCors("frontend");

app.MapGet("/", () => Results.Ok(new { status = "RedsysTester up" }));

// Inicia tokenización: devuelve los 3 campos para postear al SIS
app.MapPost("/redsys/tokenize/start", (IConfiguration cfg, RedsysSignatureService signer) =>
{
    var settings = cfg.GetSection("Redsys").Get<RedsysSettings>()!;
    
    var order = GenerateOrder();
    
    var merchantParams = new Dictionary<string, object?>
    {
        ["DS_MERCHANT_MERCHANTCODE"] = settings.MerchantCode,
        ["DS_MERCHANT_TERMINAL"] = settings.Terminal,
        ["DS_MERCHANT_ORDER"] = order,
        ["DS_MERCHANT_AMOUNT"] = settings.TokenizationAmountCents.ToString(),
        ["DS_MERCHANT_CURRENCY"] = settings.Currency,
        ["DS_MERCHANT_TRANSACTIONTYPE"] = settings.TokenizationTransactionType,
        ["DS_MERCHANT_PAYMETHODS"] = "C", // Tarjeta
        ["DS_MERCHANT_COF_INI"] = settings.CofIniForTokenization, // "Y"
        ["DS_MERCHANT_COF_TYPE"] = settings.CofType, // "R"
        ["DS_MERCHANT_EXCEP_SCA"] = "N", // Requiere autenticación
        ["DS_MERCHANT_MERCHANTURL"] = settings.MerchantUrl,
        ["DS_MERCHANT_URLOK"] = settings.UrlOk,
        ["DS_MERCHANT_URLKO"] = settings.UrlKo
    };

    if (!string.IsNullOrWhiteSpace(settings.ConsumerLanguage))
        merchantParams["DS_MERCHANT_CONSUMERLANGUAGE"] = settings.ConsumerLanguage;

    var dsMerchantParameters = signer.BuildMerchantParametersBase64(merchantParams);
    var signature = signer.BuildSignatureUrlSafe(dsMerchantParameters, settings.SecretKeyBase64!, order);

    // Log: Tokenize parámetros decodificados
    try { app.Logger.LogInformation("TOKENIZE order={order} decoded={decoded}", order, signer.Base64ToJson(dsMerchantParameters)); } catch { }

    return Results.Ok(new TokenizeStartResponse
    {
        FormUrl = settings.SisUrl!,
        Ds_SignatureVersion = "HMAC_SHA256_V1",
        Ds_MerchantParameters = dsMerchantParameters,
        Ds_Signature = signature,
        Order = order
    });
});

// Webhook de Redsys: valida firma, decodifica parámetros y muestra info
app.MapPost("/redsys/webhook", async (HttpRequest req, IConfiguration cfg, RedsysSignatureService signer, RedsysState state) =>
{
    var settings = cfg.GetSection("Redsys").Get<RedsysSettings>()!;

    // Redsys envía x-www-form-urlencoded
    if (!req.HasFormContentType)
        return Results.BadRequest("Invalid content-type");

    var form = await req.ReadFormAsync();
    var version = form["Ds_SignatureVersion"].ToString();
    var parameters = form["Ds_MerchantParameters"].ToString();
    var signature = form["Ds_Signature"].ToString();

    var ok = signer.VerifyRedsysSignature(signature, parameters, settings.SecretKeyBase64!);
    var decoded = signer.Base64ToJson(parameters);
    try { app.Logger.LogInformation("WEBHOOK validSignature={ok} decoded={decoded}", ok, decoded); } catch { }

    // Extraer campos útiles del JSON decodificado
    string? order = null;
    string? identifier = null;
    string? response = null;
    try
    {
        using var doc = JsonDocument.Parse(decoded);
        if (doc.RootElement.TryGetProperty("Ds_Order", out var pOrder)) order = pOrder.GetString();
        if (doc.RootElement.TryGetProperty("Ds_Merchant_Identifier", out var pId)) identifier = pId.GetString();
        if (doc.RootElement.TryGetProperty("Ds_Response", out var pResp)) response = pResp.GetRawText();
    }
    catch { /* ignore parse errors */ }

    // Guardar estado en memoria para polling de front
    state.Set(new RedsysLastStatus
    {
        ReceivedAt = DateTimeOffset.UtcNow,
        ValidSignature = ok,
        Order = order,
        Identifier = identifier,
        RawJson = decoded
    });

    return Results.Ok(new
    {
        signatureVersion = version,
        validSignature = ok,
        order,
        identifier,
        response,
        merchantParameters = decoded
    });
});

app.MapGet("/redsys/ok", () => Results.Content("<h2>OK</h2>", "text/html"));
app.MapGet("/redsys/ko", () => Results.Content("<h2>KO</h2>", "text/html"));

// Endpoint para que el front consulte el último estado recibido
app.MapGet("/redsys/last-status", (RedsysState state) =>
{
    var last = state.Get();
    return Results.Ok(last ?? new { completed = false });
});

// Inicia un cargo MIT (Merchant Initiated Transaction) con un Identifier existente
// Retorna los 3 campos firmados para realizar la llamada al SIS.
app.MapPost("/redsys/mit/charge/start", (MitChargeRequest req, IConfiguration cfg, RedsysSignatureService signer) =>
{
    var settings = cfg.GetSection("Redsys").Get<RedsysSettings>()!;
    if (string.IsNullOrWhiteSpace(req.Identifier))
        return Results.BadRequest("Identifier requerido");
    if (req.Amount <= 0)
        return Results.BadRequest("Amount inválido");
    if (string.IsNullOrWhiteSpace(settings.MitTransactionType))
        return Results.BadRequest("MitTransactionType no configurado");

    var order = string.IsNullOrWhiteSpace(req.Order) ? GenerateOrder() : req.Order!;

    var merchantParams = new Dictionary<string, object?>
    {
        ["DS_MERCHANT_MERCHANTCODE"] = settings.MerchantCode,
        ["DS_MERCHANT_TERMINAL"] = settings.Terminal,
        ["DS_MERCHANT_ORDER"] = order,
        ["DS_MERCHANT_CURRENCY"] = settings.Currency,
        ["DS_MERCHANT_AMOUNT"] = req.Amount.ToString(),
        ["DS_MERCHANT_IDENTIFIER"] = req.Identifier,
        ["DS_MERCHANT_TRANSACTIONTYPE"] = settings.MitTransactionType,
        ["DS_MERCHANT_PAYMETHODS"] = settings.PayMethods,
        ["DS_MERCHANT_MERCHANTURL"] = settings.MerchantUrl,
        ["DS_MERCHANT_URLOK"] = settings.UrlOk,
        ["DS_MERCHANT_URLKO"] = settings.UrlKo,
        // Parámetros COF/MIT obligatorios
        ["DS_MERCHANT_COF_INI"] = "S", // S para transacciones sucesivas
        ["DS_MERCHANT_COF_TYPE"] = "R", // R para recurrente
        ["DS_MERCHANT_EXCEP_SCA"] = "MIT",
        ["DS_MERCHANT_DIRECTPAYMENT"] = "true"
    };

    if (!string.IsNullOrWhiteSpace(settings.ConsumerLanguage))
        merchantParams["DS_MERCHANT_CONSUMERLANGUAGE"] = settings.ConsumerLanguage;

    var dsMerchantParameters = signer.BuildMerchantParametersBase64(merchantParams);
    var signature = signer.BuildSignatureUrlSafe(dsMerchantParameters, settings.SecretKeyBase64!, order);

    return Results.Ok(new MitChargeStartResponse
    {
        FormUrl = settings.SisUrl!,
        Ds_SignatureVersion = "HMAC_SHA256_V1",
        Ds_MerchantParameters = dsMerchantParameters,
        Ds_Signature = signature,
        Order = order
    });
});

app.MapPost("/redsys/mit/charge", (MitChargeRequest req, IConfiguration cfg, RedsysSignatureService signer) =>
{
    var settings = cfg.GetSection("Redsys").Get<RedsysSettings>()!;
    
    if (string.IsNullOrEmpty(req.Identifier))
        return Results.BadRequest("Se requiere Identifier");
    if (req.Amount <= 0) 
        return Results.BadRequest("Amount debe ser positivo");

    var order = GenerateOrder();
    
    var merchantParams = new Dictionary<string, object?>
    {
        ["DS_MERCHANT_MERCHANTCODE"] = settings.MerchantCode,
        ["DS_MERCHANT_TERMINAL"] = settings.Terminal,
        ["DS_MERCHANT_ORDER"] = order,
        ["DS_MERCHANT_AMOUNT"] = req.Amount.ToString(),
        ["DS_MERCHANT_CURRENCY"] = settings.Currency,
        ["DS_MERCHANT_TRANSACTIONTYPE"] = "0", // Autorización
        ["DS_MERCHANT_IDENTIFIER"] = req.Identifier,
        ["DS_MERCHANT_COF_INI"] = "S", // Transacción sucesiva
        ["DS_MERCHANT_COF_TYPE"] = "R",
        ["DS_MERCHANT_EXCEP_SCA"] = "MIT",
        ["DS_MERCHANT_DIRECTPAYMENT"] = "true"
    };

    if (!string.IsNullOrWhiteSpace(settings.ConsumerLanguage))
        merchantParams["DS_MERCHANT_CONSUMERLANGUAGE"] = settings.ConsumerLanguage;


    var dsParams = signer.BuildMerchantParametersBase64(merchantParams);
    var signature = signer.BuildSignatureUrlSafe(dsParams, settings.SecretKeyBase64!, order);

    return Results.Ok(new 
    {
        Ds_SignatureVersion = "HMAC_SHA256_V1",
        Ds_MerchantParameters = dsParams,
        Ds_Signature = signature,
        Order = order
    });
});

// Inicia un pago Bizum por redirección: genera los tres campos para postear al SIS
app.MapPost("/redsys/bizum/start", (BizumStartRequest req, IConfiguration cfg, RedsysSignatureService signer) =>
{
    var settings = cfg.GetSection("Redsys").Get<RedsysSettings>()!;

    if (req.Amount <= 0)
        return Results.BadRequest("Amount inválido");

    var order = string.IsNullOrWhiteSpace(req.Order) ? GenerateOrder() : req.Order!;

    var merchantParams = new Dictionary<string, object?>
    {
        ["DS_MERCHANT_MERCHANTCODE"] = settings.MerchantCode,
        ["DS_MERCHANT_TERMINAL"] = settings.Terminal,
        ["DS_MERCHANT_ORDER"] = order,
        ["DS_MERCHANT_AMOUNT"] = req.Amount.ToString(),
        ["DS_MERCHANT_CURRENCY"] = settings.Currency,
        // Estos valores deben alinearse con la configuración Bizum de tu entidad
        ["DS_MERCHANT_TRANSACTIONTYPE"] = settings.BizumTransactionType,
        ["DS_MERCHANT_PAYMETHODS"] = settings.BizumPayMethods,
        ["DS_MERCHANT_MERCHANTURL"] = settings.MerchantUrl,
        ["DS_MERCHANT_URLOK"] = settings.UrlOk,
        ["DS_MERCHANT_URLKO"] = settings.UrlKo
    };

    if (!string.IsNullOrWhiteSpace(settings.ConsumerLanguage))
        merchantParams["DS_MERCHANT_CONSUMERLANGUAGE"] = settings.ConsumerLanguage;

    if (!string.IsNullOrWhiteSpace(req.Phone))
        merchantParams["DS_MERCHANT_BIZUM_MOBILENUMBER"] = req.Phone;

    var dsParams = signer.BuildMerchantParametersBase64(merchantParams);
    var signature = signer.BuildSignatureUrlSafe(dsParams, settings.SecretKeyBase64!, order);

    try { app.Logger.LogInformation("BIZUM START order={order} decoded={decoded}", order, signer.Base64ToJson(dsParams)); } catch { }

    return Results.Ok(new
    {
        FormUrl = settings.SisUrl!,
        Ds_SignatureVersion = "HMAC_SHA256_V1",
        Ds_MerchantParameters = dsParams,
        Ds_Signature = signature,
        Order = order
    });
});

// Autoriza operación InSite usando DS_MERCHANT_IDOPER y el mismo ORDER que se usó al generar el ID de operación
app.MapPost("/redsys/insite/authorize", (InSiteAuthorizeRequest req, IConfiguration cfg, RedsysSignatureService signer) =>
{
    var settings = cfg.GetSection("Redsys").Get<RedsysSettings>()!;

    if (string.IsNullOrWhiteSpace(req.IdOper))
        return Results.BadRequest("IdOper requerido");
    if (string.IsNullOrWhiteSpace(req.Order))
        return Results.BadRequest("Order requerido (mismo usado en carga InSite)");
    if (req.Amount <= 0)
        return Results.BadRequest("Amount inválido");

    if (string.IsNullOrWhiteSpace(settings.MerchantCode) ||
        string.IsNullOrWhiteSpace(settings.Terminal) ||
        string.IsNullOrWhiteSpace(settings.SecretKeyBase64) ||
        string.IsNullOrWhiteSpace(settings.SisUrl))
    {
        return Results.BadRequest("Configuración Redsys incompleta (MerchantCode/Terminal/SecretKeyBase64/SisUrl)");
    }

    var payMethods = string.IsNullOrWhiteSpace(settings.PayMethods) ? "C" : settings.PayMethods;

    var merchantParams = new Dictionary<string, object?>
    {
        ["DS_MERCHANT_MERCHANTCODE"] = settings.MerchantCode,
        ["DS_MERCHANT_TERMINAL"] = settings.Terminal,
        ["DS_MERCHANT_ORDER"] = req.Order,
        ["DS_MERCHANT_AMOUNT"] = req.Amount.ToString(),
        ["DS_MERCHANT_CURRENCY"] = settings.Currency,
        ["DS_MERCHANT_TRANSACTIONTYPE"] = "0", // Autorización
        ["DS_MERCHANT_IDOPER"] = req.IdOper,
        ["DS_MERCHANT_PAYMETHODS"] = payMethods,
        ["DS_MERCHANT_MERCHANTURL"] = settings.MerchantUrl,
        ["DS_MERCHANT_URLOK"] = settings.UrlOk,
        ["DS_MERCHANT_URLKO"] = settings.UrlKo
    };

    if (!string.IsNullOrWhiteSpace(settings.ConsumerLanguage))
        merchantParams["DS_MERCHANT_CONSUMERLANGUAGE"] = settings.ConsumerLanguage;

    var dsParams = signer.BuildMerchantParametersBase64(merchantParams);
    var signature = signer.BuildSignatureUrlSafe(dsParams, settings.SecretKeyBase64!, req.Order);

    try { cfg.GetSection(""); } catch { }
    try { app.Logger.LogInformation("INSITE AUTH order={order} decoded={decoded}", req.Order, signer.Base64ToJson(dsParams)); } catch { }

    return Results.Ok(new
    {
        FormUrl = settings.SisUrl!,
        Ds_SignatureVersion = "HMAC_SHA256_V1",
        Ds_MerchantParameters = dsParams,
        Ds_Signature = signature,
        Order = req.Order
    });
});

//app.MapGet("/redsys/query", async (string order, RedsysDbContext db) => 
//{
//    var transaction = await db.Transactions
//        .FirstOrDefaultAsync(t => t.Order == order);

//    if (transaction == null)
//        return Results.NotFound();

//    return Results.Ok(new
//    {
//        transaction.Order,
//        transaction.Amount,
//        transaction.Currency,
//        transaction.Status,
//        transaction.Identifier,
//        transaction.CreatedAt
//    });
//});

app.Run();

static string GenerateOrder()
{
    // ORDER 4-12 caracteres numéricos (Redsys recomienda 12)
    var rng = RandomNumberGenerator.Create();
    byte[] bytes = new byte[8];
    rng.GetBytes(bytes);
    
    // Convertir a número positivo de 12 dígitos
    var number = Math.Abs(BitConverter.ToInt64(bytes)) % 999_999_999_999L;
    return number.ToString("D12"); // Asegurar 12 dígitos con ceros a la izquierda
}

// Settings
public sealed class RedsysSettings
{
    public string? MerchantCode { get; set; }
    public string? Terminal { get; set; }
    public string? Currency { get; set; } = "978";
    public string? SecretKeyBase64 { get; set; }
    public string? MerchantUrl { get; set; }
    public string? UrlOk { get; set; }
    public string? UrlKo { get; set; }
    public string? SisUrl { get; set; }
    public string? RestUrl { get; set; }
    public string? TokenizationTransactionType { get; set; }
    public string? MitTransactionType { get; set; }
    public string? PayMethods { get; set; }
    public bool Use3DesEcb { get; set; }
    public int TokenizationAmountCents { get; set; } = 100;
    public string? CofIniForTokenization { get; set; } = "Y";
    public string? CofType { get; set; } = "R";
    public string? ConsumerLanguage { get; set; }
    public string? BizumTransactionType { get; set; }
    public string? BizumPayMethods { get; set; }
}

public sealed class TokenizeStartResponse
{
    [JsonPropertyName("formUrl")] public string FormUrl { get; set; } = string.Empty;
    [JsonPropertyName("ds_SignatureVersion")] public string Ds_SignatureVersion { get; set; } = "HMAC_SHA256_V1";
    [JsonPropertyName("ds_MerchantParameters")] public string Ds_MerchantParameters { get; set; } = string.Empty;
    [JsonPropertyName("ds_Signature")] public string Ds_Signature { get; set; } = string.Empty;
    [JsonPropertyName("order")] public string Order { get; set; } = string.Empty;
}

public sealed class RedsysSignatureService
{
    public string BuildMerchantParametersBase64(object merchantParams)
    {
        var json = JsonSerializer.Serialize(merchantParams);
        var bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public string BuildSignatureUrlSafe(string merchantParametersXml, string secretKeyBase64, string order)
    {
        // 1. Validación de parámetros
        if (string.IsNullOrEmpty(merchantParametersXml) || 
            string.IsNullOrEmpty(secretKeyBase64) || 
            string.IsNullOrEmpty(order))
            throw new ArgumentException("Parámetros requeridos");

        // 2. Derivación de clave 3DES (CBC + zeros padding)
        byte[] key;
        try {
            key = Convert.FromBase64String(secretKeyBase64);
            if (key.Length != 24)
                throw new ArgumentException("Clave debe ser de 24 bytes");
        } catch {
            throw new ArgumentException("Clave no es Base64 válido");
        }

        byte[] orderBytes = Encoding.UTF8.GetBytes(order);
        byte[] derivedKey = Encrypt3Des(orderBytes, key);

        // 3. Calcular HMAC-SHA256 sobre XML crudo
        using var hmac = new HMACSHA256(derivedKey);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(merchantParametersXml));

        // 4. Convertir a Base64 (manteniendo padding '=')
        return Convert.ToBase64String(hash)
            .Replace('+', '-')
            .Replace('/', '_');
    }

    public bool VerifyRedsysSignature(string signatureReceived, string merchantParametersBase64, string secretKeyBase64)
    {
        // La ORDER viene dentro de merchantParameters; hay que decodificar y leer DS_ORDER
        var json = Base64ToJson(merchantParametersBase64);
        var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("Ds_Order", out var orderProp))
            return false;
        var order = orderProp.GetString() ?? string.Empty;
        var sig = BuildSignatureUrlSafe(merchantParametersBase64, secretKeyBase64, order);
        return string.Equals(sig, signatureReceived, StringComparison.Ordinal);
    }

    public string Base64ToJson(string merchantParametersBase64)
    {
        var bytes = Convert.FromBase64String(merchantParametersBase64);
        return Encoding.UTF8.GetString(bytes);
    }

    private static byte[] Encrypt3Des(byte[] input, byte[] key)
    {
        using var des = TripleDES.Create();
        des.Mode = CipherMode.CBC;
        des.Padding = PaddingMode.Zeros;
        des.Key = key;
        des.IV = new byte[8]; // IV cero
        
        using var encryptor = des.CreateEncryptor();
        return encryptor.TransformFinalBlock(input, 0, input.Length);
    }

    // Construye firma para REST usando HMAC-SHA512 sobre Ds_MerchantParameters (Base64) con la clave derivada 3DES
    public string BuildSignatureUrlSafeSha512(string merchantParametersBase64, string secretKeyBase64, string order)
    {
        // Validación de parámetros
        if (string.IsNullOrEmpty(merchantParametersBase64) || 
            string.IsNullOrEmpty(secretKeyBase64) || 
            string.IsNullOrEmpty(order))
        {
            throw new ArgumentException("Todos los parámetros son requeridos");
        }

        // Validar clave (64 bytes para SHA512)
        byte[] key;
        try
        {
            key = Convert.FromBase64String(secretKeyBase64);
            if (key.Length != 64)
                throw new ArgumentException("La clave para HMAC-SHA512 debe decodificar a 64 bytes");
        }
        catch (FormatException)
        {
            throw new ArgumentException("La clave no es un Base64 válido");
        }

        // Calcular HMAC-SHA512 (sin derivación 3DES)
        using var hmac = new HMACSHA512(key);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(merchantParametersBase64 + order));

        // Convertir a Base64 URL-safe (88 caracteres esperados)
        var signature = Convert.ToBase64String(hash)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        if (signature.Length != 88)
        {
            throw new InvalidOperationException($"Longitud de firma inválida: {signature.Length}. Se esperaban 88 caracteres");
        }

        return signature;
    }

    private static string ToBase64UrlSafe(string base64)
    {
        return base64.Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }
}

public sealed class RedsysLastStatus
{
    public DateTimeOffset ReceivedAt { get; set; }
    public bool ValidSignature { get; set; }
    public string? Order { get; set; }
    public string? Identifier { get; set; }
    public string? RawJson { get; set; }
    public bool Completed => ValidSignature && !string.IsNullOrEmpty(Identifier);
}

public sealed class RedsysState
{
    private RedsysLastStatus? _last;
    private readonly object _lock = new();

    public void Set(RedsysLastStatus status)
    {
        lock (_lock) _last = status;
    }

    public object? Get()
    {
        lock (_lock) return _last is null ? null : new
        {
            completed = _last.Completed,
            receivedAt = _last.ReceivedAt,
            validSignature = _last.ValidSignature,
            order = _last.Order,
            identifier = _last.Identifier,
            raw = _last.RawJson
        };
    }
}

public class MitChargeRequest
{
    public required string Identifier { get; set; }
    public int Amount { get; set; } // Monto en céntimos
    public string? Order { get; set; } // Opcional
}

public sealed class MitChargeStartResponse
{
    [JsonPropertyName("formUrl")] public string FormUrl { get; set; } = string.Empty;
    [JsonPropertyName("ds_SignatureVersion")] public string Ds_SignatureVersion { get; set; } = "HMAC_SHA256_V1";
    [JsonPropertyName("ds_MerchantParameters")] public string Ds_MerchantParameters { get; set; } = string.Empty;
    [JsonPropertyName("ds_Signature")] public string Ds_Signature { get; set; } = string.Empty;
    [JsonPropertyName("order")] public string Order { get; set; } = string.Empty;
}

public sealed class InSiteAuthorizeRequest
{
    public required string IdOper { get; set; }
    public required string Order { get; set; }
    public int Amount { get; set; } // en céntimos
}

public sealed class BizumStartRequest
{
    public int Amount { get; set; } // en céntimos
    public string? Order { get; set; }
    public string? Phone { get; set; }
}
