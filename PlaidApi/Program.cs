using Refit;
using PlaidApi.Services;
using PlaidApi.Paylands;

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

// Registrar el cliente Refit para Paylands
builder.Services.AddRefitClient<IPaylandsApi>()
    .ConfigureHttpClient((sp, c) =>
    {
        c.BaseAddress = new Uri("https://api.paylands.com/v1/sandbox");
        var config = sp.GetRequiredService<IConfiguration>();
        var apiKey = config["PAYLANDS_API_KEY"] ?? "cd826d49c11245c5b28dcd98d6c76f0a";
        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{apiKey}:");
        c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
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

// Registrar PaylandsApiService con interfaz
builder.Services.AddScoped<IPaylandsApiService, PaylandsApiService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
