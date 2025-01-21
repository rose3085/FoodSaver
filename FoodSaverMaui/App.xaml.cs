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
                ApiBaseUrl = "https://28ee-202-79-53-218.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
