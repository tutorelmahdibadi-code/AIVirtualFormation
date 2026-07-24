using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class Modalite
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public TypeModalite Type { get; set; }

        // Contenu texte/HTML pour un Cours, enonce pour un Exercice
        public string? Contenu { get; set; }

        public int ModuleFormationId { get; set; }

        [JsonIgnore]
        public ModuleFormation? ModuleFormation { get; set; }

        // Rempli uniquement si Type == Examen
        public List<Question>? Questions { get; set; }
    }
}
