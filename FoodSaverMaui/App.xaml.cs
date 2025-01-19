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
                ApiBaseUrl = "https://1ac2-2405-acc0-1504-cce4-e8d9-5730-e9ec-3d4d.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
