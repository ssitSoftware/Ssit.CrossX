using System;
using Ssit.Pixel.UI.Parameters;

namespace Ssit.Pixel.UI;

public abstract class View
{
    public Guid Guid { get; } = Guid.NewGuid();
    
    public Align? HorizontalAlign { get; set; }
    public Align? VerticalAlign { get; set; }
    
    public Length? AnchorX { get;set; }
    public Length? AnchorY { get;set; }
    
    public Length? Width { get; set; }
    public Length? Height { get; set; }
}