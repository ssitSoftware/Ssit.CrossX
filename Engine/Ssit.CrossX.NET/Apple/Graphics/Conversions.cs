#if __IOS__ || __MACCATALYST__

using System;
using Metal;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.NET.Apple.Graphics;

public static class Conversions
{
    public static MTLClearColor ToMetal(this RgbaColor color) => new(color.Rf, color.Gf, color.Bf, color.Af);

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

#endif