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
        private readonly IWebHostEnvironment _env; // 

        public ExportController(InternshipLogbookDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("word/{studentId}")]
        public async Task<IActionResult> ExportToWord(int studentId)
        {
            var student = await _context.Students // eager load = get toate date din bd pt student
                .Include(s => s.Company)
                .Include(s => s.InternshipEvaluation)
                .Include(s => s.StudyProgramme)
                    .ThenInclude(sp => sp.Faculty)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null) return NotFound("Studentul nu există.");
            
            var activities = await _context.DailyActivities
                .Where(a => a.StudentId == studentId)
                .OrderBy(a => a.DayNumber)
                .ToListAsync();
            
            string templatePath = Path.Combine(_env.ContentRootPath, "Templates", "Template.docx"); // path template word
            if (!System.IO.File.Exists(templatePath))
                return StatusCode(500, "Template.docx nu a fost găsit în folderul Templates."); // 500 pt internal server error
            
            var service = new WordExportService();
            byte[] fileBytes = service.GenerateLogbook(student, activities, templatePath);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "Jurnal.docx"); // descarca fisier generat
        }
    }
}