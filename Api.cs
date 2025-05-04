using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json;
using AppCRM;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace AppCRM
{

    public class ApiResponse<T>
    {
        public bool Succes { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }

    public class ApiResponseClients
    {
        public bool Succes { get; set; }
        public List<Client> Data { get; set; }
    }
    public class ApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        // Méthode pour récupérer tous les prospects
        public async Task<List<Prospect>> GetProspectsAsync(string token)
        {
            var apiUrl = "http://infotools.test/api/prospects";  // URL de l'API pour récupérer tous les prospects

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            // Faire la requête GET pour récupérer les prospects
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Prospect>>>(responseContent);

                if (apiResponse != null && apiResponse.Data != null && apiResponse.Data.Count > 0)
                {
                    return apiResponse.Data;
                }
                else
                {
                    return new List<Prospect>();  // Retourner une liste vide si aucun prospect
                }
            }
            else
            {
                throw new Exception($"Erreur lors de la récupération des prospects : {response.ReasonPhrase}");
            }
        }

        public async Task<List<Produit>> GetProduitsAsync(string token)
        {
            var apiUrl = "http://infotools.test/api/produits";  // URL de l'API pour récupérer tous les prospects

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            // Faire la requête GET pour récupérer les prospects
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Produit>>>(responseContent);

                if (apiResponse != null && apiResponse.Data != null && apiResponse.Data.Count > 0)
                {
                    return apiResponse.Data;
                }
                else
                {
                    return new List<Produit>();  // Retourner une liste vide si aucun prospect
                }
            }
            else
            {
                throw new Exception($"Erreur lors de la récupération des prospects : {response.ReasonPhrase}");
            }
        }

        // Méthode pour récupérer tous les clients et les associer aux prospects
        public async Task<List<Client>> GetClientsAndProspectsAsync(string token)
        {
            var apiUrlClients = "http://infotools.test/api/clients";  // URL pour récupérer les clients
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);  // Utilisation du token dynamique

            HttpResponseMessage responseClients = await client.GetAsync(apiUrlClients);
            var clientsContent = await responseClients.Content.ReadAsStringAsync();

            // Désérialisation en utilisant la structure attendue pour les clients
            var apiResponseClients = JsonConvert.DeserializeObject<ApiResponseClients>(clientsContent);
            var clients = apiResponseClients?.Data ?? new List<Client>();  // Si Data est null, retourner une liste vide

            // Récupérer les prospects
            var prospects = await GetProspectsAsync(token);  // Passer le token dynamique ici aussi

            // Associer les prospects aux clients en fonction de l'ID du prospect
            foreach (var client in clients)
            {
                var matchingProspect = prospects.Find(p => p.IdProspect == client.IdProspect); // Associe prospect en fonction de l'ID
                if (matchingProspect != null)
                {
                    client.NomProspect = matchingProspect.NomProspect;
                    client.PrenomProspect = matchingProspect.PrenomProspect;
                    client.MailProspect = matchingProspect.MailProspect;
                    client.TelProspect = matchingProspect.TelProspect;
                }
            }

            return clients;
        }

        // Méthode pour ajouter un prospect
        public async Task<Prospect> AddProspectAsync(Dictionary<string, string> newProspect, string token)
        {
            var apiUrl = "http://infotools.test/api/prospects";  // URL de l'API pour ajouter un prospect

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            var jsonContent = JsonConvert.SerializeObject(newProspect);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ApiResponse<Prospect>>(result);  // Désérialisation avec ApiResponse<Prospect>

                    if (responseData.Data != null)
                    {
                        return responseData.Data;  // Renvoie le prospect ajouté
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Erreur lors de l'ajout du prospect : {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la requête : {ex.Message}");
                return null;
            }
        }

        public async Task<Produit> AddProduitAsync(Dictionary<string, object> newProduit, string token)
        {
            var apiUrl = "http://infotools.test/api/produits";  // URL de l'API pour ajouter un prospect

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            var jsonContent = JsonConvert.SerializeObject(newProduit);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ApiResponse<Produit>>(result);  // Désérialisation avec ApiResponse<Prospect>

                    if (responseData.Data != null)
                    {
                        return responseData.Data;  // Renvoie le prospect ajouté
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Erreur lors de l'ajout du prospect : {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la requête : {ex.Message}");
                return null;
            }
        }

        // Méthode pour ajouter un client
        public async Task<Client> AddClientAsync(Dictionary<string, string> newClient, string token)
        {
            var apiUrl = "http://infotools.test/api/clients";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            var jsonContent = JsonConvert.SerializeObject(newClient);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ApiResponse<Client>>(result);  // Désérialisation avec ApiResponse<Client>

                    // Vérifier si la réponse contient des données et retourner le client ajouté
                    if (responseData?.Data != null)
                    {
                        return responseData.Data;  // Retourner l'objet Client ajouté
                    }
                    else
                    {
                        return null;  // Si Data est null, retourner null
                    }
                }
                else
                {
                    Console.WriteLine($"Erreur lors de l'ajout du client : {response.StatusCode}");
                    return null;  // Si l'ajout échoue, retourner null
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la requête : {ex.Message}");
                return null;  // Retourner null en cas d'exception
            }
        }

        // Méthode pour supprimer un prospect
        public async Task<bool> DeleteProspectAsync(string idProspect, string token)
        {
            var apiUrlProspect = $"http://infotools.test/api/prospects/{idProspect}";  // URL pour supprimer le prospect
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            try
            {
                // Faire la requête GET pour obtenir les détails du prospect (pour savoir si un client est associé)
                var response = await client.GetAsync(apiUrlProspect);

                if (response.IsSuccessStatusCode)
                {
                    // Récupérer les détails du prospect, en utilisant la désérialisation JSON
                    var prospectDetails = await response.Content.ReadAsAsync<Prospect>();  // Supposons que vous avez une classe Prospect

                    // Vérifier si un client est associé au prospect
                    if (prospectDetails != null && prospectDetails.IdProspect != null)
                    {
                        // Récupérer l'ID du client associé à ce prospect
                        var apiUrlClient = $"http://infotools.test/api/clients/{prospectDetails.IdProspect}";  // URL pour récupérer le client par l'ID du prospect
                        var clientResponse = await client.GetAsync(apiUrlClient);

                        if (clientResponse.IsSuccessStatusCode)
                        {
                            // Si un client est trouvé, récupérer ses détails
                            var clientDetails = await clientResponse.Content.ReadAsAsync<Client>();

                            if (clientDetails != null && clientDetails.GetId() != null)
                            {
                                // Supprimer le client associé avant de supprimer le prospect
                                var deleteClientUrl = $"http://infotools.test/api/clients/{clientDetails.GetId()}";  // URL pour supprimer le client
                                var deleteClientResponse = await client.DeleteAsync(deleteClientUrl);

                                if (deleteClientResponse.IsSuccessStatusCode)
                                {
                                    Console.WriteLine("Client associé supprimé avec succès.");
                                }
                                else
                                {
                                    Console.WriteLine($"Erreur lors de la suppression du client : {deleteClientResponse.ReasonPhrase}");
                                    return false;  // Si la suppression du client échoue, ne pas supprimer le prospect
                                }
                            }
                        }
                    }

                    // Après avoir supprimé le client (si nécessaire), supprimer le prospect
                    response = await client.DeleteAsync(apiUrlProspect);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Prospect supprimé avec succès.");
                        return true;
                    }
                    else
                    {
                        // Afficher les détails de l'erreur si la suppression du prospect échoue
                        Console.WriteLine($"Erreur lors de la suppression du prospect : {response.ReasonPhrase}");
                        return false;
                    }
                }
                else
                {
                    // Si la requête GET échoue, retourner false
                    Console.WriteLine($"Erreur lors de la récupération des détails du prospect : {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la suppression du prospect : {ex.Message}");
                return false;
            }
        }

        // Méthode pour supprimer un client
        public async Task<bool> DeleteClientAsync(string idClient, string token)
        {
            var apiUrl = $"http://infotools.test/api/clients/{idClient}";  // L'ID doit être une chaîne
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            try
            {
                // Faire la requête DELETE pour supprimer le prospect
                HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                // Vérifier si la réponse est réussie
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Client supprimé avec succès.");
                    return true;
                }
                else
                {
                    // Afficher les détails de l'erreur
                    Console.WriteLine($"Erreur lors de la suppression du client : {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la suppression du client : {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProduitAsync(string idProduit, string token)
        {
            var apiUrl = $"http://infotools.test/api/produits/{idProduit}";  // L'ID doit être une chaîne
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            try
            {
                // Faire la requête DELETE pour supprimer le prospect
                HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                // Vérifier si la réponse est réussie
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Produit supprimé avec succès.");
                    return true;
                }
                else
                {
                    // Afficher les détails de l'erreur
                    Console.WriteLine($"Erreur lors de la suppression du produit : {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de la suppression du produit : {ex.Message}");
                return false;
            }
        }

        // Méthode pour mettre à jour un prospect
        public async Task<bool> UpdateProspectAsync(Prospect prospect, string token)
        {
            var apiUrl = $"http://infotools.test/api/prospects/{prospect.IdProspect}";  // L'ID du prospect à mettre à jour
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            var updateData = new
            {
                nom = prospect.NomProspect,
                prenom = prospect.PrenomProspect,
                tel = prospect.TelProspect,
                mail = prospect.MailProspect,
            };

            var content = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

            try
            {
                // Faire la requête PUT
                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;  // Mise à jour réussie
                }
                else
                {
                    Console.WriteLine($"Erreur lors de la mise à jour : {response.Content.ReadAsStringAsync().Result}");
                    return false;  // Mise à jour échouée
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur réseau : {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateProduitAsync(Produit produit, string token)
        {
            var apiUrl = $"http://infotools.test/api/produits/{produit.IdProduit}";  // L'ID du prospect à mettre à jour
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            var updateData = new
            {
                typeProd = produit.TypeProduit,
                prixProd = produit.PrixProduit,
                nomProd = produit.NomProduit,
                descProd = produit.DescProduit,
            };

            var content = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

            try
            {
                // Faire la requête PUT
                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;  // Mise à jour réussie
                }
                else
                {
                    Console.WriteLine($"Erreur lors de la mise à jour : {response.Content.ReadAsStringAsync().Result}");
                    return false;  // Mise à jour échouée
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur réseau : {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateClientAsync(Client cli, string token)
        {
            var apiUrl = $"http://infotools.test/api/clients/{cli.IdCli}";  // L'ID du prospect à mettre à jour
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Utilisation du token dynamique

            var updateData = new
            {
                cp = cli.CpCli,
                ville = cli.VilleCli,
                adresse = cli.RueCli,
            };

            var content = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

            try
            {
                // Faire la requête PUT
                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;  // Mise à jour réussie
                }
                else
                {
                    Console.WriteLine($"Erreur lors de la mise à jour : {response.Content.ReadAsStringAsync().Result}");
                    return false;  // Mise à jour échouée
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur réseau : {ex.Message}");
                return false;
            }
        }

    }
}
