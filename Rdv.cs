using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCRM
{
    public class Rdv
    {

        [JsonProperty("id")]
        public int _idRdv { get; set; }

        [JsonProperty("DateRdv")]
        public string _dateRdv { get; set; }

        [JsonProperty("NoCom")]
        public int _idCommercial { get; set; }

        [JsonProperty("NoClient")]
        public int _idClient { get; set; }

        // Constructeur avec paramètres
        public Rdv(int idRdv, string dateRdv, int idClient, int idCom)
        {
            _idRdv = idRdv;
            _dateRdv = dateRdv;
            _idClient = idClient;
            _idCommercial = idCom;
        }

        public int? getId()
        {
            return _idRdv;
        }
    }
}
