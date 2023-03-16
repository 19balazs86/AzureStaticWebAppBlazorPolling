using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorApp.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {

        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        IServiceCollection services  = builder.Services;
        IConfiguration configuration = builder.Configuration;

        var baseAddress = new Uri(configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress);

        // Add services to the container
        {
            services.addDefaultHttpClient(baseAddress);
        }

        await builder.Build().RunAsync();
    }

    private static void addDefaultHttpClient(this IServiceCollection services, Uri baseAddress)
    {
        services.AddSingleton(new HttpClient { BaseAddress = baseAddress });
    }
}
