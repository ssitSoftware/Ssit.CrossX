using System.Globalization;
using System.Text.RegularExpressions;

namespace Ssit.CrossX.UI.Parameters;

public struct Length
{
    public static readonly Length Auto = new(float.NaN);
    public static readonly Length Fill = new(0,1);
    public static readonly Length Zero = new(0);
    
    public bool IsAuto => float.IsNaN(_value);

    internal Length(float value, float percent = 0)
    {
        _value = value;
        _percent = percent;
    }

    private Length(string str)
    {
        var parts = Regex.Split(str, @"(?<=[+-])");

        float value = 0;
        float percent = 0;

        float sign = 1;

        foreach (var part in parts)
        {
            switch (part)
            {
                case "+":
                    sign = 1;
                    break;
                
                case "-":
                    sign = -1;
                    break;
                
                case "C":
                    percent += sign * 0.5f;
                    break;
                
                case "@":
                    percent += sign;
                    break;

                default:
                {
                        if (part.EndsWith('%'))
                        {
                            percent += sign * float.Parse(part.Substring(0, part.Length - 1), NumberStyles.Float,
                                CultureInfo.InvariantCulture) / 100f;
                        }
                        else
                        {
                            value += sign * float.Parse(part, NumberStyles.Float, CultureInfo.InvariantCulture);
                        }
                }
                break;
            }
        }
        
        _value = value;
        _percent = percent;
    }
    
    public float Calculate(float reference = 0)
    {
        return IsAuto ? 0 : _value + _percent * reference;
    }

    private readonly float _value;
    private readonly float _percent;

    
    
    public static implicit operator Length(float value) => new(value);
    public static implicit operator Length(string str) => new Length(str);
}
