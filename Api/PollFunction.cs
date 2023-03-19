using BlazorApp.Api.Services;
using BlazorApp.Shared;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace BlazorApp.Api;

public sealed class PollFunction
{
    private readonly IPollService _pollService;

    public PollFunction(IPollService pollService)
    {
        _pollService = pollService;
    }

    [Function(nameof(GetPolls))]
    public async Task<HttpResponseData> GetPolls(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Poll")] HttpRequestData request,
        CancellationToken cancellationToken = default)
    {
        var pollDtos = new List<PollDto>();

        // TODO: apply pagination
        await foreach(PollDto pollDto in _pollService.GettPolls(cancellationToken))
            pollDtos.Add(pollDto);

        var response = request.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(pollDtos);

        // Using IAsyncEnumerable for return is not working with Azure Function
        return response;
    }

    [Function(nameof(GetPoll))]
    public async Task<HttpResponseData> GetPoll(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Poll/{pollIdText}")] HttpRequestData request,
        string pollIdText,
        CancellationToken cancellationToken = default)
    {
        if (Guid.TryParse(pollIdText, out Guid pollId))
        {
            PollPageDto? pollPageDto = await _pollService.GettPoll(pollId, cancellationToken);

            if (pollPageDto is not null)
            {
                var response = request.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(pollPageDto);
                return response;
            }
        }

        return request.CreateResponse(HttpStatusCode.NotFound);
    }

    [Function(nameof(CreatePoll))]
    public async Task<HttpResponseData> CreatePoll(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Poll")] HttpRequestData request)
    {
        MultipartFormDataParser parser = await MultipartFormDataParser.ParseAsync(request.Body);

        string question     = parser.GetParameterValue("Question");
        string closingAtStr = parser.GetParameterValue("ClosingAt");

        DateTime? closingAt = string.IsNullOrEmpty(closingAtStr) ? null : DateTime.Parse(closingAtStr).ToUniversalTime();

        IEnumerable<string> options = parser.GetParameterValues("Option");

        FilePart? filePart = parser.Files.FirstOrDefault();

        Guid pollId = await _pollService.CreatePoll(question, options, closingAt, filePart?.Data);

        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync(pollId.ToString());

        return response;
    }

    // TODO: Validate and resize the image

    //private static async Task<MemoryStream> getImageStream(Stream filePartStream)
    //{
    //    // Install-Package SixLabors.ImageSharp

    //    using Image image = await Image.LoadAsync(filePartStream);

    //    if (needToResize(image.Size, out Size newSize))
    //        image.Mutate(x => x.Resize(newSize));

    //    var memoryStream = new MemoryStream();

    //    await image.SaveAsJpegAsync(memoryStream);

    //    memoryStream.Seek(0, SeekOrigin.Begin);

    //    return memoryStream;
    //}

    //private static bool needToResize(in Size originalSize, out Size newSize)
    //{
    //    const float maxImageSize = 200.0f;

    //    (int width, int height) = originalSize;

    //    int largerSize = Math.Max(width, height);

    //    if (largerSize <= maxImageSize)
    //    {
    //        newSize = Size.Empty;
    //        return false;
    //    }

    //    float percentage = maxImageSize / largerSize;

    //    var sizeF = new SizeF(width * percentage, height * percentage);

    //    newSize = Size.Round(sizeF);

    //    return true;
    //}
}
