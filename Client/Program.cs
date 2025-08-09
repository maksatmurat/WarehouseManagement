using Application.Helpers;
using Application.Interfaces;
using Application.Services;
using Blazored.LocalStorage;
using Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Services.AddTransient<CustomHttpHandler>();

        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7226/")
        });

        builder.Services.AddHttpClient("SystemApiClient", client =>
        {
            //client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
            client.BaseAddress = new Uri("https://localhost:7226/");
        }).AddHttpMessageHandler<CustomHttpHandler>();


        builder.Services.AddAuthorizationCore();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddScoped<GetHttpClient>();
        builder.Services.AddScoped<LocalStorageService>();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<IUserAccountService, UserAccountService>();
        builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericApiService<>));
        builder.Services.AddScoped<IShipmentService, ShipmentServiceHttp>();


        await builder.Build().RunAsync();
    }
}