using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using SLO.MobileApp.Core.DependencyInjections;

namespace SLO.MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var mauiAppBuilder = MauiApp.CreateBuilder();

        mauiAppBuilder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        mauiAppBuilder.Services.UseSloMobileAppCore(
            appDataDirectory: FileSystem.AppDataDirectory);

#if DEBUG
        mauiAppBuilder.Logging.AddDebug();
#endif

        return mauiAppBuilder.Build();
    }
}
