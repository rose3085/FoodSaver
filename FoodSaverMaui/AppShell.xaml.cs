using FoodSaverMaui.Views;

namespace FoodSaverMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Login), typeof(Login));
        }
    }
}
