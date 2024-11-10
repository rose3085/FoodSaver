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
                ApiBaseUrl = "https://d6c8-38-255-140-147.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
