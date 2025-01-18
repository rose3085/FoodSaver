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
                ApiBaseUrl = "https://365c-2405-acc0-1504-cce4-b1d5-a8bb-47d6-f13.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
