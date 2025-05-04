using System.Configuration;
using System.Data;
using System.Windows;

namespace AppCRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Créer une instance de MainWindow sans l'afficher
            _mainWindow = new MainWindow();

            // Afficher la fenêtre de connexion
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        // Méthode pour afficher MainWindow lorsque l'utilisateur se connecte
        public void ShowMainWindow()
        {
            _mainWindow.Show();
        }
    }

}
