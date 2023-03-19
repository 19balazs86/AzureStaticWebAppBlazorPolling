namespace BlazorApp.Api.Services;

public interface IFileService
{
    Task<string> UploadImage(Guid pollId, Stream imageStream);
}
