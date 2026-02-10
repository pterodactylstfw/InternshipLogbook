using System;
using System.Collections.Generic;

namespace InternshipLogbook.API.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Faculty { get; set; } = null!;

    public string StudyProgramme { get; set; } = null!;

    public int YearOfStudy { get; set; }

    public string? InternshipPeriod { get; set; }

    public string? InternshipDirector { get; set; }

    public string? HostInstitution { get; set; }

    public string? HostTutor { get; set; }

    public string? HostDescription { get; set; }

    public virtual ICollection<DailyActivity> DailyActivities { get; set; } = new List<DailyActivity>();
}
