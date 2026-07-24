using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Enonce { get; set; } = string.Empty;

        public int ModaliteId { get; set; }

        [JsonIgnore]
        public Modalite? Modalite { get; set; }

        public List<ReponseOption>? Options { get; set; }
    }
}
