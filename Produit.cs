using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCRM
{
    public class Produit
    {

        [JsonProperty("id")]
        public int IdProduit { get; set; }

        [JsonProperty("typeProd")]
        public string TypeProduit { get; set; }

        [JsonProperty("prixProd")]
        public double PrixProduit { get; set; }

        [JsonProperty("nomProd")]
        public string NomProduit { get; set; }

        [JsonProperty("descProd")]
        public string DescProduit { get; set; }

        // Constructeur avec paramètres
        public Produit(int idProd, string typeProd, double prixProd , string nomProd, string descProd)
        {
            IdProduit = idProd;
            TypeProduit = typeProd;
            PrixProduit = prixProd;
            NomProduit = nomProd;
            DescProduit = descProd;
        }

        public int? GetId()
        {
            return IdProduit;
        }
    }
}
