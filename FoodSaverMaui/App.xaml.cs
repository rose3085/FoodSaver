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
                ApiBaseUrl = "https://9658-2405-acc0-1504-b3c0-ecf9-b721-7cfc-e389.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
