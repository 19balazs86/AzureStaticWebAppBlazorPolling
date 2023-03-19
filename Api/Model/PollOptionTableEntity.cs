using Azure;
using Azure.Data.Tables;
using BlazorApp.Shared;

namespace BlazorApp.Api.Model;

public sealed class PollOptionTableEntity : ITableEntity
{
    public const string TableNameValue    = "PollOptions";
    public const string PartitionKeyValue = "PollOption";

    public string PollId { get; set; } = string.Empty;
    public string Text { get; set; }   = string.Empty;
    public bool IsClosed { get; set; }
    public int Counter { get; set; }
    public int Order { get; set; }

    public string PartitionKey { get; set; } = PartitionKeyValue;
    public string RowKey { get; set; }       = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public static PollOptionTableEntity Create(Guid pollId, string text, int order)
    {
        return new PollOptionTableEntity
        {
            RowKey = Guid.NewGuid().ToString(),
            PollId = pollId.ToString(),
            Text   = text,
            Order  = order,
        };
    }

    // TODO: use Mapster to map table entity to DTO class
    public PollOptionDto ToPollOptionDto()
    {
        return new PollOptionDto
        {
            Id      = RowKey,
            Text    = Text,
            Counter = Counter
        };
    }
}
