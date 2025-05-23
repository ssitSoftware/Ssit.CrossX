using Nokemono.Core.UI.Pages.Internal;
using Nokemono.Core.UI.ViewModels;
using Nokemono.Core.UI.Views;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

public class PausePage: MenuItemsPageBaseEx<PausePageViewModel>
{
    protected override View CreateView()
    {
        var menuView = CreateMenuItems<LabelButtonEx>("Pause",
        [
            (Translator["Resume"], ViewModel.ResumeCommand),
            (Translator["Options"], ViewModel.OptionsCommand),
            (Translator["Exit to Main Menu"], ViewModel.ExitCommand)
        ], suppressBack: true);
        
        return new Container
        {
            Children = [
                new GameView
                {
                    GameInstance = ViewModel.GameInterfaces.Instance,
                    Active = false
                },
                DialogPageHelper.CreateDialogLayer(ViewModel.GameInterfaces.Dialogs, false),
                // new ImageView
                // {
                //     Source  = "assets:/UI/Mask.png!",
                //     HorizontalAlign = Align.Fill,
                //     VerticalAlign = Align.Fill,
                //     Scaling = ImageScalingMode.Fill
                // },
                new Background
                {
                    BackgroundColor = (1, 0.9f)
                },
                new Container
                {
                    Padding = (10,10),
                    Children = [menuView]
                }
            ]
        };
    }

    // protected override VerticalStack CreateVerticalStack()
    // {
    //     return new VerticalStack
    //     {
    //         Padding = (8, 8),
    //         VerticalAlign = Align.Center,
    //         HorizontalAlign = Align.Center,
    //         BackgroundColor = 1
    //     };
    // }
}