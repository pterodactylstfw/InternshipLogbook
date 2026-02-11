using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternshipLogbook.API.Models;
namespace InternshipLogbook.API.Controllers
{

        [Route("api/[controller]")]
        [ApiController]
        public class StudentsController: ControllerBase
        {
                private readonly InternshipLogbookDbContext _context;

                public StudentsController(InternshipLogbookDbContext context)
                {
                        _context = context;
                }

                [HttpGet]
                public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
                {
                        return await _context.Students.ToListAsync();
                }

                [HttpPost]
                public async Task<ActionResult<Student>> PostStudent(Student student)
                {
                        _context.Students.Add(student);
                        
                        await _context.SaveChangesAsync();
                        
                        return CreatedAtAction("GetStudents", new {id = student.Id}, student);
                }
                
        }
}