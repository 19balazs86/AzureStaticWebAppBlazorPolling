using BlazorApp.Api.Model;

namespace BlazorApp.Api.Repository;

public interface ITableRepository
{
    IAsyncEnumerable<PollTableEntity> GetPolls(CancellationToken cancellationToken = default);

    Task<PollTableEntity?> GetPoll(Guid pollId, CancellationToken cancellationToken = default);

    Task<IEnumerable<PollOptionTableEntity>> GetPollOptions(Guid pollId, CancellationToken cancellationToken = default);

    Task CreatePoll(Guid pollId, string question, DateTime? closingAt, string imageUrl);

    Task CreatePollOptions(Guid pollId, IEnumerable<string> options);

    Task<PollOptionTableEntity?> GetPollOption(string pollOptionid);

    Task UpdatePollOption(PollOptionTableEntity optionTableEntity);
}
