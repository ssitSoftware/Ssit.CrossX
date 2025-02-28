namespace Ssit.CrossX.Core;

public static class Blurs
{
    public static readonly float[][] Gaussian5X5 =
    [
        [0,4,7,4,0],
        [4,16,26,16,4],
        [7,26,41,26,7],
        [4,16,26,16,4],
        [0,4,7,4,0],
    ];
    
    public static readonly float[][] OptimizedGaussian5X5 =
    [
        [0,0,9,0,0],
        [0,18,30,18,0],
        [9,30,41,30,9],
        [0,18,30,18,0],
        [0,0,9,0,0],
    ];
    
    public const float Gaussian5X5Divider = 128;
}