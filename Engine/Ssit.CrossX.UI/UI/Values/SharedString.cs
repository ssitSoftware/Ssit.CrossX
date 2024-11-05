namespace Ssit.CrossX.UI.Values;

public class SharedString
{
    public string Value { get; set; }
    public static implicit operator SharedString(string value) => new() { Value = value };
}