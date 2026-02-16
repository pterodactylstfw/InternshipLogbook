using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InternshipLogbook.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Va fi folosit ca Username

        [Required]
        [JsonIgnore] // ignor ca nu trimit catre frontend niciodata
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Student"; // sau coordinator
        
        public int? StudentId { get; set; } // optional, coordonatorul nu e student

        public virtual Student? Student { get; set; }
        
        public string SecurityStamp { get; set; } = Guid.NewGuid().ToString(); // invalidare jwt
    }
}