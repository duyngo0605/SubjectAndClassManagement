using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectAndClassManagement.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isStudent = User.IsInRole("student");
            var isTeacher = User.IsInRole("teacher");
            IEnumerable<Class> classes;

            if (isStudent)
            {
                classes = await _context.StudentRegistrations
                    .Where(sr => sr.student_id == userId && sr.status == "Registered")
                    .Include(sr => sr.Class)
                        .ThenInclude(c => c.Subject)
                    .Include(sr => sr.Class)
                        .ThenInclude(c => c.Room)
                    .Include(sr => sr.Class)
                        .ThenInclude(c => c.Teacher)
                    .Select(sr => sr.Class)
                    .ToListAsync();
            }
            else if (isTeacher)
            {
                classes = await _context.Classes
                    .Where(c => c.teacher_id == userId)
                    .Include(c => c.Subject)
                    .Include(c => c.Room)
                    .Include(c => c.Teacher)
                    .ToListAsync();
            }
            else
            {
                return Unauthorized();
            }

            var semesters = classes.Select(c => c.semester).Distinct().OrderByDescending(s => s);
            var academicYears = classes.Select(c => c.academic_year).Distinct().OrderByDescending(y => y);
            var latestSem = semesters.FirstOrDefault();
            var latestYear = academicYears.FirstOrDefault();

            // ViewBag with latest semester
            ViewBag.Semesters = new SelectList(semesters, selectedValue: latestSem);
            ViewBag.AcademicYears = new SelectList(academicYears, selectedValue: latestYear);
            ViewBag.SelectedSemester = latestSem;
            ViewBag.SelectedAcademicYear = latestYear;

            return View(classes);
        }
    }
}