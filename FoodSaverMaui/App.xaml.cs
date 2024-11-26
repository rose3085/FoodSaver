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
                ApiBaseUrl = "https://1b30-2405-acc0-1504-b3c0-bce9-5e05-9a37-a8ad.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
