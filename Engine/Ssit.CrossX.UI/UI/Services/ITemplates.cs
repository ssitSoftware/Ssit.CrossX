namespace Ssit.CrossX.UI.Services;

public interface ITemplates
{
    public TTemplate Get<TTemplate>() where TTemplate : TemplatesContainer;
}