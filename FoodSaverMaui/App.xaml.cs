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
                ApiBaseUrl = "https://586c-2405-acc0-1504-b3c0-2098-36e6-9bd9-5c1c.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
