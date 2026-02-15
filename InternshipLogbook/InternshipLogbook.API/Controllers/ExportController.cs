using InternshipLogbook.API.Models;
using InternshipLogbook.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipLogbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly InternshipLogbookDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ExportController(InternshipLogbookDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("word/{studentId}")]
        public async Task<IActionResult> ExportToWord(int studentId)
        {
            // 1. Încărcăm studentul cu TOATE relațiile (Eager Loading)
            var student = await _context.Students
                .Include(s => s.Company)              // JOIN cu Companies
                .Include(s => s.InternshipEvaluation)
                .Include(s => s.StudyProgramme)       // JOIN cu StudyProgrammes
                    .ThenInclude(sp => sp.Faculty)    // JOIN cu Faculties (prin StudyProgramme)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null) return NotFound("Studentul nu există.");

            // 2. Încărcăm activitățile
            var activities = await _context.DailyActivities
                .Where(a => a.StudentId == studentId)
                .OrderBy(a => a.DayNumber)
                .ToListAsync();

            // 3. Verificăm Template-ul
            string templatePath = Path.Combine(_env.ContentRootPath, "Templates", "Template.docx");
            if (!System.IO.File.Exists(templatePath))
                return StatusCode(500, "Template.docx nu a fost găsit în folderul Templates.");

            // 4. Generăm
            var service = new WordExportService();
            byte[] fileBytes = service.GenerateLogbook(student, activities, templatePath);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Jurnal.docx");
        }
    }
}