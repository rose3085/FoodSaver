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
                ApiBaseUrl = "https://5377-27-34-59-253.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
