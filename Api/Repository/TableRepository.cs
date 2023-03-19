using Azure;
using Azure.Data.Tables;
using BlazorApp.Api.Model;
using System.Runtime.CompilerServices;

namespace BlazorApp.Api.Repository;

public sealed class TableRepository : ITableRepository
{
    private readonly TableServiceClient _tableServiceClient;

    private readonly string[] _gettPollColumns;
    private readonly string[] _gettOptionColumns;

    public TableRepository(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;

        _gettPollColumns = new string[]
        {
            nameof(PollTableEntity.RowKey),
            nameof(PollTableEntity.Question),
            nameof(PollTableEntity.IsClosed)
        };

        _gettOptionColumns = new string[]
        {
            nameof(PollOptionTableEntity.RowKey),
            nameof(PollOptionTableEntity.Text),
            nameof(PollOptionTableEntity.Counter),
            nameof(PollOptionTableEntity.Order)
        };
    }

    public async IAsyncEnumerable<PollTableEntity> GetPolls([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        TableClient pollTableClient = _tableServiceClient.GetTableClient(PollTableEntity.TableNameValue);

        AsyncPageable<PollTableEntity> asyncPagablePolls = pollTableClient.QueryAsync<PollTableEntity>(
            select: _gettPollColumns, cancellationToken: cancellationToken);

        await foreach (PollTableEntity pollTable in asyncPagablePolls)
        {
            if (cancellationToken.IsCancellationRequested)
                yield break;

            yield return pollTable;
        }
    }

    public async Task<PollTableEntity?> GetPoll(Guid pollId, CancellationToken cancellationToken = default)
    {
        TableClient pollTableClient = _tableServiceClient.GetTableClient(PollTableEntity.TableNameValue);

        NullableResponse<PollTableEntity> pollTableEntity = await pollTableClient.GetEntityIfExistsAsync<PollTableEntity>(
            PollTableEntity.PartitionKeyValue, rowKey: pollId.ToString(), cancellationToken: cancellationToken);

        return pollTableEntity.HasValue ? pollTableEntity.Value : null;
    }

    public async Task<IEnumerable<PollOptionTableEntity>> GetPollOptions(Guid pollId, CancellationToken cancellationToken = default)
    {
        string pollOptionFilter = $"{nameof(PollOptionTableEntity.PollId)} eq '{pollId}'";

        TableClient pollOptionTableClient = _tableServiceClient.GetTableClient(PollOptionTableEntity.TableNameValue);

        AsyncPageable<PollOptionTableEntity> asyncPagableOptions = pollOptionTableClient.QueryAsync<PollOptionTableEntity>(
            pollOptionFilter, select: _gettOptionColumns, cancellationToken: cancellationToken);

        var pollOptions = new List<PollOptionTableEntity>();

        await foreach (PollOptionTableEntity optionTableEntity in asyncPagableOptions.WithCancellation(cancellationToken))
        {
            pollOptions.Add(optionTableEntity);
        }

        return pollOptions;
    }

    public async Task CreatePoll(Guid pollId, string question, DateTime? closingAt, string imageUrl)
    {
        TableClient pollTableClient = _tableServiceClient.GetTableClient(PollTableEntity.TableNameValue);

        var pollTableEntity = PollTableEntity.Create(pollId, question, imageUrl, closingAt);

        await pollTableClient.AddEntityAsync(pollTableEntity);
    }

    public async Task CreatePollOptions(Guid pollId, IEnumerable<string> options)
    {
        TableClient pollOptionTableClient = _tableServiceClient.GetTableClient(PollOptionTableEntity.TableNameValue);

        int order = 0;

        foreach (string optionText in options)
        {
            var pollOptionTableEntity = PollOptionTableEntity.Create(pollId, optionText, order++);

            await pollOptionTableClient.AddEntityAsync(pollOptionTableEntity);
        }
    }

    public async Task<PollOptionTableEntity?> GetPollOption(string pollOptionid)
    {
        TableClient optionTableClient = _tableServiceClient.GetTableClient(PollOptionTableEntity.TableNameValue);

        NullableResponse<PollOptionTableEntity> optionTableEntity = await optionTableClient.GetEntityIfExistsAsync<PollOptionTableEntity>(
            PollOptionTableEntity.PartitionKeyValue, rowKey: pollOptionid);

        return optionTableEntity.HasValue ? optionTableEntity.Value : null;
    }

    public async Task UpdatePollOption(PollOptionTableEntity optionTableEntity)
    {
        TableClient optionTableClient = _tableServiceClient.GetTableClient(PollOptionTableEntity.TableNameValue);

        await optionTableClient.UpdateEntityAsync(optionTableEntity, optionTableEntity.ETag);
    }
}
