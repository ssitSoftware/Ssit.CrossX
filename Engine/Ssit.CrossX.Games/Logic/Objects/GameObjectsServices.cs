using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Games.Template;
using Ssit.IoC;

namespace Ssit.CrossX.Games.Logic.Objects;

public class GameObjectsServices(
    World world,
    IContentManager contentManager,
    IIoCContainer container,
    IGameTemplate gameTemplate,
    ICommonSoundContainer commonSoundContainer,
    IParticleSystem particleSystem)
{
    public World World { get; } = world;
    public IContentManager ContentManager { get; } = contentManager;
    public IIoCContainer Container { get; } = container;
    public IGameTemplate GameTemplate { get; } = gameTemplate;
    public ICommonSoundContainer CommonSoundContainer { get; } = commonSoundContainer;
    public IParticleSystem ParticleSystem { get; } = particleSystem;
}