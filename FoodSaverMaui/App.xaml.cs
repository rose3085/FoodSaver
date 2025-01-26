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
                ApiBaseUrl = "https://34bf-2405-acc0-1504-cce4-f915-d307-b1b4-24af.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
