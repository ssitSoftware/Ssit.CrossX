using System;
using Metal;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

public static class Conversions
{
    public static MTLClearColor ToMetal(this RgbaColor color) => new(color.Rf, color.Bf, color.Gf, color.Af);

    public static MTLPrimitiveType ToMetal(this PrimitiveType pt)
    {
        switch (pt)
        {
            case PrimitiveType.Triangles:
                return MTLPrimitiveType.Triangle;
            
            case PrimitiveType.Lines:
                return MTLPrimitiveType.Line;
        }

        throw new NotSupportedException();
    }
}