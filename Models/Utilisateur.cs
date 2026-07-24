using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Jamais renvoyé au client
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        public Role Role { get; set; } = Role.Apprenant;

        // Rempli uniquement si Role == Formateur
        public Formateur? Formateur { get; set; }

        public List<Inscription>? Inscriptions { get; set; }
    }
}
