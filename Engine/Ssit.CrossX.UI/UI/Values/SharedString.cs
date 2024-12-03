using System;
using System.Text;

namespace Ssit.CrossX.UI.Values;

public abstract class SharedString
{
    public event Action TextChanged;
    
    public abstract int Length { get; }
    public abstract char this[int index] { get; }
    
    public static implicit operator SharedString(string value) => new SharedStringValue(value);
    public static SharedString operator + (SharedString str1, SharedString str2) => new SharedStringJoin(str1, str2);
    
    protected void RaiseTextChanged() => TextChanged?.Invoke();
}