using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;
using Ssit.CrossX.UI.Parameters;

namespace SampleGame.Game.UI;

public class MainPage: Page<MainPageViewModel>
{
    protected override View CreateView()
    {
        var templates = GetContainer<Templates>();

        return new Container
        {
            HorizontalAlign = Align.Fill,
            Children = [
                
                new Label
                {
                    Text = ViewModel.Title,
                    AnchorX = 10,
                    AnchorY = 10,
                    Width = 100,
                    Height = 100,
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Start
                }.ApplyStyles(Styles, "Title", "Label"),
                
                new Label
                {
                    Text = "Hello World!",
                    AnchorX = 10,
                    AnchorY = 10,
                    Width = 100,
                    Height = 100,
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Start
                }.ApplyStyles(Styles, "Label"),
                
                new ListBox<TestItem>
                {
                    Items = ViewModel.Items,
                    ItemTemplate = templates.TestItemTemplate
                }]
        };
    }
}
