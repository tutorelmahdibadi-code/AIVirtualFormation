using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class Inscription
    {
        public int Id { get; set; }

        public int ApprenantId { get; set; }
        [JsonIgnore]
        public Utilisateur? Apprenant { get; set; }

        public int FormationId { get; set; }
        public Formation? Formation { get; set; }

        public DateTime DateInscription { get; set; } = DateTime.UtcNow;
        public StatutInscription Statut { get; set; } = StatutInscription.EnCours;

        // Pourcentage de progression (0 a 100)
        public int Progression { get; set; } = 0;
    }
}
