using Refit;
using PlaidApi.Services;
using PlaidApi.Paylands;
using PlaidApi.Monei;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Builder;

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

// Registrar Refit para MONEI
builder.Services.AddRefitClient<IMoneiApi>()
    .ConfigureHttpClient((sp, c) =>
    {
        c.BaseAddress = new Uri("https://api.monei.com/v1");
        var config = sp.GetRequiredService<IConfiguration>();
        var apiKey = config["MONEI_API_KEY"] ?? "pk_test_1e0e281d34ae1fc78b4fc826777208ef";
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
builder.Services.AddOpenApi();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors(); 
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
