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
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch classes where the student registered
            var registeredClasses = await _context.StudentRegistrations
                .Where(sr => sr.student_id == studentId && sr.status == "Registered")
                .Include(sr => sr.Class) // Include Class
                    .ThenInclude(c => c.Subject)
                .Include(sr => sr.Class) 
                    .ThenInclude(c => c.Room)
                .Include(sr => sr.Class) 
                    .ThenInclude(c => c.Teacher) 
                .Select(sr => sr.Class) 
                .ToListAsync();

            return View(registeredClasses);
        }
    }
}