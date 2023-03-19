using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BlazorApp.Api.Services.Implementation;

public sealed class FileService : IFileService
{
    public const string ImageContainerName = "poll-images";

    private readonly BlobServiceClient _blobServiceClient;

    public FileService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadImage(Guid pollId, Stream imageStream)
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(ImageContainerName);

        string fileName = $"{pollId}.jpeg";
        var binaryData = await BinaryData.FromStreamAsync(imageStream);

        BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(binaryData);

        //await blobClient.SetAccessTierAsync(AccessTier.Cool);

        await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders { ContentType = "image/jpeg" });

        return blobClient.Uri.AbsoluteUri;
    }
}
