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
                ApiBaseUrl = "https://7112-2405-acc0-1504-cce4-9534-963d-17f3-f208.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
