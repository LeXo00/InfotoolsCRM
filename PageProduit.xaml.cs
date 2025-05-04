using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppCRM
{
    /// <summary>
    /// Logique d'interaction pour PageProduit.xaml
    /// </summary>
    public partial class PageProduit : Window
    {
        private ApiClient apiClient;
        private ObservableCollection<Produit> observableProduits;
        private string token;
        public PageProduit()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            apiClient = new ApiClient();

            observableProduits = new ObservableCollection<Produit>();
            dtgProduit.ItemsSource = observableProduits;
            token = AuthManager.AuthToken;

            if (string.IsNullOrEmpty(token)) { }

            else
            {
                LoadProduitsData();
            }
        }

        // Permet l'affichage des différents acteurs du projets
        private async void LoadProduitsData()
        {
            Debug.WriteLine("Début de la récupération des produits");

            try
            {
                // Récupérer les prospects depuis l'API
                List<Produit> produits = await apiClient.GetProduitsAsync(token);
                Debug.WriteLine($"Produits récupérés : {produits.Count}");

                if (produits != null && produits.Count > 0)
                {
                    foreach (Produit produit in produits)
                    {
                        observableProduits.Add(produit);
                    }

                    dtgProduit.Items.Refresh();  // Rafraîchir la DataGrid
                }
                else
                {
                    MessageBox.Show("Aucun produit à afficher.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des produits : " + ex.Message);
            }
        }

        private void dtgProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgProduit.SelectedItem is Produit selectedProduit)
            {
                txtIdProd.Text = selectedProduit.IdProduit.ToString();
                txtTypeProd.Text = selectedProduit.TypeProduit;
                txtPrixProd.Text = selectedProduit.PrixProduit.ToString();
                txtNomProd.Text = selectedProduit.NomProduit;
                txtDescProd.Text = selectedProduit.DescProduit;
            }
        }

        private async void btnAjouterProduit_Click(object sender, RoutedEventArgs e)
        {

            string type = txtTypeProd.Text;
            double prix;

            // Essayez de convertir le texte en double
            bool isValidPrix = double.TryParse(txtPrixProd.Text, out prix);
            if (!isValidPrix)
            {
                MessageBox.Show("Le prix doit être un nombre valide.");
                return;  // Sortir si le prix n'est pas valide
            }

            string nom = txtNomProd.Text;
            string desc = txtDescProd.Text;

            // Créer un dictionnaire pour envoyer les données du produit
            var newProduit = new Dictionary<string, object>  // Utilisez object pour le prix en double
            {
                { "typeProd", type },
                { "prixProd", prix },  // Laisser le prix en tant que double
                { "nomProd", nom },
                { "descProd", desc }
            };

            try
            {
                // Appeler la méthode AddProduitAsync pour ajouter le produit
                var addedProduit = await apiClient.AddProduitAsync(newProduit, token);

                if (addedProduit != null)
                {
                    // Si l'ajout est réussi, afficher un message de succès
                    MessageBox.Show("Produit ajouté avec succès.");

                    // Ajouter le produit à la liste (ou ObservableCollection) utilisée pour la DataGrid
                    observableProduits.Add(addedProduit);

                    // Mettre à jour la DataGrid avec le nouveau produit
                    dtgProduit.ItemsSource = observableProduits;
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du produit.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }

        private async void btnSupprProduit_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si un prospect est sélectionné dans le DataGrid
            if (dtgProduit.SelectedItem is Produit selectedProduit)
            {

                // Convertir l'ID du prospect en chaîne de caractères
                string produitId = selectedProduit.GetId().ToString();
                Console.WriteLine($"ID du produit à supprimer : {produitId}");  // Affichage de l'ID pour vérification

                try
                {
                    // Appeler la méthode de l'API pour supprimer le prospect
                    bool isDeleted = await apiClient.DeleteProduitAsync(produitId, token);

                    // Vérifier si la suppression a réussi
                    if (isDeleted)
                    {
                        // Supprimer le prospect de l'ObservableCollection
                        observableProduits.Remove(selectedProduit);

                        // Afficher un message de succès
                        MessageBox.Show("Produit supprimé avec succès.");
                    }
                    else
                    {
                        // Si la suppression échoue, afficher un message d'erreur
                        MessageBox.Show("Erreur lors de la suppression du produit.");
                        Console.WriteLine("La suppression a échoué. Vérifiez la réponse de l'API.");
                    }
                }
                catch (Exception ex)
                {
                    // Si une exception se produit, l'afficher
                    MessageBox.Show($"Erreur lors de la suppression du produit : {ex.Message}");
                    Console.WriteLine($"Exception lors de la suppression : {ex.Message}");
                }

                // Désélectionner l'élément après la suppression
                dtgProduit.SelectedItem = null;
            }
            else
            {
                // Si aucun prospect n'est sélectionné, afficher un message
                MessageBox.Show("Veuillez sélectionner un produit à supprimer.");
            }
        }

        private async void btnModifierProduit_Click(object sender, RoutedEventArgs e)
        {
            if (dtgProduit.SelectedItem is Produit selectedProduit)
            {
                // Valider que tous les champs sont remplis
                if (string.IsNullOrWhiteSpace(txtTypeProd.Text) ||
                    string.IsNullOrWhiteSpace(txtPrixProd.Text) ||
                    string.IsNullOrWhiteSpace(txtNomProd.Text) ||
                    string.IsNullOrWhiteSpace(txtDescProd.Text))
                {
                    MessageBox.Show("Tous les champs doivent être remplis.");
                    return;
                }

                // Mettre à jour l'objet Prospect avec les nouvelles valeurs des TextBox
                selectedProduit.TypeProduit = txtTypeProd.Text;
                selectedProduit.PrixProduit = Convert.ToDouble(txtPrixProd.Text);
                selectedProduit.NomProduit = txtNomProd.Text;
                selectedProduit.DescProduit = txtDescProd.Text;


                // Appeler la méthode UpdateProspectAsync
                bool isUpdated = await apiClient.UpdateProduitAsync(selectedProduit, token);

                if (isUpdated)
                {
                    // Rafraîchir la liste des prospects dans le DataGrid
                    MessageBox.Show("Produit mis à jour avec succès !");

                    // Mettre à jour l'élément modifié dans la ObservableCollection
                    var index = observableProduits.IndexOf(selectedProduit);
                    if (index != -1)
                    {
                        // Remplacer l'élément dans la ObservableCollection
                        observableProduits[index] = selectedProduit;
                    }

                    // Optionnel : rafraîchir explicitement le DataGrid (normalement, ObservableCollection le fait automatiquement)
                    dtgProduit.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du produit.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un produit pour le mettre à jour.");
            }
        }
    }
}
