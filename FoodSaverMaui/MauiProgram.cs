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
                    fonts.AddFont("Solway-Bold.tff", "Solway");
                });

            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);
            builder.Services.AddSingleton<IJwtHelper, JwtHelper>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<CreateAccount>();
            builder.Services.AddSingleton<CreateAccountViewModel>();
            builder.Services.AddSingleton<UserServices>();


            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomePageViewModel>();

            builder.Services.AddSingleton<LandingPage>();
            builder.Services.AddSingleton<LandingPageViewModel>();

            builder.Services.AddSingleton<UserProfile>();
            builder.Services.AddSingleton<UserProfileViewModel>();
            builder.Services.AddSingleton<UserProfileService>();

            builder.Services.AddSingleton<UpdatePassword>();
            builder.Services.AddSingleton<UpdatePasswordViewModel>();

            builder.Services.AddSingleton<DeleteUser>();
            builder.Services.AddSingleton<DeleteUserViewModel>();

            builder.Services.AddSingleton<UpdateEmail>();
            builder.Services.AddSingleton<UpdateEmailViewModel>();

            builder.Services.AddTransient<FindFood>();
            builder.Services.AddTransient<FindFoodViewModel>();
            builder.Services.AddSingleton<FoodService>();

            builder.Services.AddTransient<FoodDetail>();
            builder.Services.AddTransient<FoodDetailViewModel>();

            builder.Services.AddTransient<UploadFood>();
            builder.Services.AddTransient<UploadFoodViewModel>();
#if DEBUG
            builder.Logging.AddDebug();

#endif

            return builder.Build();
        }
    }
}
