using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Editor.ViewModels;
using Editor.Views;
using Gunslinger.Core;
using Ssit.CrossX.Games;
using Ssit.CrossX.IoC;

namespace Editor;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var services = PrepareServices();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.IoCConstruct<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
    
    private IIoCContainer PrepareServices()
    {
        var builder = IoC.NewBuilder();

        return builder
            .WithInstance(this)
            .WithSingleton<IGameTemplate, GunslingerTemplate>()
            .WithInstance(new IGameTemplate[]
            {
                new GunslingerTemplate()
            })
            .Build();
    }
}