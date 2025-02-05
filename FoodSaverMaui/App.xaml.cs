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
                ApiBaseUrl = "https://9d85-2405-acc0-1504-cce4-98c9-36b4-d35a-6799.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
