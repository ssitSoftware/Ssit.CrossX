using System;

namespace Ssit.CrossX.Graphics.Utils;

public static class Technicolor
{
    const float Amount1 = 30; //[0, 100]Red weight
    const float Amount2 = 59; //[0, 100]Green weight
    const float Amount3 = 11; //[0, 100]Blue weight

    public static RgbaColor ToTechniColor2(RgbaColor color)
    {
        int r, r2, c;
        int g, g2, m;
        int b, b2, y, l;

        float rWeight = Amount1 / 100.0f;
        float gWeight = Amount2 / 100.0f;
        float bWeight = Amount3 / 100.0f;

        

        r = color.R;
        g = color.G;
        b = color.B;

        c = Gray(color, 0, gWeight, bWeight);
        m = Gray(color, rWeight, 0, bWeight);
        y = Gray(color, rWeight, gWeight, 0);
            
        r2 = 255 - Clamp(r - c);
        g2 = 255 - Clamp(g - m);
        b2 = 255 - Clamp(b - y);

        r -= 255 - ((g2 * b2) >> 8);
        g -= 255 - ((r2 * b2) >> 8);
        b -= 255 - ((g2 * r2) >> 8);
            
        var nR = Clamp(r);
        var nG = Clamp(g);
        var nB = Clamp(b);

        l = Gray(new RgbaColor(nR, nG, nB), rWeight, gWeight, bWeight);

        nB = nG;
        nG = Clamp(l);
        
        return new RgbaColor(nR, nG, nB, color.A);
    }
    
    public static RgbaColor ToTechniColor3(RgbaColor color)
    {
        int r, r2, c;
        int g, g2, m;
        int b, b2, y;

        float rWeight = Amount1 / 100.0f;
        float gWeight = Amount2 / 100.0f;
        float bWeight = Amount3 / 100.0f;

        
        r = color.R;
        g = color.G;
        b = color.B;
        
        c = Gray(color, 0, gWeight, bWeight);
        m = Gray(color, rWeight, 0, bWeight);
        y = Gray(color, rWeight, gWeight, 0);

        r2 = 255 - Clamp(r - c);
        g2 = 255 - Clamp(g - m);
        b2 = 255 - Clamp(b - y);

        r -= 255 - ((g2 * b2) >> 8);
        g -= 255 - ((r2 * b2) >> 8);
        b -= 255 - ((g2 * r2) >> 8);
    
        r = Clamp(r);
        g = Clamp(g);
        b = Clamp(b);
    
        return new RgbaColor(Clamp(r), Clamp(g), Clamp(b), color.A); 
    }

    private static byte Clamp(int num)
    {
        return (byte)Math.Max(0, Math.Min(255, num));
    }

    private static byte Gray(RgbaColor col, float wr, float wg, float wb)
    {
        float r = col.R, g = col.G, b = col.B;
        var sum = Math.Abs(wr + wg + wb);
    
        var res = (byte)(r * (wr/sum) + g * (wg/sum) + b * (wb/sum));
        return res;
    }
}