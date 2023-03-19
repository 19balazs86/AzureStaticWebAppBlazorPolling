using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Client.Model;

public sealed class CreatePollFormModel
{
    [Required(AllowEmptyStrings = false), MaxLength(75)]
    public string Question { get; set; } = string.Empty;

    public IBrowserFile? Image  { get; set; }

    [ValidateClosingAt]
    public DateTime? ClosingAt { get; set; }

    [ValidateOptions]
    public List<string> Options { get; set; } = new List<string> { string.Empty };
}

public sealed class ValidateOptionsAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not List<string> options || options.Count == 0)
            return false;

        return !options.Any(x => string.IsNullOrWhiteSpace(x) || x.Length is <= 0 or >= 75);
    }
}

public sealed class ValidateClosingAtAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime closingAt)
        {
            return closingAt >= DateTime.Now.AddMinutes(5);
        }

        return true;
    }
}