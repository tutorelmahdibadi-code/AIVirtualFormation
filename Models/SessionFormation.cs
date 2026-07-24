using System.Text.Json.Serialization;

namespace VIRTUAL_CLASSE_FORMATION.Models
{
    public class SessionFormation
    {
        public int Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }

        public int FormationId { get; set; }

        [JsonIgnore]
        public Formation? Formation { get; set; }
    }
}
