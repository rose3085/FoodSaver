using CommunityToolkit.Maui;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.ViewModel;
using FoodSaverMaui.Views;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Biometric;

namespace FoodSaverMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Bold", "PoppinsBold");
                    fonts.AddFont("Poppins-SemiBold", "PoppinsSemiBold");
                    fonts.AddFont("Poppins-Regular", "PoppinsRegular");
                });

            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<CreateAccount>();
            builder.Services.AddSingleton<CreateAccountViewModel>();
            builder.Services.AddSingleton<UserServices>();


            builder.Services.AddSingleton<Login>();
            builder.Services.AddSingleton<LoginViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();

#endif

            return builder.Build();
        }
    }
}
