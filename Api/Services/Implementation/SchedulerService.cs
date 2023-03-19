using Azure.Storage.Queues;

namespace BlazorApp.Api.Services.Implementation;

public sealed class SchedulerService : ISchedulerService
{
    public const string PollClosingQueueName = "poll-closing";

    private readonly TimeSpan _maxVisibilityTimeout = TimeSpan.FromDays(7);

    private readonly QueueServiceClient _queueServiceClient;
    private readonly IClock _clock;

    public SchedulerService(QueueServiceClient queueServiceClient, IClock clock)
    {
        _queueServiceClient = queueServiceClient;
        _clock = clock;
    }

    public async Task ScheduleClosingMessage(Guid pollId, DateTime closingAt)
    {
        var binaryData = BinaryData.FromString(pollId.ToString());

        TimeSpan visibilityTimeout = closingAt - _clock.UtcNow;

        // Storage queue VisibilityTimeout can not be greater than 7 days
        if (visibilityTimeout > _maxVisibilityTimeout)
            visibilityTimeout = _maxVisibilityTimeout;

        QueueClient queueClient = _queueServiceClient.GetQueueClient(PollClosingQueueName);

        // If you want to update closing date, you need to save the MessageId and the PopReceipt from the response
        await queueClient.SendMessageAsync(binaryData, visibilityTimeout);
    }
}
