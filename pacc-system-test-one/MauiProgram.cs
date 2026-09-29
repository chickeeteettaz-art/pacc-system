using Microsoft.Extensions.Logging;
using pacc_system_test_one.Services;

namespace pacc_system_test_one
{
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            // Blazor authentication/authorization support
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<FirebaseAuthService>();
            builder.Services.AddScoped<IBlobStorageService, ImageStorageService>();
            return builder.Build();
        }
    }
}
