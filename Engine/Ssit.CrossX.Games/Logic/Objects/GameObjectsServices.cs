using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.XxFormats.Template;
using Ssit.CrossX.XxGames.Physics;
using Ssit.IoC;

namespace Ssit.CrossX.Games.Logic.Objects;

public class GameObjectsServices(
    ISimulation simulation,
    IContentManager contentManager,
    IIoCContainer container,
    IGameTemplate gameTemplate,
    ICommonSoundContainer commonSoundContainer,
    IParticleSystem particleSystem)
{
    public ISimulation Simulation { get; } = simulation;
    public IContentManager ContentManager { get; } = contentManager;
    public IIoCContainer Container { get; } = container;
    public IGameTemplate GameTemplate { get; } = gameTemplate;
    public ICommonSoundContainer CommonSoundContainer { get; } = commonSoundContainer;
    public IParticleSystem ParticleSystem { get; } = particleSystem;
}