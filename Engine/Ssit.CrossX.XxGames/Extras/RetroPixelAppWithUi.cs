using System;
using System.Numerics;
using System.Reflection;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.Input;
using Ssit.CrossX.IO;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Common;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.XxFormats.Template;
using Ssit.IoC;

namespace Ssit.CrossX.XxGames.Extras;

public abstract class RetroPixelAppWithUi<TGameTemplate>(string name, RgbaColor[] palette)
    : UiPixelApp, IResourcesLoaderSettings, IDefaultSpriteConfiguration where TGameTemplate : IGameTemplate, new()
{
    protected virtual Assembly ResourceAssembly => GetType().Assembly;
    
    protected sealed override RgbaColor BackgroundColor => RgbaColor.Black;
    ContentAlign IDefaultSpriteConfiguration.OriginAlignment => GameTemplate.ObjectsOriginAlignment;

    protected readonly TGameTemplate GameTemplate = new();
    
    protected sealed override IAppHost CreateAppHost(IIoCContainer container) => container.IoCConstruct<PixelAppHost>(HostParameters = CreateAppHostParameters());

    protected virtual (string,string)[] InterfaceSounds => null;
    protected virtual IAssetsSource[] FontSources => [];
    
    protected PixelAppHost.Parameters HostParameters { get; private set; }
    
    protected PixelAppHost.Parameters CreateAppHostParameters()
    {
        const float scale = 0.5f;
        
        var mode = GameTemplate.TargetSize.Height > GameTemplate.TargetSize.Width ? PixelAppHost.Mode.Width : PixelAppHost.Mode.Height;
        
        return new PixelAppHost.Parameters
        { 
            DesignSize = GameTemplate.TargetSize,
            MaxScale = 2,
            MinScale = 2,
            Mode = mode,
            GlowParameters = new PixelAppHost.GlowParameters
            {
              Blur = Blurs.Gaussian3X3,
              BlurDivider = Blurs.Gaussian3X3Divider,
              DisplacementFactorR = new Vector2(-1f, 1.0f) * scale,
              DisplacementFactorG = new Vector2(0.0f, 0.0f) * scale,
              DisplacementFactorB = new Vector2(1f, -1.0f) * scale,
              EnableGameGlow = true,
              SelfGlowFactor = 0.3f
            },
            CrtParameters = new PixelAppHost.CrtParameters
            {
                DisplacementFactorR = new Vector2(-1f, 1.0f) * scale,
                DisplacementFactorG = new Vector2(0.0f, 0.0f) * scale,
                DisplacementFactorB = new Vector2(1f, -1.0f) * scale,
                LampGlow = 0.3f,
                LampDownSize = 6,
                Distortion = 1.05f,
                Interline = 0.2f,
                NoiseCount = 1000,
                NoiseIntensity = 0.2f
            }
        };
    }

    protected sealed override void OnDraw(IRenderer2 renderer) => base.OnDraw(renderer);
    protected sealed override void OnStart(object args) => base.OnStart(args);
    public sealed override void OnUpdate(float elapsedTime) => base.OnUpdate(elapsedTime);
    protected sealed override void PostRender(IRenderer2 renderer) => base.PostRender(renderer);

    protected sealed override void OnInitializeServices(IIoCContainerBuilder builder)
    {
        var assembly = ResourceAssembly;
        var filesProvider = new AggregatedFilesProvider();

        filesProvider.AddProvider("assets:", new EmbeddedFilesProvider(assembly, assembly.GetName().Name + ".Assets"));
        filesProvider.AddProvider("music:", new CacheFilesProvider(assembly, assembly.GetName().Name + ".Music",  "Music"));

        foreach (var fontSource in FontSources)
        {
            filesProvider.AddProvider(fontSource.DriveName, fontSource.FilesProvider);
        }
        
        ShouldAddProvider( (d, p) => filesProvider.AddProvider(d, p));
        
        base.OnInitializeServices(builder);

        builder
            .WithInstance<IFilesProvider>(filesProvider)
            .WithInstance<IGameTemplate>(GameTemplate)
            .WithIndexedRenderer(palette)
            .WithInstance<IResourcesLoaderSettings>(this)
            .WithCommonSoundsContainer()
            .WithInstance<IFileStorage>(new FilesStorage(name));

        ShouldInitializeServices(builder);
    }

    protected virtual void ShouldInitializeServices(IIoCContainerBuilder builder)
    {
    }

    protected sealed override void OnInitialize(IIoCContainer container)
    {
        var mappings = container.Get<IInputMappings>();
        ShouldMapInput(mappings);
        
        var manager = container.Get<IFontsManager>();
        LoadFonts(manager);

        base.OnInitialize(container);
        
        InitializeUiSounds();
        ShouldInitializeApp(UiApp);
    }

    private void LoadFonts(IFontsManager manager)
    {
        foreach (var fontSource in FontSources)
        {
            manager.LoadFonts(fontSource.DefinitionPath);
        }
    }

    protected abstract void ShouldInitializeApp(IUiApp uiApp);

    protected sealed override void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers)
    {
        base.OnInitializeUi(builder, navigationMap, handlers);
        builder.WithTranslator("assets:/Languages.tsv");

        ShouldInitializeNavigation(navigationMap);
        ShouldInitializeCustomViews(handlers);
    }

    protected virtual void ShouldInitializeNavigation(INavigationMap navigationMap)
    {
    }

    protected virtual void ShouldInitializeCustomViews(IHandlerMapper handlers)
    {
        
    }
    
    protected virtual void ShouldAddProvider(Action<string, IFilesProvider> addProvider)
    {
    }

    protected virtual void ShouldMapInput(IInputMappings mappings)
    {
    }
    
    private void InitializeUiSounds()
    {
        var uiSoundValues = InterfaceSounds;

        if (uiSoundValues is not { Length: > 0 }) return;
        
        var uiSounds = UiApp.Services.Get<IUiSounds>();
        foreach (var v in uiSoundValues)
        {
            uiSounds.AddSound(v.Item1, v.Item2);
        }
    }
}