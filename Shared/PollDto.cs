namespace BlazorApp.Shared
{
    public sealed class PollDto
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public bool IsClosed { get; set; }
    }
}