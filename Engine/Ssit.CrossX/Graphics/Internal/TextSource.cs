using System;
using System.Text;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics.Internal;

public readonly struct TextSource
{
    private readonly StringBuilder _builder;
    private readonly string _string;
    private readonly ICharProvider _provider;
    private readonly int _start;

    public int Length { get; }
    
    public TextSource(string str, int start = 0, int length = -1) 
    {
        _string = str;
        _builder = null;
        _provider = null;
        _start = start;
        Length = length >= 0  ? length : str.Length - start;
        Length = Math.Min(Length, str.Length - start);
    }
    
    public TextSource(StringBuilder str, int start = 0, int length = -1)
    {
        _string = null;
        _builder = str;
        _provider = null;
        _start = start;
        Length = length >= 0  ? length : str.Length - start;
        Length = Math.Min(Length, str.Length - start);
    }
    
    public TextSource(ICharProvider str, int start = 0, int length = -1)
    {
        _string = null;
        _builder = null;
        _provider = str;
        Length = length >= 0  ? length : str.Length - start;
        Length = Math.Min(Length, str.Length - start);
    }
    
    public char this[int index] =>
        _string is not null ? _string[index + _start] : _builder is not null ? _builder[index + _start] : _provider[index + _start];
}