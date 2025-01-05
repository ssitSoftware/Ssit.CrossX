using System;
using SampleGame.UI.Views;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace SampleGame.UI.Pages;

public class GamePage: Page<GamePageViewModel>
{
    protected override bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        if (button == UiButton.Left)
        {
            var value = Math.Max(0, ViewModel.HitPoints.Value - 1);
            ViewModel.HitPoints.Value = value;
            return true;
        }
        
        if (button == UiButton.Right)
        {
            var value = Math.Min(ViewModel.MaxHitPoints.Value, ViewModel.HitPoints.Value + 1);
            ViewModel.HitPoints.Value = value;
            return true;
        }
        
        if (button == UiButton.Down)
        {
            var value = Math.Max(0, ViewModel.Rounds.Value - 1);
            ViewModel.Rounds.Value = value;
            return true;
        }
        
        if (button == UiButton.Up)
        {
            var value = Math.Min(ViewModel.MaxRounds.Value, ViewModel.Rounds.Value + 1);
            ViewModel.Rounds.Value = value;
            return true;
        }
        return base.OnUiButton(button, inputContext);
    }

    protected override View CreateView()
    {
        return new Container
        {
            Children = [
               new GameView
               {
               },
               new PointsView
               {
                   Path = "assets:/Sprites/HP",
                   Spacing = 3,
                   MaxPoints = ViewModel.MaxHitPoints,
                   Points = ViewModel.HitPoints,
                   HorizontalAlign = Align.Start,
                   VerticalAlign = Align.Start,
                   AnchorX = 10,
                   AnchorY = 10,
               },
               new PointsView
               {
                   Path = "assets:/Sprites/Rounds",
                   Spacing = 2,
                   MaxPoints = ViewModel.MaxRounds,
                   Points = ViewModel.Rounds,
                   HorizontalAlign = Align.Start,
                   VerticalAlign = Align.Start,
                   AnchorX = 11,
                   AnchorY = 22,
               }
            ]
        };
    }
}