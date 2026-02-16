using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InternshipLogbook.API.Models
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Ex: IESC

        public string? ShortName { get; set; } // Ex: IESC
        
        [JsonIgnore]
        public virtual ICollection<StudyProgramme> StudyProgrammes { get; set; } = new List<StudyProgramme>();
    }
}