namespace InternshipLogbook.API.DTOs
{
    public class StudentDto // dto pt transfer api - client
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        
        public string Faculty { get; set; } = string.Empty; 
        public string StudyProgramme { get; set; } = string.Empty;
        public int YearOfStudy { get; set; }

        public string? InternshipPeriod { get; set; }
        public string? InternshipDirector { get; set; }
        
        public string? HostInstitution { get; set; }
        public string? HostTutor { get; set; }
        public string? HostDescription { get; set; }
        
        public string? EvaluationQuality { get; set; }
        public string? EvaluationCommunication { get; set; }
        public string? EvaluationLearning { get; set; }
        public int? SuggestedGrade { get; set; }
    }
}