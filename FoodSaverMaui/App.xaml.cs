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
                ApiBaseUrl = "https://0e1a-38-255-141-53.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
