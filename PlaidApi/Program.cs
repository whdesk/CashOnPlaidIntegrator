using Refit;

var builder = WebApplication.CreateBuilder(args);

// Registrar el cliente Refit para IPibisiApi
builder.Services.AddRefitClient<PlaidApi.Pibisi.IPibisiApi>()
    .ConfigureHttpClient((sp, c) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var baseUrl = config["PIBISI_BASE_URL"] ?? "https://int.api.pibisi.com/v2";
        c.BaseAddress = new Uri(baseUrl);
        var token = config["PIBISI_AUTH_TOKEN"];
        if (!string.IsNullOrWhiteSpace(token))
            c.DefaultRequestHeaders.Add("X-AUTH-TOKEN", token);
    });

// Permite CORS para el frontend Angular
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//// Configura un HttpClient específico para Pibisi
//builder.Services.AddHttpClient("Pibisi", (serviceProvider, client) =>
//{
//    var config = serviceProvider.GetRequiredService<IConfiguration>();
//    client.BaseAddress = new Uri(config["PIBISI_API_BASE"] ?? "https://int.api.pibisi.com/");
//    var token = config["PIBISI_AUTH_TOKEN"];
//    if (!string.IsNullOrWhiteSpace(token))
//        client.DefaultRequestHeaders.Add("X-AUTH-TOKEN", token);
//});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
