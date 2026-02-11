using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InternshipLogbook.API.Models;

public partial class DailyActivity
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int DayNumber { get; set; }

    public DateOnly? DateOfActivity { get; set; }

    public string? TimeFrame { get; set; }

    public string? Venue { get; set; }

    public string? Activities { get; set; }

    public string? EquipmentUsed { get; set; }

    public string? SkillsPracticed { get; set; }

    public string? Observations { get; set; }

    [JsonIgnore]
    public virtual Student? Student { get; set; }
}
