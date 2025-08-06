using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using Calabonga.AspNetCore.AppDefinitions;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server.Definitions;

public class CommonDefinition:AppDefinition
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
        var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>()
                         ?? throw new InvalidOperationException("JWT section not configured");

        builder.Services.AddDbContext<WarehouseDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string not found!"));

        });
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection!.Issuer,
                ValidAudience = jwtSection!.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!))
            };
        });

        #region CORS Setting 
        var originsFromEnv = Environment.GetEnvironmentVariable("CORS__AllowedOrigins");

        string[] baseOrigins;

        if (!string.IsNullOrEmpty(originsFromEnv))
        {
            baseOrigins = originsFromEnv.Split(';', StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            baseOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        }

        // Функция для автоматического добавления http и https если отсутствуют
        IEnumerable<string> NormalizeOrigins(IEnumerable<string> origins)
        {
            foreach (var origin in origins)
            {
                if (origin.StartsWith("http://") || origin.StartsWith("https://"))
                {
                    yield return origin;
                }
                else
                {
                    yield return "http://" + origin;
                    yield return "https://" + origin;
                }
            }
        }

        var allowedOrigins = NormalizeOrigins(baseOrigins).Distinct().ToArray();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorWasm", policy =>
            {
                policy.WithOrigins(allowedOrigins!)
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
        #endregion

        builder.Services.AddScoped<IUserAccount, UserAccountRepository>();

        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


        builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IUnitOfMeasureService, UnitOfMeasureService>();
        builder.Services.AddScoped<IReceiptDocumentService, ReceiptDocumentService>();
        builder.Services.AddScoped<IReceiptResourceService, ReceiptResourceService>();
        builder.Services.AddScoped<IShipmentDocumentService, ShipmentDocumentService>();
        builder.Services.AddScoped<IShipmentResourceService, ShipmentResourceService>();

    }


    public override void ConfigureApplication(WebApplication app)
    {
        // Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowBlazorWasm");
        app.MapControllers();
    }
  
}
