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
                ApiBaseUrl = "https://0432-2405-acc0-1504-9a1f-7d83-813b-6a64-8e82.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
