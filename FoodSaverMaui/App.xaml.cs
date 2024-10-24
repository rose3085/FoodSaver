namespace FoodSaverMaui
{
    public partial class App : Application
    {
        public static AppSettings Settings { get; private set; }
        public App()
        {
            InitializeComponent();
            Settings = new AppSettings
            {
                ApiBaseUrl = "https://1b22-2400-1a00-bb20-db04-90c7-6070-88d6-ff9c.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
