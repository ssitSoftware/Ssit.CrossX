namespace Ssit.CrossX.Core;

public static class Blurs
{
    public static readonly float[][] Gaussian7X7 =
    [
        [  1,   4,   8,  10,   8,   4,   1 ],
        [  4,  13,  28,  36,  28,  13,   4 ],
        [  8,  28,  59,  76,  59,  28,   8 ],
        [ 10,  36,  76,  99,  76,  36,  10 ],
        [  8,  28,  59,  76,  59,  28,   8 ],
        [  4,  13,  28,  36,  28,  13,   4 ],
        [  1,   4,   8,  10,   8,   4,   1 ]
    ];
    
    public static readonly float[][] Gaussian5X5 =
    [
        [0,4,7,4,0],
        [4,16,26,16,4],
        [7,26,41,26,7],
        [4,16,26,16,4],
        [0,4,7,4,0],
    ];

    public static readonly float[][] None1x1 =
    [
        [1]
    ];
    
    public static readonly float[][] Gaussian3X3 =
    [
        [0,1,0],
        [1,5,1],
        [0,1,0]
    ];
    
    public static readonly float[][] Gaussian2X2 =
    [
        [0,1,0],
        [0,3,1],
        [0,0,0]
    ];
    
    public static readonly float[][] OptimizedGaussian5X5 =
    [
        [0,0,9,0,0],
        [0,18,30,18,0],
        [9,30,41,30,9],
        [0,18,30,18,0],
        [0,0,9,0,0],
    ];
    
    public const float Gaussian2X2Divider = 5;
    
    public const float Gaussian3X3Divider = 9;
    public const float Gaussian5X5Divider = 256;
    public const float Gaussian5X5DividerLight = 192;
    public const float Gaussian7X7Divider = 1199;
}