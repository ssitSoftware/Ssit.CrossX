using Ssit.CrossX.Games.Map;

namespace Ssit.CrossX.Games.Template;

public class LayerDescription
{
    public readonly string Name = null;
    public readonly Size Size = default;

    public readonly float HorizontalSpeed = 0;
    public readonly float VerticalSpeed = 0;

    public readonly string InitialTileset = null; 
    
    public readonly float Depth = 0;

    public readonly RgbaColor Tint;

    public readonly LayerAlign Anchor;
    
    public LayerDescription(string name, Size size, 
        float horizontalSpeed, float verticalSpeed, 
        string initialTileset, float depth, 
        RgbaColor tint, LayerAlign anchor)
    {
        Name = name;
        Size = size;
        HorizontalSpeed = horizontalSpeed;
        VerticalSpeed = verticalSpeed;
        InitialTileset = initialTileset;
        Depth = depth;
        Tint = tint;
        Anchor = anchor;
    }
}