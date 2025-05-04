using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCRM
{
    public class Prospect
    {

        [JsonProperty("id")]
        public int IdProspect { get; set; }

        [JsonProperty("NomProspects")]
        public string NomProspect { get; set; }

        [JsonProperty("PrenomProspects")]
        public string PrenomProspect { get; set; }

        [JsonProperty("EmailProspects")]
        public string MailProspect { get; set; }

        [JsonProperty("telProspects")]
        public string TelProspect { get; set; }

        [JsonProperty("mdpProspect")]
        public string MdpProspect { get; set; }


        // Constructeur avec paramètres
        public Prospect(int idProspect,string nomProspect, string prenomProspect, string mailProspect, string telProspect, string mdpProspect)
        {
            IdProspect = idProspect;
            NomProspect = nomProspect;
            MailProspect = mailProspect;
            PrenomProspect = prenomProspect;
            TelProspect = telProspect;
            MdpProspect = mdpProspect;
        }
       
        public int? getId()
        {
            return IdProspect;
        }

    }
}
