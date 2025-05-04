using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AppCRM
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'email et le mot de passe de l'utilisateur
            string email = txtEmailLogin.Text;
            string password = txtMdp.Password;

            // Appel de la méthode qui vérifie si l'email et le mot de passe sont corrects dans l'API
            bool isAuthenticated = await VerifyCredentialsAsync(email, password);

            if (isAuthenticated)
            {
                // Si l'utilisateur est authentifié, afficher la page principale
                PagePrincipale pageP = new PagePrincipale();
                pageP.Visibility = Visibility.Visible;
                this.Close();  // Fermer la fenêtre de connexion
            }
            else
            {
                // Si l'utilisateur n'est pas authentifié, afficher un message d'erreur
                MessageBox.Show("Les identifiants sont incorrects ou l'utilisateur n'existe pas.");
            }
        }

        // Méthode pour vérifier les identifiants via l'API
        private async Task<bool> VerifyCredentialsAsync(string email, string password)
        {
            var apiClient = new EmailPasswordSender();

            var (isAuthenticated, errorMessage) = await apiClient.VerifyAdminAsync(email, password);

            if (!isAuthenticated && !string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage);
            }

            return isAuthenticated;
        }

    }
}
