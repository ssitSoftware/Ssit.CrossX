using System.Numerics;

namespace Ssit.CrossX.XxGames.Physics.Coliders;

public class RectColliderCreationParameters : ColliderCreationParameters
{
    public Vector2 Center { get; set; } = Vector2.Zero;
    public SizeF Size { get; set; }
}