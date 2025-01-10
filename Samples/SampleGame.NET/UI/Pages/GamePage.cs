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
    protected override View CreateView()
    {
        return new Container
        {
            Children = [
               new GameView
               {
                   Simulation = ViewModel.Simulation
               },
               new PointsView
               {
                   Path = "assets:/Sprites/HP",
                   Spacing = 3,
                   MaxPoints = ViewModel.MaxHitPoints,
                   Points = ViewModel.HitPoints,
                   HorizontalAlign = Align.End,
                   VerticalAlign = Align.Start,
                   AnchorX =  "100%-8",
                   AnchorY = 8,
               },
               new PointsView
               {
                   Path = "assets:/Sprites/Rounds",
                   Spacing = 2,
                   MaxPoints = ViewModel.MaxRounds,
                   Points = ViewModel.Rounds,
                   HorizontalAlign = Align.End,
                   VerticalAlign = Align.Start,
                   AnchorX  = "100%-9",
                   AnchorY = 22
               }
            ]
        };
    }
}