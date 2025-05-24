namespace Ssit.CrossX.UI.Values;

public class SharedStringSource : SharedString
{
    private SharedString _source = new SharedStringValue();
    
    public override int Length => _source.Length;
    public override char this[int index] => _source[index];
    
    public void SetSource(SharedString source)
    {
        _source = source;
        RaiseTextChanged();
    }
}