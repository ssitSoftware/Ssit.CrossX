using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;
using Ssit.CrossX.XxFormats.Template;
using Ssit.CrossX.XxGames.Audio;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Rendering;
using Ssit.IoC;

namespace Ssit.CrossX.XxGames.Logic.Objects;

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