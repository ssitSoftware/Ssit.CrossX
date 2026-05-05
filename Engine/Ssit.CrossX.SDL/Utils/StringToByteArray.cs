using System.Text;

namespace Ssit.CrossX.SDL.Utils;

public static class StringToByteArray
{
    public static readonly object Lock = new object();
    private static byte[] _buffer = new byte[100];

    public static byte[] GetBytes(string text, Encoding encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var length = encoding.GetByteCount(text) + 10;
        if (_buffer.Length < length)
        {
            _buffer = new byte[length];
            for (var idx = 0; idx < length; idx++)
            {
                _buffer[idx] = 0;
            }
        }
        
        encoding.GetBytes(text, 0, text.Length, _buffer, 0);
        return _buffer;
    }
}