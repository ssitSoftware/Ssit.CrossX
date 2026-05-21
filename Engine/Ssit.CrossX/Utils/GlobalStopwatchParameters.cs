using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.Utils;

public class GlobalStopwatchParameters
{
    public FontDesc Font { get; set; }
    public ColorWrapper TextColor { get; set; }
    public ColorWrapper OutlineColor { get; set; }
    public TextScaling Scaling { get; set; }
    public StopwatchComponents TimeComponents { get; set; } = StopwatchComponents.Minutes | StopwatchComponents.Seconds | StopwatchComponents.Milliseconds;
    public SharedValue<DateTime?> StartTime { get; set; }
    public SharedBool ShouldDisplay { get; set; }
    public Thickness Padding { get; set; }
    public ContentAlign Align { get; set; } = ContentAlign.Center | ContentAlign.Top;
}
