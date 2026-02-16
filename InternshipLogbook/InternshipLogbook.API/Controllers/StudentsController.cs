using InternshipLogbook.API.Models;
using InternshipLogbook.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

                [HttpGet("{id}")]
                public async Task<ActionResult<StudentDto>> GetStudentById(int id)
                {
                        var studentDto = await _context.Students
                                .Where(s => s.Id == id)
                                .Select(s => new StudentDto
                                {
                                        Id = s.Id,
                                        FullName = s.FullName,
                                        YearOfStudy = s.YearOfStudy,
                                        
                                        Faculty = s.StudyProgramme.Faculty.Name, 
                                        StudyProgramme = s.StudyProgramme.Name,
                                        
                                        HostInstitution = s.Company != null ? s.Company.Name : "-",
                                        HostDescription = s.Company != null ? s.Company.Description : "",
            
                                        HostTutor = s.HostTutor,
                                        InternshipPeriod = s.InternshipPeriod,
                                        InternshipDirector = s.InternshipDirector,
                                        
                                        EvaluationQuality = s.EvaluationQuality,
                                        EvaluationCommunication = s.EvaluationCommunication,
                                        EvaluationLearning = s.EvaluationLearning,
                                        SuggestedGrade = s.SuggestedGrade
                                })
                                .FirstOrDefaultAsync();

                        if (studentDto == null)
                        { 
                                return NotFound();
                        }

                        return studentDto;
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