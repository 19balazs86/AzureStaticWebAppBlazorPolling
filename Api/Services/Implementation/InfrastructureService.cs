using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using BlazorApp.Api.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorApp.Api.Services.Implementation;

public sealed class InfrastructureService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public InfrastructureService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        IServiceProvider serviceProvider = scope.ServiceProvider;

        await createBlobContainerIfNotExists(serviceProvider);

        await createQueueIfNotExists(serviceProvider);

        await createTableIfNotExists(serviceProvider);
    }

    private async Task createBlobContainerIfNotExists(IServiceProvider serviceProvider)
    {
        BlobServiceClient blobServiceClient = serviceProvider.GetRequiredService<BlobServiceClient>();

        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(FileService.ImageContainerName);

        await blobContainerClient.CreateIfNotExistsAsync(publicAccessType: PublicAccessType.Blob);
    }

    private async Task createQueueIfNotExists(IServiceProvider serviceProvider)
    {
        QueueServiceClient queueServiceClient = serviceProvider.GetRequiredService<QueueServiceClient>();

        QueueClient queueClient = queueServiceClient.GetQueueClient(SchedulerService.PollClosingQueueName);

        await queueClient.CreateIfNotExistsAsync();
    }

    private async Task createTableIfNotExists(IServiceProvider serviceProvider)
    {
        TableServiceClient tableServiceClient = serviceProvider.GetRequiredService<TableServiceClient>();

        await tableServiceClient.CreateTableIfNotExistsAsync(PollTableEntity.TableNameValue);
        await tableServiceClient.CreateTableIfNotExistsAsync(PollOptionTableEntity.TableNameValue);
    }
}
