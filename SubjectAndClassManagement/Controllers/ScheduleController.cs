using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectAndClassManagement.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SubjectAndClassManagement.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        private readonly SchoolContext _context;

        public ScheduleController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {
            var isStudent = User.IsInRole("student");
            var isTeacher = User.IsInRole("teacher");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (isStudent)
            {
                var registeredClasses = await _context.StudentRegistrations
                    .Where(sr => sr.student_id == userId && sr.status == "Registered")
                    .Include(sr => sr.Class) // Include Class
                        .ThenInclude(c => c.Subject)
                    .Include(sr => sr.Class)
                        .ThenInclude(c => c.Room)
                    .Include(sr => sr.Class)
                        .ThenInclude(c => c.Teacher)
                    .Select(sr => sr.Class)
                    .ToListAsync();

                return View("Index", registeredClasses);
            }
            else if (isTeacher)
            {
                var teachingClasses = await _context.Classes
                    .Where(c => c.teacher_id == userId)
                    .Include(c => c.Subject)
                    .Include(c => c.Room)
                    .Include(c => c.Teacher)
                    .ToListAsync();

                return View("Index", teachingClasses);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}