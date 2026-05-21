using System;

namespace Ssit.CrossX.Utils;

[Flags]
public enum StopwatchComponents
{
    None = 0,
    Hours = 1,
    Minutes = 2,
    Seconds = 4,
    TenthSeconds = 8,
    Milliseconds = 16
}