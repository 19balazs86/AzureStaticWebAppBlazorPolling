﻿namespace BlazorApp.Api.Services;

public interface IClock
{
    DateTime UtcNow { get; }
}
