using SampleGame.Game.UI.Handlers;
using SampleGame.Game.UI.ViewModels;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;

namespace SampleGame.Game.UI.Pages;

public class Game
{
    
}

public class GamePage: Page<GamePageViewModel>
{
    protected override View CreateView()
    {
        return new Container
        {
            Children = [
                new CustomDataView<Game>
                {
                    Data = new Game(),
                    CustomHandlerType = typeof(GameViewHandler)
                }
            ]
        };
    }
}