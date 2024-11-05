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
                ApiBaseUrl = "https://1c7e-2405-acc0-1504-9a1f-fcee-6a30-8a4-92b7.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
