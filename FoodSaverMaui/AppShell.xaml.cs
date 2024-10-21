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
        }
    }
}
