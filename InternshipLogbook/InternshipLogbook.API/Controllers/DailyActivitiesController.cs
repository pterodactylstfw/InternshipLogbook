using InternshipLogbook.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipLogbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyActivitiesController: ControllerBase
    {
        private readonly InternshipLogbookDbContext _context;
        
        public DailyActivitiesController(InternshipLogbookDbContext context)
        {
            _context = context;
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<DailyActivity>>> GetDailyActivitiesByStudent(int studentId)
        {
            return await _context.DailyActivities
                .Where(activity => activity.StudentId == studentId)
                .OrderBy(activity => activity.DateOfActivity) // ord dupa data
                .ToListAsync();
        }

        [HttpPost("student/{studentId}")]
        public async Task<ActionResult<DailyActivity>> PostDailyActivitiesByStudent(int studentId,
            DailyActivity dailyActivity)
        {
            dailyActivity.StudentId = studentId;
            
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return BadRequest("Student not found");
            }
            
            _context.DailyActivities.Add(dailyActivity);
            await _context.SaveChangesAsync();
            
            
            return CreatedAtAction(
                nameof(GetDailyActivitiesByStudent), 
                new { studentId = dailyActivity.StudentId }, 
                dailyActivity
            );
        }
    }
}