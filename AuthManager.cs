using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCRM
{
    internal class AuthManager
    {
        // Variable pour stocker le token
        private static string _authToken;

        // Propriété pour récupérer et définir le token
        public static string AuthToken
        {
            get { return _authToken; }
            set { _authToken = value; }
        }

        // Méthode pour vérifier si un token est présent
        public static bool IsTokenAvailable()
        {
            return !string.IsNullOrEmpty(_authToken);
        }

        // Méthode pour effacer le token (par exemple lors de la déconnexion)
        public static void ClearToken()
        {
            _authToken = null;
        }
    }
}
