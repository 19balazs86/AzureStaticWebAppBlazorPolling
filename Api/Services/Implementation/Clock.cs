namespace BlazorApp.Api.Services.Implementation;

public sealed class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
