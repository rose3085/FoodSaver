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
                ApiBaseUrl = "https://616a-2405-acc0-1504-9a1f-f568-cac7-3127-f895.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
