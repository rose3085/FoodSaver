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
                ApiBaseUrl = " https://4c79-2405-acc0-1504-cce4-140b-94d5-ca12-1ce4.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
