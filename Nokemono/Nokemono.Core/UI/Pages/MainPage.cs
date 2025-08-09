using System.Numerics;
using Nokemono.Core.UI.Pages.Internal;
using Nokemono.Core.UI.ViewModels;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Transitions;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

internal class MainPage: MenuItemsPageBaseEx<MainPageViewModel>
{
    protected override void OnLoad(IInputContext inputContext)
    {
        base.OnLoad(inputContext);
        TransitionTime = 0.3f;
        Services.Get<IMusicPlayer>().ChangePlaylist("Menu");
    }
    
    protected override View CreateView()
    {
        var menuView = CreateMenuItems<LabelButtonEx>("MainMenu",
        [
            (Translator["Start Game"], ViewModel.StartGameCommand),
            (Translator["Options"], ViewModel.OptionsCommand),
            (Translator["Credits"], ViewModel.OptionsCommand),
            (Translator["Exit"], ViewModel.ExitCommand)
        ]);

        ITransition[] logoTransitions =
        [
            new TranslationTransition
            {
                ForTransitions = TransitionType.Navigation,
                Offset = new Vector2(0, -200),
                Power = 3,
                ProgressMin = 0.125f,
                ProgressMax = 0.75f
            }
        ];
        
        return new Container
        {
            Children = [
                new ImageView
                {
                    Source = "assets:/UI/PoweredBy.png!",
                    AnchorX = "100%-10",
                    AnchorY = "100%-10",
                    HorizontalAlign = Align.End,
                    VerticalAlign = Align.End,
                    Scaling = ImageScalingMode.None,
                    Width = Length.Auto,
                    Height = Length.Auto,
                    Transitions = [
                        new TranslationTransition
                        {
                            ForTransitions = TransitionType.NavigateFrom | TransitionType.NavigateBackTo,
                            Offset = new Vector2(240, 0),
                            Power = 2,
                            ProgressMin = 0.25f
                        }
                    ]
                },
                new ImageView
                {
                  Source  = "assets:/UI/Logo.png!",
                  AnchorX = "50%",
                  HorizontalAlign = Align.Center,
                  VerticalAlign = Align.Start,
                  AnchorY = 10,
                  Scaling = ImageScalingMode.None,
                  Width = Length.Auto,
                  Height = Length.Auto,
                  Transitions = logoTransitions
                },
                new Label
                {
                    Text = Translator["THE LOST BARRELS"],
                    HorizontalAlign = Align.End,
                    TextAlign = ContentAlign.Right,
                    VerticalAlign = Align.Start,
                    AnchorY = 70,
                    AnchorX = "84%",
                    Font = ("Default", 12),
                    TextColor = Palette.Foreground,
                    TextOutlineColor = Palette.Background,
                    Scaling = TextScaling.Pixel,
                    Transitions = logoTransitions
                },
                menuView,
                new Label
                {
                    Text = "© 2025 ebatianoGames™.\n" + Translator["All rights reserved."],
                    AnchorX = "33%",
                    AnchorY = "100%-8",
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.End,
                    Font = ("Default", 12),
                    TextColor = Palette.Dim,
                    TextOutlineColor = Palette.Background,
                    Scaling = TextScaling.Pixel,
                    Transitions = [
                            new TranslationTransition
                            {
                                ForTransitions = TransitionType.Navigation,
                                Offset = new Vector2(0, 60),
                                Power = 3,
                                ProgressMin = 0.5f
                            }
                        ]
                }
            ]
        };
    }

    protected override void MenuItemApplyStyle(LabelButton button)
    {
        base.MenuItemApplyStyle(button);
        
        button.Font = ("Default", 22);
        button.Height = 26;
    }

    protected override void MenuApplyStyle(VerticalStack stack)
    {
        base.MenuApplyStyle(stack);
        stack.AnchorY = "50%+15";
        stack.AnchorX = "33%";
        stack.Width = "50%";
        stack.HorizontalAlign = Align.Center;
        stack.Transitions =
        [
            new TranslationTransition
            {
                ForTransitions = TransitionType.Navigation,
                Offset = new Vector2(-300, 0),
                Power = 3,
                ProgressMin = 0.1f,
                ProgressMax = 0.8f,
            }
        ];
    }
}