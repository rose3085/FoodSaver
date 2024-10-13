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
                ApiBaseUrl = "https://b221-2400-1a00-bb20-2e4e-11e9-177b-b9ac-85dc.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
