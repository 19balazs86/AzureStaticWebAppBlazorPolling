namespace BlazorApp.Api.Services;

public interface ISchedulerService
{
    Task ScheduleClosingMessage(Guid pollId, DateTime closingAt);
}
