using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipLogbook.API.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public int StudyProgrammeId { get; set; }

    [ForeignKey("StudyProgrammeId")]
    public virtual StudyProgramme? StudyProgramme { get; set; }
    public int YearOfStudy { get; set; }



    public string? InternshipPeriod { get; set; }
    public string? InternshipDirector { get; set; }
    
    public int? CompanyId { get; set; }
    
    [ForeignKey("CompanyId")]
    public virtual Company? Company { get; set; }


    public string? HostTutor { get; set; }
    
    
    public string? EvaluationQuality { get; set; }
    public string? EvaluationCommunication { get; set; }
    public string? EvaluationLearning { get; set; }
    public int? SuggestedGrade { get; set; }

    
    public virtual ICollection<DailyActivity> DailyActivities { get; set; } = new List<DailyActivity>();
}