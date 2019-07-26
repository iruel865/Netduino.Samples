using Xamarin.Forms;

namespace PlantMonitorApp
{
    public partial class App : Application
    {
        public const string LOG_FILE_NAME = "plant_log.json";

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
