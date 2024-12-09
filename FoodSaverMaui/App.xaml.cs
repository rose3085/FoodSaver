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
                ApiBaseUrl = "https://fe60-2405-acc0-1504-cce4-c5f-136b-e0bb-ad3a.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
