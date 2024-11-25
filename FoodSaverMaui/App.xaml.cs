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
                ApiBaseUrl = "https://26e7-2405-acc0-1504-b3c0-4184-2c4a-fdaa-b0db.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
