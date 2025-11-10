using System.Numerics;

namespace Ssit.CrossX.XxGames.Physics.Coliders;

public class RectColliderCreationParameters : ColliderCreationParameters
{
    public Vector2 Center { get; set; }
    public SizeF Size { get; set; }
}