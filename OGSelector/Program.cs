using System;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia;
//using Avalonia.ReactiveUI;

namespace OGSelector;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var currentDirectory = Directory.GetCurrentDirectory();

        var appSettingsExists =
            File.Exists(Path.Combine(currentDirectory, "appsettings.json")) ||
            File.Exists(Path.Combine(exeDirectory, "appsettings.json"));

        var inputsExists = File.Exists(Path.Combine(exeDirectory, "inputs.json"));

        if (!appSettingsExists || !inputsExists)
        {
            var message = "Startup failed due to missing required file(s):";
            if (!appSettingsExists)
            {
                message += $"{Environment.NewLine}- appsettings.json (checked in '{exeDirectory}')";
            }

            if (!inputsExists)
            {
                message += $"{Environment.NewLine}- inputs.json (checked in '{exeDirectory}')";
            }

            ShowStartupError(message);
            return;
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    private static void ShowStartupError(string message)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            MessageBoxW(IntPtr.Zero, message, "OGSelector - Startup Error", 0x00000010);
            return;
        }

        Console.Error.WriteLine(message);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
            //.UseReactiveUI();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(IntPtr hWnd, string text, string caption, uint type);
}
public sealed class Settings
{
    public required bool Fullscreen { get; set; }
    public required bool AllowExit { get; set; }

}