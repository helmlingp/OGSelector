using OGSelector.Services;
using OGSelector.ViewModels;

namespace OGSelector;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .Services
            .AddSingleton<JsonDownloadService>()
            .AddSingleton<RegistryService>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<MainPage>();

        return builder.Build();
    }
}
