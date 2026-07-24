using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class Formateur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Biographie { get; set; }

        // Lien vers le compte de connexion du formateur
        public int UtilisateurId { get; set; }

        [JsonIgnore]
        public Utilisateur? Utilisateur { get; set; }

        // --- Configuration de l'avatar IA (formateur virtuel) ---

        // ID du persona configure directement dans lab.anam.ai (visible dans l'URL
        // de la page de build : lab.anam.ai/build/{personaId}). C'est la methode
        // recommandee : le prompt systeme, l'avatar, la voix et le LLM sont deja
        // configures dans le Lab, pas besoin de les dupliquer ici.
        public string? PersonaId { get; set; }

        // Alternative (optionnelle) : configurer manuellement sans persona pre-cree.
        // Utilise seulement si PersonaId est vide.
        public string? AvatarId { get; set; }
        public string? AvatarModel { get; set; }
        public string? VoiceId { get; set; }
        public string? LlmId { get; set; }
        public string? SystemPrompt { get; set; }

        [JsonIgnore]
        public List<Formation>? Formations { get; set; }
    }
}
