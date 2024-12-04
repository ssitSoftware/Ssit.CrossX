using System.Text;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics.Internal;

public struct TextSource
{
    public StringBuilder Builder;
    public string String;
    public ICharProvider Provider;

    public char this[int index] =>
        String is not null ? String[index] : Builder is not null ? Builder[index] : Provider[index];
        
    public int Length => String?.Length ?? Builder?.Length ?? Provider.Length;
}