using CommunityToolkit.Maui;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Helper.CacheHelper;
using FoodSaverMaui.KhaltiServices;
using FoodSaverMaui.Services.Food;
using FoodSaverMaui.Services.OrderDelivery;
using FoodSaverMaui.Services.PurchaseHistory;
using FoodSaverMaui.Services.SalesRecord;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.SignalRServices;
using FoodSaverMaui.ViewModel;
using FoodSaverMaui.Views;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
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
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Bold", "PoppinsBold");
                    fonts.AddFont("Poppins-SemiBold", "PoppinsSemiBold");
                    fonts.AddFont("Poppins-Regular", "PoppinsRegular");
                    fonts.AddFont("Solway-Bold.tff", "Solway");
                });

            builder.Services.AddScoped<ICacheService, CacheService >();
            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);
            builder.Services.AddSingleton<IJwtHelper, JwtHelper>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddTransient<CreateAccount>();
            builder.Services.AddTransient<CreateAccountViewModel>();
            builder.Services.AddSingleton<UserServices>();
            

            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomePageViewModel>();

            builder.Services.AddSingleton<LandingPage>();
            builder.Services.AddSingleton<LandingPageViewModel>();

            builder.Services.AddSingleton<UserProfile>();
            builder.Services.AddSingleton<UserProfileViewModel>();
            builder.Services.AddSingleton<UserProfileService>();
            builder.Services.AddSingleton<SalesRecordServices>();
            builder.Services.AddSingleton<PurchaseHistoryService>();

            builder.Services.AddSingleton<OrderDetail>();
            builder.Services.AddSingleton<OrderDetailViewModel>();
            builder.Services.AddSingleton<DeliveryService>();

            builder.Services.AddTransient<UpdatePassword>();
            builder.Services.AddTransient<UpdatePasswordViewModel>();

            builder.Services.AddTransient<DeleteUser>();
            builder.Services.AddTransient<DeleteUserViewModel>();

            builder.Services.AddTransient<UpdateEmail>();
            builder.Services.AddTransient<UpdateEmailViewModel>();

            builder.Services.AddTransient<FindFood>();
            builder.Services.AddTransient<FindFoodViewModel>();
            builder.Services.AddSingleton<FoodService>();

            builder.Services.AddTransient<FoodDetail>();
            builder.Services.AddTransient<FoodDetailViewModel>();

            builder.Services.AddTransient<UploadFood>();
            builder.Services.AddTransient<UploadFoodViewModel>();

            builder.Services.AddTransient<PostSuccessfullPage>();
            builder.Services.AddTransient<PostSuccessfullViewModel>();

            builder.Services.AddTransient<UploadPostSucessfull>();
            builder.Services.AddTransient<UploadPostSuccessfullViewModel>();

            builder.Services.AddSingleton<ConfirmPaymentUrl>();
            builder.Services.AddSingleton<ConfirmPaymentUrlViewModel>();
            builder.Services.AddSingleton<ConfirmPaymentSucessfull>();
            builder.Services.AddSingleton<ConfirmPaymentSucessfullViewModel>();

            builder.Services.AddTransient<KhaltiPaymentView>();
            builder.Services.AddTransient<KhaltiPaymentViewModel>();
            builder.Services.AddSingleton<IKhaltiService,KhaltiService>();


            builder.Services.AddTransient<PaymentUrl>();
            builder.Services.AddTransient<PaymentUrlViewModel>();


            builder.Services.AddTransient<EditProfile>();
            builder.Services.AddTransient<EditProfileViewModel>();
            builder.Services.AddSingleton<ISignalRService, SignalRService>();


            builder.Services.AddTransient<TempPage>();
            builder.Services.AddTransient<TempPageViewModel>();


#if DEBUG
            builder.Logging.AddDebug();

#endif

            return builder.Build();
        }
    }
}
