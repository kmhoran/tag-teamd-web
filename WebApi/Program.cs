using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using TagTeamdWeb.Common;
using TagTeamdWeb.Common.Interfaces.Jwt;
using TagTeamdWeb.Common.Models.Jwt;
using TagTeamdWeb.Services.Jwt;
using TagTeamdWeb.WebApi.Middleware.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

string stage = Environment.GetEnvironmentVariable("STAGE");
bool isDebug = stage == null || stage.ToLower() != "prod";



var builder = WebApplication.CreateBuilder(args);

if (isDebug)
{
    Console.WriteLine("-----------------------------------------");
    Console.WriteLine("DEBUG");
    Console.WriteLine("-----------------------------------------");
    Console.WriteLine($"JWT AUTH URL: {Constants.JwtValidationUrl}");
    Console.WriteLine($"JWT SECRET: {Constants.JwtSecret}");
    Console.WriteLine($"JWT ISSUER: {Constants.Issuer}");
    Console.WriteLine("-----------------------------------------");

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Constants.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecret)) 
        };
    });

builder.Services.AddSingleton<IJwtValidationService, JwtValidationService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ValidUser", policy =>
        policy.RequireClaim(ClaimTypes.NameIdentifier));
});

string corsPolicy = "_allowAllOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy,
        policy =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true);
        });
});

var app = builder.Build();

app.UseMiddleware<JwtValidationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


if (!isDebug)
{
    app.UseHttpsRedirection();
}
app.UseCors(corsPolicy);


app.Run();
