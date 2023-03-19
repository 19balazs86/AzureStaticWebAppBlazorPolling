using Azure;
using Azure.Data.Tables;
using BlazorApp.Shared;

namespace BlazorApp.Api.Model;

public sealed class PollTableEntity : ITableEntity
{
    public const string TableNameValue    = "Polls";
    public const string PartitionKeyValue = "Poll";

    public string Question { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosingAt { get; set; }
    public bool IsClosed { get; set; }

    public string PartitionKey { get; set; } = PartitionKeyValue;
    public string RowKey { get; set; }       = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public static PollTableEntity Create(Guid id, string question, string imageUrl, DateTime? closingAt)
    {
        return new PollTableEntity
        {
            RowKey    = id.ToString(),
            Question  = question,
            ImageUrl  = imageUrl,
            ClosingAt = closingAt,
            CreatedAt = DateTime.UtcNow
        };
    }

    // TODO: use Mapster to map table entity to DTO classes
    public PollDto ToPollDto()
    {
        return new PollDto
        {
            Id       = RowKey,
            Question = Question,
            IsClosed = IsClosed
        };
    }

    public PollPageDto ToPollPageDto(IEnumerable<PollOptionDto> options)
    {
        return new PollPageDto
        {
            Id        = RowKey,
            Question  = Question,
            ImageUrl  = ImageUrl,
            IsClosed  = IsClosed,
            ClosingAt = ClosingAt,
            Options   = options
        };
    }
}
