using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Template;
using Ssit.IoC;

namespace Ssit.CrossX.Games.Logic.Objects;

public class GameObjectsServices
{
    public World World { get; }
    public IContentManager ContentManager { get; }
    public IIoCContainer Container { get; }
    public IGameTemplate GameTemplate { get; }
    public ICommonSoundContainer CommonSoundContainer { get; }

    public GameObjectsServices(World world, IContentManager contentManager, IIoCContainer container, IGameTemplate gameTemplate, ICommonSoundContainer commonSoundContainer)
    {
        World = world;
        ContentManager = contentManager;
        Container = container;
        GameTemplate = gameTemplate;
        CommonSoundContainer = commonSoundContainer;
    }
}