using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Components;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.UI.Views;

public class StopwatchControl: View
{
    public SharedValue<DateTime?> StartTime { get; set; }
    public FontDesc Font { get; set; }
    public ColorWrapper TextColor { get; set; }
    public ColorWrapper OutlineColor { get; set; }
    public StopwatchTimeElements TimeTimeElements { get; set; } = StopwatchTimeElements.Minutes | StopwatchTimeElements.Seconds | StopwatchTimeElements.Milliseconds;
    public TextScaling Scaling { get; set; }
    public Thickness? Padding { get; set; }
    public ContentAlign? Align { get; set; }
}