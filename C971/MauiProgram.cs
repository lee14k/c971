using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace C971
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<MainPage>();
#if DEBUG
    		builder.Logging.AddDebug();
            builder.Services.AddTransient<MainPage>();

#endif

            return builder.Build();
        }
    }
}
