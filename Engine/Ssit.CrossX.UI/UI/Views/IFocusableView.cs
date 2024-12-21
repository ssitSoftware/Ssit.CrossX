namespace Ssit.CrossX.UI.Views;

public interface IFocusableView
{
    public string UniqueId { get; set; }
    (string left, string right) HorizontalNavigation { get; set; }
    (string up, string down) VerticalNavigation { get; set; }
}