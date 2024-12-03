// CXTOOL PAGE GENERATOR
// MainPage.xml

using Ssit.CrossX.UI; //-> 4
using Ssit.CrossX.UI.Views; //-> 5
using Ssit.CrossX.UI.Parameters; //-> 6

namespace SampleGame.Game.UI; //-> 2

public class MainPage: Page<MainPageViewModel> //-> 2
{
    protected override View CreateView()
    {
        var templates = GetContainer<Templates>(); //-> 8
        
        return new Container //-> 10
        {
            HorizontalAlign = Align.Fill,
            Children = [
                
                new Label //-> 11
                {
                    Text = ViewModel.Title + "%",
                    AnchorX = 10,
                    AnchorY = 10,
                    Width = 100,
                    Height = 100,
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Start
                }.ApplyStyles(Styles, "Title", "Label"),
                
                new Label //-> 12
                {
                    Text = "Hello World!",
                    AnchorX = 10,
                    AnchorY = 10,
                    Width = 100,
                    Height = 100,
                    HorizontalAlign = Align.Center,
                    VerticalAlign = Align.Start
                }.ApplyStyles(Styles, "Label"),
                
                new ListBox<TestItem> //-> 13
                {
                    Items = ViewModel.Items,
                    ItemTemplate = templates.TestItemTemplate
                }]
        };
    }
}
