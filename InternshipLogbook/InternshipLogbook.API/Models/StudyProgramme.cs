using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InternshipLogbook.API.Models
{
    public class StudyProgramme
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Ex: Calculatoare
        
        public int FacultyId { get; set; }
        
        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}