using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Scalar.AspNetCore;
using API_banco.Data;
using API_banco.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================
// ✅ Controllers
// =============================
builder.Services.AddControllers();

// =============================
// ✅ CORS (puedes dejar abierto para pruebas)
// =============================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// =============================
// ✅ OPENAPI (NO SwaggerGen)
// =============================
builder.Services.AddOpenApi();

// =============================
// ✅ CONEXIÓN MYSQL
// =============================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

// =============================
// ✅ INYECCIÓN DE DEPENDENCIAS
// =============================
builder.Services.AddScoped<BancoService>();

var app = builder.Build();

// =============================
// ✅ OPENAPI + SCALAR
// =============================
app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    // 🔥 ESTA LÍNEA ES LA CLAVE
    options.OpenApiRoutePattern = "/openapi/v1.json";
});

// =============================
// ✅ Middleware
// =============================
app.UseRouting();
app.UseCors();

// =============================
// ✅ MANEJO GLOBAL DE ERRORES
// =============================
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var error = feature?.Error?.Message ?? "Error inesperado";

        var payload = JsonSerializer.Serialize(new
        {
            error,
            detail = app.Environment.IsDevelopment() ? feature?.Error?.ToString() : null
        });

        await context.Response.WriteAsync(payload);
    });
});

// =============================
app.UseHttpsRedirection();
app.UseAuthorization();

// =============================
app.MapControllers();

app.Run();