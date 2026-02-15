using InternshipLogbook.API.Models;

namespace InternshipLogbook.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InternshipLogbookDbContext context)
        {
            context.Database.EnsureCreated();


            if (context.Students.Any())
            {
                return;
            }
            
            var faculty = new Faculty
            {
                Name = "Facultatea de Inginerie Electrică și Știința Calculatoarelor",
                ShortName = "IESC"
            };
            context.Faculties.Add(faculty);
            context.SaveChanges();
            
            var studyProgramme = new StudyProgramme
            {
                Name = "Calculatoare",
                FacultyId = faculty.Id
            };
            context.StudyProgrammes.Add(studyProgramme);
            context.SaveChanges();
            
            var company = new Company
            {
                Name = "Siemens Industry Software",
                Description = "Lider global în domeniul software-ului industrial și al soluțiilor de automatizare. " +
                              "Compania dezvoltă platforme precum Teamcenter și NX, utilizate în industria auto și aerospațială.",
                Address = "Brașov, Str. Turnului 5"
            };
            context.Companies.Add(company);
            context.SaveChanges();
            
            var student = new Student
            {
                FullName = "Alexandru Popa",
                YearOfStudy = 3,
                StudyProgrammeId = studyProgramme.Id,
                CompanyId = company.Id,
                InternshipPeriod = "01.07.2026 - 21.07.2026",
                InternshipDirector = "Conf. Dr. Ing. Mihai Ionescu",
                HostTutor = "Ing. Andrei Radu",
                
                EvaluationQuality = "Studentul a demonstrat o capacitate excelentă de învățare și adaptare la tehnologiile utilizate în proiect.",
                EvaluationCommunication = "Comunicare eficientă cu echipa, a participat activ la ședințele zilnice (Daily Stand-up).",
                EvaluationLearning = "A asimilat rapid conceptele de .NET și Angular.",
                SuggestedGrade = 10
            };
            context.Students.Add(student);
            context.SaveChanges();
            
            var activities = new List<DailyActivity>();
            DateTime startDate = new DateTime(2026, 7, 1);

            for (int i = 1; i <= 15; i++)
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday) startDate = startDate.AddDays(2);
                if (startDate.DayOfWeek == DayOfWeek.Sunday) startDate = startDate.AddDays(1);

                activities.Add(new DailyActivity
                {
                    StudentId = student.Id,
                    DayNumber = i,
                    DateOfActivity = DateOnly.FromDateTime(startDate),
                    TimeFrame = "09:00 - 17:00",
                    Venue = "Sediul Firmei / Birou R&D",
                    Activities = $"Studiul documentației proiectului și configurarea mediului de dezvoltare (Ziua {i}). " +
                                 "Implementare funcționalități Backend în C#.",
                    EquipmentUsed = "Laptop, Visual Studio 2026, SQL Server",
                    SkillsPracticed = "C#, Entity Framework, Git",
                    Observations = "Am învățat cum se face debugging eficient într-o aplicație Enterprise."
                });

                startDate = startDate.AddDays(1);
            }

            context.DailyActivities.AddRange(activities);
            context.SaveChanges();
        }
    }
}