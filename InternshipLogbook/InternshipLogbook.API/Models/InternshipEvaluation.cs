using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InternshipLogbook.API.Models
{
    public class InternshipEvaluation
    {
        [Key]
        public int Id { get; set; }

        // --- RELAȚIA CU STUDENTUL (Foreign Key) ---
        public int StudentId { get; set; }
        
        [JsonIgnore] // Evităm cicluri infinite în JSON
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        // --- COMPETENȚE SPECIFICE (S1 - S13) ---
        // Valori: 1 (Beginner), 2 (Intermediate), 3 (Advanced)
        public int? TaskSequencing { get; set; }      // S1
        public int? Documentation { get; set; }       // S2
        public int? TheoreticalKnowledge { get; set; } // S3
        public int? ToolsUsage { get; set; }          // S4
        public int? Measurements { get; set; }        // S5
        public int? DataRecording { get; set; }       // S6
        public int? GraphicPlans { get; set; }        // S7
        public int? Techniques { get; set; }          // S8
        public int? SafetyFeatures { get; set; }      // S9
        public int? ManualAssembly { get; set; }      // S10
        public int? ElectricalAssembly { get; set; }  // S11
        public int? Design { get; set; }              // S12
        public int? Testing { get; set; }             // S13

        // --- COMPETENȚE GENERALE (G1 - G7) ---
        public int? CompanyKnowledge { get; set; }    // G1
        public int? CommunicationTeam { get; set; }   // G2
        public int? CommunicationClients { get; set; } // G3
        public int? Reflection { get; set; }          // G4
        public int? EthicalConduct { get; set; }      // G5
        public int? OrgCulture { get; set; }          // G6
        public int? CareerAnalysis { get; set; }      // G7
        
        // Data evaluării (pentru semnătură)
        public DateTime EvaluationDate { get; set; } = DateTime.Now;
    }
}