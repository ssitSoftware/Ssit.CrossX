using System;

namespace Ssit.CrossX.XxGames.Physics;

public interface IMaterial
{
    int Index { get; }
    float Friction { get; set; }
    float Bounce { get; set; }
    ColliderSides Sides { get; set; }
    int ColliderGroup { get; set; }
    IMaterial Clone(int? newIndex = null);
    
}