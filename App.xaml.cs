using System.Windows;

namespace SoftMarine
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var navigationService = new NavigationService();
            var mainViewModel = new AllInspectionsViewModel(navigationService);

            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }
    }
}
