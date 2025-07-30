using Calabonga.Blazor.AppDefinitions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Server.Configuration
{
    public class CommonDefiniton : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<WarehouseDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        }
        public override void ConfigureApplication(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
                db.Database.Migrate();
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
