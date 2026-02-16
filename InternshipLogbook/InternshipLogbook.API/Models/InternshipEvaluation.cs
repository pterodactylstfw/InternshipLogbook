using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InternshipLogbook.API.Models
{
    public class InternshipEvaluation
    {
        [Key]
        public int Id { get; set; }
        
        public int StudentId { get; set; } //FK student
        
        [JsonIgnore]
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        // comp specifice
        public int? TaskSequencing { get; set; }      
        public int? Documentation { get; set; }       
        public int? TheoreticalKnowledge { get; set; }
        public int? ToolsUsage { get; set; }
        public int? Measurements { get; set; }
        public int? DataRecording { get; set; }
        public int? GraphicPlans { get; set; } 
        public int? Techniques { get; set; }
        public int? SafetyFeatures { get; set; }
        public int? ManualAssembly { get; set; }
        public int? ElectricalAssembly { get; set; }
        public int? Design { get; set; }
        public int? Testing { get; set; }     

        // comp generale
        public int? CompanyKnowledge { get; set; }
        public int? CommunicationTeam { get; set; }
        public int? CommunicationClients { get; set; }
        public int? Reflection { get; set; }
        public int? EthicalConduct { get; set; }
        public int? OrgCulture { get; set; }      
        public int? CareerAnalysis { get; set; }
        
        // data eval
        public DateTime EvaluationDate { get; set; } = DateTime.Now;
    }
}