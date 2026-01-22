using System;
using Avalonia;
//using Avalonia.ReactiveUI;

namespace OGSelector;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    // public static void Main(string[] args)
    // {
    //     AvaloniaLocator.CurrentMutable.Bind<Win32PlatformOptions>()
    //     .ToLazy(() => new Win32PlatformOptions()
    //     {
    //         CompositionBackdropCornerRadius = 7
    //     });

    //     BuildAvaloniaApp()
    //         .StartWithClassicDesktopLifetime(args);
    // }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
            //.UseReactiveUI();
}
public sealed class Settings
{
    public required bool Fullscreen { get; set; }
    public required bool AllowExit { get; set; }

}