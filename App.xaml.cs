using OGSelector.Services;
using OGSelector.ViewModels;

namespace OGSelector;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        base.OnStart();
        _ = AutoLoadDataAsync();
    }

    private async Task AutoLoadDataAsync()
    {
        try
        {
            // Get command-line arguments (if any)
            var args = Environment.GetCommandLineArgs();
            string? jsonUrl = null;

            // Check if a URL was passed as a command-line argument
            if (args.Length > 1)
            {
                jsonUrl = args[1];
            }

            // Get the MainViewModel from the service provider
            var viewModel = IPlatformApplication.Current?.Services?.GetService<MainViewModel>();
            if (viewModel != null)
            {
                if (!string.IsNullOrWhiteSpace(jsonUrl))
                {
                    viewModel.JsonUrl = jsonUrl;
                }

                // Try to auto-load from file or provided URL
                var jsonDownloadService = IPlatformApplication.Current?.Services?.GetService<JsonDownloadService>();
                if (jsonDownloadService != null)
                {
                    var data = await jsonDownloadService.LoadFromFileOrUrlAsync(jsonUrl);
                    if (data != null)
                    {
                        await viewModel.LoadDataCommand.ExecuteAsync(null);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error during auto-load: {ex.Message}");
        }
    }
}
