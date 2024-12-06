using System;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.UI.Values;

public abstract class SharedString: CharProvider
{
    private int _hashCode = -1;
    
    public event Action TextChanged;
    
    public static implicit operator SharedString(string value) => new SharedStringValue(value);
    public static SharedString operator + (SharedString str1, SharedString str2) => new SharedStringJoin(str1, str2);
    
    protected void RaiseTextChanged()
    {
        TextChanged?.Invoke();
        _hashCode = -1;
    }

    public override int GetHashCode()
    {
        if (_hashCode == -1)
        {
            CalculateHashCode();
        }
        return _hashCode;
    }

    protected void CalculateHashCode()
    {
        const int prime = 37;
        int result = 1;

        for (var idx = 0; idx < Length; ++idx)
        {
            result = result * prime + this[idx].GetHashCode();
        }
    }
}