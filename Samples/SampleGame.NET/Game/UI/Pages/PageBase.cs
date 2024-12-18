using System;
using Ssit.CrossX.UI;

namespace SampleGame.Game.UI.Pages;

public abstract class PageBase<TViewModel>: Page<TViewModel> where TViewModel: class
{
    protected override float Scale => MathF.Min(ScreenSize.Width / 1280f, ScreenSize.Height / 720f);
}