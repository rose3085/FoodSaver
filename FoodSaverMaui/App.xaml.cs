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
                ApiBaseUrl = "https://520e-2405-acc0-1504-cce4-b195-6ce9-ee75-482b.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
