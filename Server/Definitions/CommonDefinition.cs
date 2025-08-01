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

        builder.Services.AddScoped<IUserAccount, UserAccountRepository>();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorWasm", builder =>
            {
                builder.WithOrigins("http://localhost:5227", "https://localhost:7185")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
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
