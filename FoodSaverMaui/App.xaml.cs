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
                ApiBaseUrl = "https://dd31-2405-acc0-1504-cce4-d12a-f565-6a03-9e7f.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
