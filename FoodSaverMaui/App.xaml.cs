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
                ApiBaseUrl = "https://91e8-2405-acc0-1504-cce4-117a-683f-20a2-62cc.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
