using Avalonia.Controls;
using Microsoft.Extensions.Configuration;
using OGSelector.Services;
using OGSelector.ViewModels;
using System;
using System.Threading.Tasks;

namespace OGSelector.Views;

public partial class MainWindow : Window
{
    static IConfiguration config = ConfigurationService.BuildConfiguration();
    static Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

    private static bool _fullscreen = settings.Fullscreen;
    private static bool _canExit = settings.AllowExit;
    public static string RegKeyPath = settings.RegKeyPath.Replace("HKEY_LOCAL_MACHINE\\", "");

    public MainWindow()
    {
        // Set the DataContext to a new MainViewModel instance
        var viewModel = new MainViewModel();
        DataContext = viewModel;
        
        if (_canExit == false)
        {
            Closing += (sender, args) => args.Cancel = true;
        }

        if (_fullscreen == true)
        {
            this.WindowState = WindowState.FullScreen;
        }
        InitializeComponent();
        
        // Load data through the ViewModel
        _ = InitializeAsync(viewModel);
    }

    private async Task InitializeAsync(MainViewModel viewModel)
    {
        string? jsonUrl = null;

        // Check if jsonURL was provided as a command-line argument
        var args = Environment.GetCommandLineArgs();
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
        {
            jsonUrl = args[1];
            System.Diagnostics.Debug.WriteLine($"Using jsonURL from command-line argument: {jsonUrl}");
        }
        else if (!string.IsNullOrWhiteSpace(settings.jsonURL))
        {
            // Fall back to jsonURL from settings
            jsonUrl = settings.jsonURL;
            System.Diagnostics.Debug.WriteLine($"Using jsonURL from settings: {jsonUrl}");
        }

        await viewModel.LoadDataAsync(jsonUrl);
    }
}

public sealed class Settings
{
    public required string jsonURL { get; set; }
    public required bool Fullscreen { get; set; }
    public required bool AllowExit { get; set; }
    public required string RegKeyPath { get; set; }
}