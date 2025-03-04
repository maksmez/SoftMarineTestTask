using System.Windows;

namespace SoftMarine
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var navigationService = new NavigationService();
            var mainViewModel = new AllInspectionsViewModel(navigationService);

            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
        }
    }
}
