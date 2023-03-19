using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Shared
{
    public sealed class PollPageDto
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public string ImageUrl { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? ClosingAt { get; set; }
        public IEnumerable<PollOptionDto> Options { get; set; } = Enumerable.Empty<PollOptionDto>();
    }

    public sealed class PollOptionDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int Counter { get; set; }
    }
}