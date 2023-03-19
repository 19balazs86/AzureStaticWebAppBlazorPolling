using BlazorApp.Shared;

namespace BlazorApp.Api.Services;

public interface IPollService
{
    IAsyncEnumerable<PollDto> GettPolls(CancellationToken cancellationToken = default);

    Task<PollPageDto?> GettPoll(Guid pollId, CancellationToken cancellationToken = default);

    Task<Guid> CreatePoll(string question, IEnumerable<string> options, DateTime? closingAt, Stream? imageStream);
}
