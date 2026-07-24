using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class Formation
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int CategorieId { get; set; }
        public Categorie? Categorie { get; set; }

        public int FormateurId { get; set; }
        public Formateur? Formateur { get; set; }

        public List<ModuleFormation>? Modules { get; set; }

        [JsonIgnore]
        public List<Inscription>? Inscriptions { get; set; }

        public List<SessionFormation>? Sessions { get; set; }
    }
}
