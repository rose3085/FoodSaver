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
                ApiBaseUrl = "https://0b9f-2405-acc0-1504-b3c0-d44c-7b26-1c02-fbc5.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
