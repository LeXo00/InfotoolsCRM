using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using AppCRM;

public class EmailPasswordSender
{
    private static readonly HttpClient client = new HttpClient();

    // Méthode pour vérifier si l'email et le mot de passe correspondent dans l'API
    public async Task<(bool, string)> VerifyAdminAsync(string email, string password)
    {
        string apiUrl = "http://infotools.test/api/login";

        var credentials = new { email = email, psw = password };
        string jsonContent = JsonConvert.SerializeObject(credentials);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.Content.Headers.ContentType.MediaType != "application/json")
            {
                return (false, "L'API ne retourne pas du JSON.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(responseContent);
                return (false, errorResponse.Message);
            }

            var responseObject = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
            if (responseObject.Success)
            {
                if (responseObject.Data.Role?.ToLower() == "manager")
                {
                    AuthManager.AuthToken = responseObject.Data.Token;
                    return (true, null);
                }
                else
                {
                    return (false, "Vous n'avez pas l'autorisation d'accéder à cette application.");
                }
            }

            return (false, "Identifiants incorrects ou utilisateur inconnu.");
        }
        catch (Exception ex)
        {
            return (false, "Erreur de connexion à l'API : " + ex.Message);
        }
    }
}

// Classe pour désérialiser la réponse de l'API en cas de succès
public class ApiResponse
{
    public bool Success { get; set; }
    public ApiData Data { get; set; }
}

// Classe pour les données de la réponse (nom de l'utilisateur)
public class ApiData
{
    public string Name { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
}

// Classe pour désérialiser la réponse d'erreur de l'API
public class ApiErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
