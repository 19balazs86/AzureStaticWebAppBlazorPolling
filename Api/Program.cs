using BlazorApp.Api.Repository;
using BlazorApp.Api.Services;
using BlazorApp.Api.Services.Implementation;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorApp.Api;

public static class Program
{
    public static void Main()
    {
        IHost host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(configureServices)
            .Build();

        host.Run();
    }

    private static void configureServices(HostBuilderContext builderContext, IServiceCollection services)
    {
        services.AddHostedService<InfrastructureService>();

        services.AddSingleton<IClock, Clock>();

        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<ISchedulerService, SchedulerService>();
        services.AddScoped<ITableRepository, TableRepository>();

        string storageConnString = builderContext.Configuration["AzureWebJobsStorage"];

        // Azure .NET SDKs
        // https://azure.github.io/azure-sdk/releases/latest/dotnet.html

        // Example: https://kaylumah.nl/2022/02/21/working-with-azure-sdk-for-dotnet.html
        // For DI container using the package: Microsoft.Extensions.Azure
        services.AddAzureClients(clients =>
        {
            clients.AddBlobServiceClient(storageConnString);
            clients.AddTableServiceClient(storageConnString);
            clients.AddQueueServiceClient(storageConnString);
        });
    }
}