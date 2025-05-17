using Ssit.CrossX;

namespace RetroGunslinger.Core;

public class Palette
{
    public const byte Transparent = 0;
    public const byte Background = 1;
    public const byte Foreground = 2;
    public const byte Dim = 3;
    public const byte Dimmer = 4;
    public const byte Accent = 5;

    public static readonly Palette[] Palettes =
    [
        new("Default", RgbaColor.Transparent, 0x100008, 0xfff1e8, 0x5f574f, 0x3b302f, 0xff004d),
        new("Contrast", RgbaColor.Transparent, RgbaColor.Black, RgbaColor.White, 0x606060, 0x303030, RgbaColor.Red),
        new("Soft", RgbaColor.Transparent, 0x100800, 0xddd8d0, 0x706868,  0x403438, 0xc04040),
        new("Purple", RgbaColor.Transparent, 0x150413, 0xffffd1, 0x332431, 0x191218, 0x6e3c3c),
        new("Silver", RgbaColor.Transparent, 0x080808, 0xb6bac5, 0x4d5465, 0x272a32, 0xae4080),
        new("Cold", RgbaColor.Transparent, 0x08001f, 0x8f929c, 0x3e4452, 0x1f2229,0xb4643d),
        new("Marron", RgbaColor.Transparent, 0x340011, 0xf2b63f, 0x9c2032, 0x681022, 0xff0064),
        new("Chocolate", RgbaColor.Transparent, 0x3b313c, 0xedd8bd, 0x8b5760, 0x63444e, 0xab313c),
        new("LCD", RgbaColor.Transparent, 0x101010, 0xa6f76e, 0x5b843f, 0x2d421f, 0xfe6a26),
        new("Amber", RgbaColor.Transparent, 0x201000, 0xfda813, 0x462800, 0x331c00, 0xfd4713),
        new("Fav1", RgbaColor.Transparent, 0x101010, 0xfda880, 0x562820, 0x331c18, 0xfe3a26),
        new("Fav2", RgbaColor.Transparent, 0x101010, 0xd6d66e, 0x5b442f, 0x362a20, 0xee5436)
    ];
    
    public string Name { get; }
    public RgbaColor[] Colors { get; }
    
    public Palette(string name, params RgbaColor[] colors)
    {
        Name = name;
        Colors = colors;
    }
}