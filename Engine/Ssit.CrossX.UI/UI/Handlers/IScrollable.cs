using System.Numerics;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Handlers;

public interface IScrollable
{
    ScrollMode ScrollMode { get; }
    
    SizeF ContentSize { get; }
    SizeF ViewSize { get; }
    
    void UpdateScrollPosition(Vector2 offset);
}