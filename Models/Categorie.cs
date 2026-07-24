using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Formation>? Formations { get; set; }
    }
}
