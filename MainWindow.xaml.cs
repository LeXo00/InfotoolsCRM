using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using AppCRM;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Diagnostics;
using System.Net.Sockets;
using System.Linq.Expressions;

namespace AppCRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApiClient apiClient;
        private ObservableCollection<Prospect> observableProspects;
        private ObservableCollection<Client> observableClients;

        private string token;
        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            apiClient = new ApiClient();

            observableProspects = new ObservableCollection<Prospect>();
            observableClients = new ObservableCollection<Client>();
            dtgProspect.ItemsSource = observableProspects;
            dtgClient.ItemsSource = observableClients;
            token = AuthManager.AuthToken;

            if (string.IsNullOrEmpty(token)) { }

            else
            {
                LoadProspectsData();
                LoadClientsData();
                RemplirComboBoxProspects();
            }
                    }



        private async void LoadClientsData()
        {
            Debug.WriteLine("Début de la récupération des clients");
            string token = AuthManager.AuthToken;

            try
            {
                // Récupérer les prospects depuis l'API
                List<Client> clients = await apiClient.GetClientsAndProspectsAsync(token);
                Debug.WriteLine($"Clients récupérés : {clients.Count}");

                if (clients != null && clients.Count > 0)
                {
                    // Ajouter chaque prospect à l'ObservableCollection pour mise à jour dynamique
                    foreach (var client in clients)
                    {
                        observableClients.Add(client);
                    }

                    dtgClient.Items.Refresh();  // Rafraîchir la DataGrid
                }
                else
                {
                    MessageBox.Show("Aucun client à afficher.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des clients : " + ex.Message);
            }


        }

        // Permet l'affichage des différents acteurs du projets
        private async void LoadProspectsData()
        {
            Debug.WriteLine("Début de la récupération des prospects");

            try
            {
                // Récupérer les prospects depuis l'API
                List<Prospect> prospects = await apiClient.GetProspectsAsync(token);
                Debug.WriteLine($"Prospects récupérés : {prospects.Count}");

                if (prospects != null && prospects.Count > 0)
                {
                    // Ajouter chaque prospect à l'ObservableCollection pour mise à jour dynamique
                    foreach (var prospect in prospects)
                    {
                        observableProspects.Add(prospect);
                    }

                    dtgProspect.Items.Refresh();  // Rafraîchir la DataGrid
                }
                else
                {
                    MessageBox.Show("Aucun prospect à afficher.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des prospects : " + ex.Message);
            }
        }

        private void dtgProspect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgProspect.SelectedItem is Prospect selectedProspect)
            {
                txtIDProspect.Text = selectedProspect.IdProspect.ToString();
                txtPrenomProspect.Text = selectedProspect.PrenomProspect;
                txtNomProspect.Text = selectedProspect.NomProspect;
                txtMailProspect.Text = selectedProspect.MailProspect;
                txtTelProspect.Text = selectedProspect.TelProspect;
            }
        }
            private async void btnAjouterProspect_Click(object sender, RoutedEventArgs e)
            {

            string nom = txtNomProspect.Text;
            string prenom = txtPrenomProspect.Text;
            string mail = txtMailProspect.Text;
            string tel = txtTelProspect.Text;
            string mdp = txtMdpProspect.Password;

            // Créer un dictionnaire pour envoyer les données du prospect
            var newProspect = new Dictionary<string, string>
            {
                { "NomProspects", nom },
                { "PrenomProspects", prenom },
                { "telProspects", tel },
                { "EmailProspects", mail },
                { "mdpProspect", mdp }
            };


            try
            {
                // Appeler la méthode AddProspectAsync pour ajouter le prospect
                var addedProspect = await apiClient.AddProspectAsync(newProspect, token);

                if (addedProspect != null)
                {
                    // Si l'ajout est réussi, afficher un message de succès
                    MessageBox.Show("Prospect ajouté avec succès.");

                    // Ajouter le prospect à la liste (ou ObservableCollection) utilisée pour la DataGrid
                    observableProspects.Add(addedProspect);

                    // Mettre à jour la DataGrid avec le nouveau prospect
                    dtgProspect.ItemsSource = observableProspects;
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du prospect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
            }

            private async void btnSuppriméProspect_Click(object sender, RoutedEventArgs e)
            {
            // Vérifier si un prospect est sélectionné dans le DataGrid
            if (dtgProspect.SelectedItem is Prospect selectedProspect)
            {

                // Convertir l'ID du prospect en chaîne de caractères
                string prospectId = selectedProspect.getId().ToString();

                try
                {
                    // Appeler la méthode de l'API pour supprimer le prospect
                    bool isDeleted = await apiClient.DeleteProspectAsync(prospectId, token);

                    // Vérifier si la suppression a réussi
                    if (isDeleted)
                    {
                        // Supprimer le prospect de l'ObservableCollection
                        observableProspects.Remove(selectedProspect);

                        // Tenter de supprimer le client associé, même s'il n'existe pas
                        var clientAssociated = observableClients.FirstOrDefault(c => c.IdProspect == selectedProspect.IdProspect);
                        if (clientAssociated != null)
                        {
                            observableClients.Remove(clientAssociated);
                        }

                        // Afficher un message de succès
                        MessageBox.Show("Prospect supprimé avec succès.");
                    }
                    else
                    {
                        // Si la suppression échoue, afficher un message d'erreur
                        MessageBox.Show("Erreur lors de la suppression du prospect.");
                        Console.WriteLine("La suppression a échoué. Vérifiez la réponse de l'API.");
                    }
                }
                catch (Exception ex)
                {
                    // Si une exception se produit, l'afficher
                    MessageBox.Show($"Erreur lors de la suppression du prospect : {ex.Message}");
                    Console.WriteLine($"Exception lors de la suppression : {ex.Message}");
                }

                // Désélectionner l'élément après la suppression
                dtgProspect.SelectedItem = null;
            }
            else
            {
                // Si aucun prospect n'est sélectionné, afficher un message
                MessageBox.Show("Veuillez sélectionner un prospect à supprimer.");
            }
        }

        private async void btnModifierProspect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgProspect.SelectedItem is Prospect selectedProspect)
            {
                // Valider que tous les champs sont remplis
                if (string.IsNullOrWhiteSpace(txtNomProspect.Text) ||
                    string.IsNullOrWhiteSpace(txtPrenomProspect.Text) ||
                    string.IsNullOrWhiteSpace(txtTelProspect.Text) ||
                    string.IsNullOrWhiteSpace(txtMailProspect.Text))
                {
                    MessageBox.Show("Tous les champs doivent être remplis.");
                    return;
                }

                // Mettre à jour l'objet Prospect avec les nouvelles valeurs des TextBox
                selectedProspect.NomProspect = txtNomProspect.Text;
                selectedProspect.PrenomProspect = txtPrenomProspect.Text;
                selectedProspect.TelProspect = txtTelProspect.Text;
                selectedProspect.MailProspect = txtMailProspect.Text;


                // Appeler la méthode UpdateProspectAsync
                bool isUpdated = await apiClient.UpdateProspectAsync(selectedProspect, token);

                if (isUpdated)
                {
                    // Rafraîchir la liste des prospects dans le DataGrid
                    MessageBox.Show("Prospect mis à jour avec succès !");

                    // Mettre à jour l'élément modifié dans la ObservableCollection
                    var index = observableProspects.IndexOf(selectedProspect);
                    if (index != -1)
                    {
                        // Remplacer l'élément dans la ObservableCollection
                        observableProspects[index] = selectedProspect;
                    }

                    // Optionnel : rafraîchir explicitement le DataGrid (normalement, ObservableCollection le fait automatiquement)
                    dtgProspect.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du prospect.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un prospect pour le mettre à jour.");
            }
        }

        private async void RefreshProspectList()
        {
            // Vous pouvez soit recharger la liste complète de prospects depuis l'API,
            // soit mettre à jour l'élément modifié dans la collection existante si vous l'avez stockée localement.
            var updatedProspects = await apiClient.GetProspectsAsync(token);  // Méthode qui récupère la liste mise à jour des prospects
            dtgProspect.ItemsSource = updatedProspects;  // Mettre à jour la source de données du DataGrid
        }

        private void dtgClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgClient.SelectedItem is Client selectedClient)
            {
                txtIDClient.Text = selectedClient.IdCli.ToString();
                txtPrenomClient.Text = selectedClient.PrenomProspect.ToString();
                txtNomClient.Text = selectedClient.NomProspect.ToString();
                txtCPClient.Text = selectedClient.CpCli;
                txtMailClient.Text = selectedClient.MailProspect;
                txtVilleClient.Text = selectedClient.VilleCli;
                txtRueClient.Text = selectedClient.RueCli;
                txtTelClient.Text = selectedClient.TelProspect;

                txtPrenomClient.IsEnabled = false;
                txtNomClient.IsEnabled = false;
                txtMailClient.IsEnabled = false;
                txtTelClient.IsEnabled = false;
            }
        }

        private async void RemplirComboBoxProspects()
        {
            // Récupérer les prospects depuis l'API ou votre base de données

            try
            {
                // Récupérer les prospects depuis l'API
                List<Prospect> prospects = await apiClient.GetProspectsAsync(token);

                if (prospects != null && prospects.Count > 0)
                {
                    // Créer une liste d'objets personnalisés qui contiennent l'ID et le nom du prospect
                    var prospectsDisplay = prospects.Select(p => new
                    {
                        DisplayText = $"{p.IdProspect} - {p.NomProspect}  {p.PrenomProspect}",  // Format : "ID - Nom"
                        IdProspect = p.IdProspect
                    }).ToList();

                    // Lier la ComboBox avec la nouvelle liste d'objets
                    cbIDProspectClient.ItemsSource = prospectsDisplay;

                    // Définir la manière dont chaque élément sera affiché dans la ComboBox
                    cbIDProspectClient.DisplayMemberPath = "DisplayText";  // Affiche "ID - Nom"
                    cbIDProspectClient.SelectedValuePath = "IdProspect";   // L'ID sera la valeur sélectionnée
                }
                else
                {
                    MessageBox.Show("Aucun prospect à afficher.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des prospects : " + ex.Message);
            }
        }

        private async void btnAjouterClient_Click(object sender, RoutedEventArgs e)
        {
            var apiUrl = "http://infotools.test/api/clients";

            string nom = txtNomClient.Text;
            string prenom = txtPrenomClient.Text;
            string mail = txtMailClient.Text;
            string tel = txtTelClient.Text;
            string mdp = txtMdpClient.Password;
            string cp = txtCPClient.Text;
            string ville = txtVilleClient.Text;
            string adresse = txtRueClient.Text;

            int? idProspect = null;

            if (cbIDProspectClient.SelectedItem != null)
            {
                idProspect = (int?)cbIDProspectClient.SelectedValue;
            }

            // Si aucun prospect sélectionné, ajouter un nouveau prospect
            if (idProspect == null)
            {
                var newProspect = new Dictionary<string, string>
        {
            { "NomProspects", nom },
            { "PrenomProspects", prenom },
            { "telProspects", tel },
            { "EmailProspects", mail },
            { "mdpProspect", mdp }
        };


                try
                {
                    var addedProspect = await apiClient.AddProspectAsync(newProspect, token);

                    if (addedProspect != null)
                    {
                        idProspect = addedProspect.IdProspect;

                        // Ajouter le nouveau prospect à la collection Observable
                        observableProspects.Add(addedProspect);
                        // Rafraîchir la ComboBox des prospects
                        cbIDProspectClient.ItemsSource = observableProspects;
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout du prospect");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}");
                    return;
                }
            }

            // Créer le client une fois que le prospect est ajouté ou sélectionné
            var newClient = new Dictionary<string, string>
            {
                { "CPClient", cp },
                { "VilleClient", ville },
                { "AdresseClient", adresse },
                { "idProspects", idProspect.ToString() }
            };

            try
            {
                // Appeler la méthode modifiée pour ajouter le client
                var addedClient = await apiClient.AddClientAsync(newClient, token);

                if (addedClient != null)
                {
                    MessageBox.Show("Client ajouté avec succès");

                    // Ajouter le nouveau prospect à la collection Observable
                    observableClients.Clear();
                    LoadClientsData();
                    observableClients.Add(addedClient);
                    
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du client.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }


        }

        // Méthode pour rafraîchir la liste des clients dans la DataGrid (si nécessaire)
        private async Task RafraichirClients()
        {
            var apiClient = new ApiClient();
            var clients = await apiClient.GetClientsAndProspectsAsync(token);  // Supposons que vous avez une méthode pour récupérer les clients
            dtgClient.ItemsSource = clients;  // Rafraîchit la DataGrid avec la nouvelle liste de clients
        }

        private async void btnSuppriméClient_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si un prospect est sélectionné dans le DataGrid
            if (dtgClient.SelectedItem is Client selectedClient)
            {

                // Convertir l'ID du prospect en chaîne de caractères
                string clientId = selectedClient.GetId().ToString();
                Console.WriteLine($"ID du client à supprimer : {clientId}");  // Affichage de l'ID pour vérification

                try
                {
                    // Appeler la méthode de l'API pour supprimer le prospect
                    bool isDeleted = await apiClient.DeleteClientAsync(clientId, token);

                    // Vérifier si la suppression a réussi
                    if (isDeleted)
                    {
                        // Supprimer le prospect de l'ObservableCollection
                        observableClients.Remove(selectedClient);

                        // Afficher un message de succès
                        MessageBox.Show("Client supprimé avec succès.");
                    }
                    else
                    {
                        // Si la suppression échoue, afficher un message d'erreur
                        MessageBox.Show("Erreur lors de la suppression du client.");
                        Console.WriteLine("La suppression a échoué. Vérifiez la réponse de l'API.");
                    }
                }
                catch (Exception ex)
                {
                    // Si une exception se produit, l'afficher
                    MessageBox.Show($"Erreur lors de la suppression du client : {ex.Message}");
                    Console.WriteLine($"Exception lors de la suppression : {ex.Message}");
                }

                // Désélectionner l'élément après la suppression
                dtgClient.SelectedItem = null;
            }
            else
            {
                // Si aucun prospect n'est sélectionné, afficher un message
                MessageBox.Show("Veuillez sélectionner un client à supprimer.");
            }
        }

        private async void btnModifierClient_Click(object sender, RoutedEventArgs e)
        {
            if (dtgClient.SelectedItem is Client selectedClient)  // Utiliser le bon DataGrid ici
            {
                // Valider que tous les champs sont remplis pour un client
                if (string.IsNullOrWhiteSpace(txtCPClient.Text) ||
                    string.IsNullOrWhiteSpace(txtVilleClient.Text) ||
                    string.IsNullOrWhiteSpace(txtRueClient.Text))
                {
                    MessageBox.Show("Tous les champs doivent être remplis.");
                    return;
                }

                // Mettre à jour l'objet Client avec les nouvelles valeurs des TextBox
                selectedClient.CpCli = txtCPClient.Text;
                selectedClient.VilleCli = txtVilleClient.Text;
                selectedClient.RueCli = txtRueClient.Text;

                // Appeler la méthode UpdateClientAsync
                bool isUpdated = await apiClient.UpdateClientAsync(selectedClient, token);

                if (isUpdated)
                {
                    // Rafraîchir la liste des clients dans le DataGrid
                    MessageBox.Show("Client mis à jour avec succès !");

                    // Mettre à jour l'élément modifié dans la ObservableCollection des clients
                    var index = observableClients.IndexOf(selectedClient);
                    if (index != -1)
                    {
                        // Remplacer l'élément dans la ObservableCollection des clients
                        observableClients[index] = selectedClient;
                    }

                    // Optionnel : rafraîchir explicitement le DataGrid (normalement, ObservableCollection le fait automatiquement)
                    dtgClient.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du client.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client pour le mettre à jour.");
            }
        }

        private void cbIDProspectClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbIDProspectClient.SelectedItem != null)
            {
                txtNomClient.IsEnabled = false;
                txtPrenomClient.IsEnabled = false;
                txtTelClient.IsEnabled = false;
                txtMailClient.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PagePrincipale pageP = new PagePrincipale();
            pageP.Visibility = Visibility.Visible;
        }
    }

}
 