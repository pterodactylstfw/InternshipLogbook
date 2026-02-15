using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InternshipLogbook.API.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // == HostInstitution

        public string? Description { get; set; }
        public string? Address { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; } = new List<Student>(); // mai multi studenti
    }
}