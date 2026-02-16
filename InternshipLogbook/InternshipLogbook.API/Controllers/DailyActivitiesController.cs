using InternshipLogbook.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipLogbook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyActivitiesController: ControllerBase
    {
        private readonly InternshipLogbookDbContext _context; // legatura bd
        
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
        public async Task<IActionResult> PostDailyActivity(int studentId, DailyActivity dailyActivity)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return NotFound($"Student with ID {studentId} not found.");
            
            dailyActivity.Id = 0;
            dailyActivity.StudentId = studentId;

            _context.DailyActivities.Add(dailyActivity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostDailyActivity),
                new { id = dailyActivity.Id }, dailyActivity);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDailyActivity(int id)
        {
            var dailyActivity = await _context.DailyActivities.FindAsync(id);
    
            if (dailyActivity == null)
            {
                return NotFound();
            }
            
            _context.DailyActivities.Remove(dailyActivity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDailyActivity(int id, DailyActivity dailyActivity)
        {
            var existingActivity = await _context.DailyActivities.FindAsync(id);
            if(existingActivity ==  null)
                return NotFound();
            existingActivity.DayNumber = dailyActivity.DayNumber;
            existingActivity.DateOfActivity = dailyActivity.DateOfActivity;
            existingActivity.TimeFrame = dailyActivity.TimeFrame;
            existingActivity.Venue = dailyActivity.Venue;
            existingActivity.Activities = dailyActivity.Activities;
            existingActivity.EquipmentUsed = dailyActivity.EquipmentUsed;
            existingActivity.SkillsPracticed = dailyActivity.SkillsPracticed;
            existingActivity.Observations = dailyActivity.Observations;

            await _context.SaveChangesAsync();

            return Ok(existingActivity);

        }
    }
}