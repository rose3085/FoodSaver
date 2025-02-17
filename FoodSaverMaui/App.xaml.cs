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
                ApiBaseUrl = "https://0d6c-2405-acc0-1504-cce4-00-1.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
