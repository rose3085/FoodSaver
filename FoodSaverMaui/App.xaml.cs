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
                ApiBaseUrl = "https://0886-2405-acc0-1504-9a1f-309f-9145-80de-4fc0.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
