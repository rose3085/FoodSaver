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
                ApiBaseUrl = "https://a74f-2405-acc0-1504-9a1f-5cd0-73cd-a875-7303.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
