using Calabonga.Blazor.AppDefinitions;
using System.Reflection;

namespace Server.Configuration
{
    public static class AppDefinitionExtensions
    {
        public static WebApplicationBuilder AddAppDefinitions(this WebApplicationBuilder builder, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetExecutingAssembly();

            var definitions = assembly.GetTypes()
                .Where(t => typeof(AppDefinition).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<AppDefinition>()
                .ToList();

            builder.Services.AddSingleton(definitions); // сохраняем в DI

            foreach (var def in definitions)
                def.ConfigureServices(builder);

            return builder;
        }

        public static WebApplication UseAppDefinitions(this WebApplication app)
        {
            var definitions = app.Services.GetRequiredService<IEnumerable<AppDefinition>>();
            foreach (var def in definitions)
                def.ConfigureApplication(app);

            return app;
        }
    }
}
