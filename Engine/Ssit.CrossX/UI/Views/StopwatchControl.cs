using System;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.UI.Views;

public class StopwatchControl: View
{
    public SharedValue<DateTime?> StartTime { get; set; }
    public FontDesc Font { get; set; }
    public ColorWrapper TextColor { get; set; }
    public ColorWrapper OutlineColor { get; set; }
    public StopwatchComponents TimeComponents { get; set; } = StopwatchComponents.Minutes | StopwatchComponents.Seconds | StopwatchComponents.Milliseconds;
    public TextScaling Scaling { get; set; }
}