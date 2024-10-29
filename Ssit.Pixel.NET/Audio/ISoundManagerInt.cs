using System;
using Ssit.Pixel.Audio;

namespace Ssit.Pixel.NET.Audio;

internal interface ISoundManagerInt: ISoundManager
{
    event Action<float> MasterVolumeUpdated;
}