using Newtonsoft.Json;
using Mysqlx.Expr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCRM
{
    public class Facture
    {
        [JsonProperty("id")]
        public int _idFact { get; set; }

        [JsonProperty("DateFact")]
        public string _dateFact { get; set; }

        [JsonProperty("idClient")]
        public int _idClient { get; set; }

        // Constructeur avec paramètres
        public Facture(int idFact, string dateFact, int idClient)
        {
            _idFact = idFact;
            _dateFact = dateFact;
            _idClient = idClient;
        }

        public int? getId()
        {
            return _idClient;
        }
    }
}
