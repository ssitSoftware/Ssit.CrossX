using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;
using Ssit.CrossX.UI.Parameters;

namespace SampleGame.Game.UI;

public class Templates: TemplatesContainer
{
    public View TestItemTemplate(TestItem context) => new Container
    {
        HorizontalAlign = Align.Fill,
        Height = 50,
        Children = [
            
            new Label
            {
                Text = context.Title,
                HorizontalAlign = Align.Start,
                VerticalAlign = Align.Center
            }.ApplyStyles(Styles, "Label"),
            
            new Label
            {
                Text = "YES!",
                HorizontalAlign = Align.End,
                VerticalAlign = Align.Center
            }.ApplyStyles(Styles),
            
            new ListBox<string>
            {
                Items = context.Names,
                ItemTemplate = subContext => 
                new Container
                {
                    Children = [
                        
                        new Label
                        {
                            Text = context.Title
                        }.ApplyStyles(Styles, "Label"),
                        
                        new Label
                        {
                            Text = subContext
                        }.ApplyStyles(Styles, "Label")]
                }
            }]
    };
}
