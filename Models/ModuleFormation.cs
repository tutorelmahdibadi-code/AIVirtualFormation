using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    // "Module" est le terme du cahier des charges (gestion des modules/cours).
    // Nommé ModuleFormation pour eviter toute ambiguite avec le mot-cle 'module'.
    public class ModuleFormation
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Ordre { get; set; }

        public int FormationId { get; set; }

        [JsonIgnore]
        public Formation? Formation { get; set; }

        public List<Modalite>? Modalites { get; set; }
    }
}
