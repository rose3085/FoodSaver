using FoodSaverMaui.Views;

namespace FoodSaverMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Login), typeof(Login));
            Routing.RegisterRoute(nameof(FindFood),typeof(FindFood));
            Routing.RegisterRoute(nameof(LandingPage), typeof(LandingPage));
            Routing.RegisterRoute(nameof(UserProfile), typeof(UserProfile));
            Routing.RegisterRoute(nameof(UploadFood), typeof(UploadFood));
            Routing.RegisterRoute(nameof(UpdatePassword), typeof(UpdatePassword));
            Routing.RegisterRoute(nameof(DeleteUser), typeof(DeleteUser));
            Routing.RegisterRoute(nameof(UpdateEmail), typeof(UpdateEmail));
            Routing.RegisterRoute(nameof(FoodDetail), typeof(FoodDetail));
            Routing.RegisterRoute(nameof(PostSuccessfullPage), typeof(PostSuccessfullPage));
            Routing.RegisterRoute(nameof(PaymentUrl), typeof(PaymentUrl));
            Routing.RegisterRoute(nameof(KhaltiPaymentView), typeof(KhaltiPaymentView));

        }
    }
}
