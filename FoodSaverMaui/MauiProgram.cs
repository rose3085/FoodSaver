using CommunityToolkit.Maui;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Services.Food;
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
            builder.Services.AddSingleton<IJwtHelper, JwtHelper>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<CreateAccount>();
            builder.Services.AddSingleton<CreateAccountViewModel>();
            builder.Services.AddSingleton<UserServices>();


            builder.Services.AddSingleton<Login>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomePageViewModel>();

            builder.Services.AddSingleton<LandingPage>();

            builder.Services.AddSingleton<UserProfile>();
            builder.Services.AddSingleton<UserProfileViewModel>();

            builder.Services.AddSingleton<FindFood>();
            builder.Services.AddSingleton<FindFoodViewModel>();
            builder.Services.AddSingleton<FoodService>();
#if DEBUG
            builder.Logging.AddDebug();

#endif

            return builder.Build();
        }
    }
}
