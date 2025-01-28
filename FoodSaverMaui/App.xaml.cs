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
                ApiBaseUrl = "https://0aa2-2405-acc0-1504-cce4-2ca5-a92b-1937-8d3e.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
