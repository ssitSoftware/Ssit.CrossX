using System.Collections.Generic;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Values;

namespace SampleGame.Game.UI;

public class TestItem
{
    public string Title = "Test";
    public List<string> Names { get; } = new ();
}

public class MainPageViewModel
{
    public SharedString Title { get; } = "Test Title";
    public List<TestItem> Items { get; } = new();
}