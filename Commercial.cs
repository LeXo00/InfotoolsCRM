using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCRM
{
    public class Commercial
    {
        [JsonProperty("id")]
        public int IdCommercial { get; set; }

        [JsonProperty("cpCom")]
        public string CpCommercial { get; set; }

        [JsonProperty("villeCom")]
        public string VilleCommercial { get; set; }

        [JsonProperty("rueCom")]
        public string RueCommercial { get; set; }

        [JsonProperty("telCom")]
        public string TelCommercial { get; set; }

        [JsonProperty("idUser")]
        public int IdUser { get; set; }

        // Constructeur avec paramètres
        public Commercial(int idCom, string cpCom, string villeCom, string rueCom, string telCom, int idU)
        {
            IdCommercial = idCom;
            CpCommercial = cpCom;
            VilleCommercial = villeCom;
            RueCommercial = rueCom;
            TelCommercial = telCom;
            IdUser = idU;
        }

        public int? GetId()
        {
            return IdCommercial;
        }
    }
}
