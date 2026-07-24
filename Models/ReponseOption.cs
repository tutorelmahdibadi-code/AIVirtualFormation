using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class ReponseOption
    {
        public int Id { get; set; }
        public string Texte { get; set; } = string.Empty;
        public bool EstCorrecte { get; set; }

        public int QuestionId { get; set; }

        [JsonIgnore]
        public Question? Question { get; set; }
    }
}
