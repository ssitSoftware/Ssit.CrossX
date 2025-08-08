using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Nokemono.Core.Game.Objects.Enemies;

public class Slicer: SpriteGameObject
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Parameters
    {
        [EditorLink(typeof(ITarget))] public int Target { get; set; }
    }
    
    public Slicer(GameObjectsServices services, ObjectCreationParameters parameters) : base(services, parameters)
    {
        
    }
}