using System;

namespace Ssit.CrossX.Utils;

public class MersenneTwister
{
    /// <summary>
    /// N
    /// </summary>
    private static readonly int N = 624;

    /// <summary>
    /// M
    /// </summary>
    private static readonly int M = 397;

    /// <summary>
    /// Constant vector a
    /// </summary>
    private readonly UInt32 _matrixA = 0x9908b0df;

    /// <summary>
    /// most significant w-r bits
    /// </summary>
    private readonly UInt32 _upperMask = 0x80000000;

    /// <summary>
    /// least significant r bits
    /// </summary>
    private readonly UInt32 _lowerMask = 0x7fffffff;

    /// <summary>
    /// Tempering mask B
    /// </summary>
    private readonly UInt32 _temperingMaskB = 0x9d2c5680;

    /// <summary>
    /// Tempering mask C
    /// </summary>
    private readonly UInt32 _temperingMaskC = 0xefc60000;

    /// <summary>
    /// Last constant used for generation
    /// </summary>
    private readonly double _finalConstant = 2.3283064365386963e-10;
    
    private static volatile MersenneTwister _instance;

    private static object _syncRoot = new Object();

    public static MersenneTwister Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new MersenneTwister();
                    }
                }
            }
            return _instance;
        }
    }

    private MersenneTwister()
    {
        //init
        Sgenrand(4327);
    }
    
    private ulong TEMPERING_SHIFT_U(ulong y)
    {
        return y >> 11;
    }

    private ulong TEMPERING_SHIFT_S(ulong y)
    {
        return y << 7;
    }

    private ulong TEMPERING_SHIFT_T(ulong y)
    {
        return y << 15;
    }

    private ulong TEMPERING_SHIFT_L(ulong y)
    {
        return y >> 18;
    }

    /// <summary>
    /// the array for the state vector
    /// </summary>
    private readonly ulong[] _mt = new ulong[625];

    /// <summary>
    /// mti==N+1 means mt[N] is not initialized 
    /// </summary>
    private int _mti = N + 1;
    
    /// <summary>
    /// setting initial seeds to mt[N] using
    /// the generator Line 25 of Table 1 in
    /// [KNUTH 1981, The Art of Computer Programming Vol. 2 (2nd Ed.), pp102] 
    /// </summary>
    /// <param name="seed"></param>
    private void Sgenrand(ulong seed)
    {
        _mt[0] = seed & 0xffffffff;
        for (_mti = 1; _mti < N; _mti++)
            _mt[_mti] = (69069 * _mt[_mti - 1]) & 0xffffffff;
    }

    private double Genrand()
    {
        ulong y;
        ulong[] mag01 = new ulong[2] { 0x0, _matrixA };
        /* mag01[x] = x * MATRIX_A  for x=0,1 */

        if (_mti >= N)
        { /* generate N words at one time */
            int kk;

            if (_mti == N + 1)   /* if sgenrand() has not been called, */
                Sgenrand(4357); /* a default initial seed is used   */

            for (kk = 0; kk < N - M; kk++)
            {
                y = (_mt[kk] & _upperMask) | (_mt[kk + 1] & _lowerMask);
                _mt[kk] = _mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1];
            }
            for (; kk < N - 1; kk++)
            {
                y = (_mt[kk] & _upperMask) | (_mt[kk + 1] & _lowerMask);
                _mt[kk] = _mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1];
            }
            y = (_mt[N - 1] & _upperMask) | (_mt[0] & _lowerMask);
            _mt[N - 1] = _mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1];

            _mti = 0;
        }

        y = _mt[_mti++];
        y ^= TEMPERING_SHIFT_U(y);
        y ^= TEMPERING_SHIFT_S(y) & _temperingMaskB;
        y ^= TEMPERING_SHIFT_T(y) & _temperingMaskC;
        y ^= TEMPERING_SHIFT_L(y);

        //reals: (0,1)-interval
        //return y; for integer generation
        return ((double)y * _finalConstant);
    }
    
    /// <summary>
    /// Generate a random number between 0 and 1
    /// </summary>
    /// <returns></returns>
    public double Generate()
    {
        return Genrand();
    }

    /// <summary>
    /// Generate an int between two bounds
    /// </summary>
    /// <param name="lowerBound">The lower bound (inclusive)</param>
    /// <param name="higherBound">The higher bound (inclusive)</param>
    /// <returns></returns>
    public double Generate(int lowerBound, int higherBound)
    {
        if (higherBound < lowerBound)
        {
            return double.NaN;
        }
        return Convert.ToInt32(Math.Floor(Generate(lowerBound * 1.0d, higherBound * 1.0d)));
    }

    /// <summary>
    /// Generate a double between two bounds
    /// </summary>
    /// <param name="lowerBound">The lower bound (inclusive)</param>
    /// <param name="higherBound">The higher bound (inclusive)</param>
    /// <returns>The random num or NaN if higherbound is lower than lowerbound</returns>
    public double Generate(double lowerBound, double higherBound)
    {
        if (higherBound < lowerBound)
        {
            return double.NaN;
        }
        return (Generate() * (higherBound - lowerBound + 1)) + lowerBound;
    }
}