using Microsoft.Extensions.Hosting;

namespace BlazorApp.Api;

public static class Program
{
    public static void Main()
    {
        IHost host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .Build();

        host.Run();
    }
}