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
                ApiBaseUrl = "https://b000-2405-acc0-1504-cce4-e5e8-d0-a178-88a.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
