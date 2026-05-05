namespace Ssit.CrossX.XxGames.Physics.Coliders;

public abstract class ColliderCreationParameters
{
    public string Name { get; set; }
    public IBody AttachToBody { get; set; }
    public IMaterial Material { get; set; }
    public ColliderType Type { get; set; }
    public bool Active { get; set; }
}