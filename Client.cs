using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppCRM
{
    public class Client
    {
        [JsonProperty("id")]
        public int? IdCli { get; set; }  // ID du client

        [JsonProperty("CpClient")]
        public string CpCli { get; set; }  // Code postal

        [JsonProperty("VilleClient")]
        public string VilleCli { get; set; }  // Ville

        [JsonProperty("AdresseClient")]
        public string RueCli { get; set; }  // Rue

        [JsonProperty("idProspects")]
        public int? IdProspect { get; set; }  // ID du prospect correspondant (ID du Prospect dans la classe Prospect)

        // Ajoutez les propriétés pour stocker les informations du Prospect
        public string NomProspect { get; set; }
        public string PrenomProspect { get; set; }
        public string MailProspect { get; set; }
        public string TelProspect { get; set; }

        // Constructeur avec paramètres
        public Client(int? idCli, string cpCli, string villeCli, string rueCli, int? idProspect)
        {
            IdCli = idCli;
            CpCli = cpCli;
            VilleCli = villeCli;
            RueCli = rueCli;
            IdProspect = idProspect;
        }

        // Méthode pour obtenir l'ID du client
        public int? GetId()
        {
            return IdCli;
        }
    }
}
