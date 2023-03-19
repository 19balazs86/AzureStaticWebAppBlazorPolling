using BlazorApp.Api.Model;
using BlazorApp.Api.Repository;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace BlazorApp.Api;

public sealed class VoteFunction
{
    private const string _voteRequestQueue = "vote-request";

    private readonly ITableRepository _tableRepository;

    public VoteFunction(ITableRepository tableRepository)
    {
        _tableRepository = tableRepository;
    }

    [Function(nameof(PostVote))]
    [QueueOutput(_voteRequestQueue)]
    public async Task<VoteRequest> PostVote(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Vote")] HttpRequestData request)
    {
        return await request.ReadFromJsonAsync<VoteRequest>();
    }

    [Function(nameof(ProcessVote))]
    public async Task ProcessVote([QueueTrigger(_voteRequestQueue)] VoteRequest voteRequest)
    {
        PollOptionTableEntity? optionTableEntity = await _tableRepository.GetPollOption(voteRequest.OptionId);

        if (optionTableEntity is null || optionTableEntity.IsClosed)
            return;

        optionTableEntity.Counter++;

        await _tableRepository.UpdatePollOption(optionTableEntity);
    }
}
