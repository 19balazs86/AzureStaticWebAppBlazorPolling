using BlazorApp.Api.Model;
using BlazorApp.Api.Repository;
using BlazorApp.Shared;
using System.Runtime.CompilerServices;

namespace BlazorApp.Api.Services.Implementation;

public sealed class PollService : IPollService
{
    private readonly IFileService _fileService;
    private readonly ISchedulerService _schedulerService;
    private readonly ITableRepository _pollTableRepository;
    private readonly IClock _clock;

    public PollService(
        IFileService fileService,
        ISchedulerService schedulerService,
        ITableRepository pollTableRepository,
        IClock clock)
    {
        _fileService         = fileService;
        _schedulerService    = schedulerService;
        _pollTableRepository = pollTableRepository;
        _clock               = clock;
    }

    public async IAsyncEnumerable<PollDto> GettPolls([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (PollTableEntity pollTable in _pollTableRepository.GetPolls(cancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                yield break;

            yield return pollTable.ToPollDto();
        }
    }

    public async Task<PollPageDto?> GettPoll(Guid pollId, CancellationToken cancellationToken = default)
    {
        IEnumerable<PollOptionTableEntity> optionTableEntities = await _pollTableRepository.GetPollOptions(pollId, cancellationToken);

        IEnumerable<PollOptionDto> pollOptionDtos = optionTableEntities
            .OrderBy(x => x.Order)
            .Select(x => x.ToPollOptionDto())
            .ToArray();

        PollTableEntity? pollTableEntity = await _pollTableRepository.GetPoll(pollId, cancellationToken);

        if (pollTableEntity is null)
            return null;

        return pollTableEntity.ToPollPageDto(pollOptionDtos);
    }

    public async Task<Guid> CreatePoll(string question, IEnumerable<string> options, DateTime? closingAt, Stream? imageStream)
    {
        Guid pollId = Guid.NewGuid();

        string imageUrl = await createPoll_UploadPollImage(pollId, imageStream);

        await createPoll_AddTableEntities(pollId, question, options, imageUrl, closingAt);

        await createPoll_ScheduleClosingMessage(pollId, closingAt);

        return pollId;
    }

    private async ValueTask<string> createPoll_UploadPollImage(Guid pollId, Stream? imageStream)
    {
        if (imageStream is null)
            return string.Empty;

        return await _fileService.UploadImage(pollId, imageStream);
    }

    private async Task createPoll_AddTableEntities(Guid pollId, string question, IEnumerable<string> options, string imageUrl, DateTime? closingAt)
    {
        await _pollTableRepository.CreatePoll(pollId, question, closingAt, imageUrl);

        await _pollTableRepository.CreatePollOptions(pollId, options);
    }

    private async ValueTask createPoll_ScheduleClosingMessage(Guid pollId, DateTime? closingAt)
    {
        if (closingAt is null || closingAt.Value < _clock.UtcNow)
            return;

        await _schedulerService.ScheduleClosingMessage(pollId, closingAt.Value);
    }
}
