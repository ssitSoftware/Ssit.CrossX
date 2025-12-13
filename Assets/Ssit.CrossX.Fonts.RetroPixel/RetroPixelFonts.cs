using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.IO;
// ReSharper disable InconsistentNaming

namespace Ssit.CrossX.Fonts.RetroPixel;

public static class RetroPixelFonts
{
    public const string PxPlus_IBM_EGA_8x8 = nameof(PxPlus_IBM_EGA_8x8);
    public const string PxPlus_IBM_EGA_9x14 = nameof(PxPlus_IBM_EGA_9x14);
    public const string PxPlus_Tandy1K_II_200L = nameof(PxPlus_Tandy1K_II_200L);
    public const string PxPlus_ToshibaSat_9x16 = nameof(PxPlus_ToshibaSat_9x16);
    public const string PxPlus_Rainbow100_re_40 = nameof(PxPlus_Rainbow100_re_40);
    
    public static AggregatedFilesProvider AddRetroPixelFontsDrive(this AggregatedFilesProvider aggregator) => 
        aggregator.AddProvider("RetroPixel:", new EmbeddedFilesProvider(typeof(RetroPixelFonts).Assembly, "Ssit.CrossX.Fonts.RetroPixel.Fonts"));
        
    public static IFontsManager LoadRetroPixelFonts(this IFontsManager fontsManager)
    {
        fontsManager.LoadFonts("RetroPixel:/Fonts.json");
        return fontsManager;
    }
}