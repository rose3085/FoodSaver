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
                ApiBaseUrl = "https://a0af-2400-1a00-bb20-f9f-a873-b229-b2e6-a42e.ngrok-free.app"
            };

            MainPage = new AppShell();
        }
    }
}
