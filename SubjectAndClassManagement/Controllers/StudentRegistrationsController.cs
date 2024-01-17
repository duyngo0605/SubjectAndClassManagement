using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubjectAndClassManagement.Models;

namespace SubjectAndClassManagement.Controllers
{
    public class StudentRegistrationsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentRegistrationsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: StudentRegistrations
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.StudentRegistrations.Include(s => s.Class).Include(s => s.Student);
            return View(await schoolContext.ToListAsync());
        }

        // GET: StudentRegistrations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.StudentRegistrations == null)
            {
                return NotFound();
            }

            var studentRegistration = await _context.StudentRegistrations
                .Include(s => s.Class)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.registration_id == id);
            if (studentRegistration == null)
            {
                return NotFound();
            }

            return View(studentRegistration);
        }

        // GET: StudentRegistrations/Create
        public IActionResult Create()
        {
            ViewData["class_id"] = new SelectList(_context.Classes, "class_id", "class_id");
            ViewData["student_id"] = new SelectList(_context.Students, "student_id", "student_id");
            return View();
        }

        // POST: StudentRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("registration_id,student_id,class_id,registration_date,status,reason")] StudentRegistration studentRegistration)
        {
            _context.Add(studentRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: StudentRegistrations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.StudentRegistrations == null)
            {
                return NotFound();
            }

            var studentRegistration = await _context.StudentRegistrations.FindAsync(id);
            if (studentRegistration == null)
            {
                return NotFound();
            }
            ViewData["class_id"] = new SelectList(_context.Classes, "class_id", "class_id", studentRegistration.class_id);
            ViewData["student_id"] = new SelectList(_context.Students, "student_id", "student_id", studentRegistration.student_id);
            return View(studentRegistration);
        }

        // POST: StudentRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("registration_id,student_id,class_id,registration_date,status,reason")] StudentRegistration studentRegistration)
        {
            try
            {
                _context.Update(studentRegistration);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentRegistrationExists(studentRegistration.registration_id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: StudentRegistrations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.StudentRegistrations == null)
            {
                return NotFound();
            }

            var studentRegistration = await _context.StudentRegistrations
                .Include(s => s.Class)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.registration_id == id);
            if (studentRegistration == null)
            {
                return NotFound();
            }

            return View(studentRegistration);
        }

        // POST: StudentRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.StudentRegistrations == null)
            {
                return Problem("Entity set 'SchoolContext.StudentRegistrations'  is null.");
            }
            var studentRegistration = await _context.StudentRegistrations.FindAsync(id);
            if (studentRegistration != null)
            {
                _context.StudentRegistrations.Remove(studentRegistration);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("registration_id,student_id,registration_date,status,reason")] StudentRegistration studentRegistration, string[] selectedClasses)
        {
            var studentId = User.FindFirstValue("username");

            string success = "";
            string error = "";
            if (selectedClasses != null && selectedClasses.Length > 0)
            {
                foreach (var classId in selectedClasses)
                {
                    // Kiểm tra xem lớp đã đăng ký có trùng lịch với các lớp đã đăng ký trước đó không
                    if (IsClassScheduleConflict(studentId, classId))
                    {
                        // Nếu có trùng lịch, bạn có thể thực hiện các xử lý hoặc hiển thị thông báo lỗi
                        error+=$"Class schedule conflict for class {classId}. Registration failed.\n";
                        break;
                    }

                    success += $"Successfully registered for class {classId}.\n";
                    var newStudentRegistration = new StudentRegistration
                    {
                        registration_id = studentId + "_" + classId,
                        student_id = studentId,
                        class_id = classId,
                        registration_date = DateTime.Now,
                        status = "Registered",
                        reason = ""
                    };

                    _context.Add(newStudentRegistration);
                    await _context.SaveChangesAsync();
                }
                if (success != "")
                    TempData["Success"] = success;
                if (error != "")
                    TempData["Error"] = error;
            }

            return RedirectToAction("Register", "Classes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unregister(string[] selectedClasses)
        {
            var studentId = User.FindFirstValue("username");
            string success = "";
            if (selectedClasses != null && selectedClasses.Length > 0)
            {
                foreach (var classId in selectedClasses)
                {
                    var registration_id = studentId + "_" + classId;
                    var studentRegistration = await _context.StudentRegistrations.FindAsync(registration_id);
                    if (studentRegistration != null)
                    {
                        _context.StudentRegistrations.Remove(studentRegistration);
                        success += $"Successfully unregistered for class {classId}.\n";
                    }
                }

                await _context.SaveChangesAsync();

                if (success != "")
                    TempData["Success"] = success;
            }

            return RedirectToAction("Register", "Classes");
        }



        private bool StudentRegistrationExists(string id)
        {
          return (_context.StudentRegistrations?.Any(e => e.registration_id == id)).GetValueOrDefault();
        }

        private bool IsClassScheduleConflict(string studentId, string classId)
        {
            // Lấy thông tin về lớp học mà sinh viên đang cố gắng đăng ký
            var newClass = _context.Classes
                .Include(s => s.Room)
                .FirstOrDefault(c => c.class_id == classId);

            // Lấy danh sách các lớp học đã đăng ký
            var registeredClasses = _context.Classes
                .Include(s => s.Room)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Where(c => _context.StudentRegistrations.Any(sr => sr.class_id == c.class_id && sr.student_id == studentId))
                .ToList();

            // Kiểm tra xem lớp mới có trùng lịch với bất kỳ lớp đã đăng ký nào không
            foreach (var registeredClass in registeredClasses)
            {
                if (registeredClass.class_id != classId)
                {
                    // Kiểm tra xem có trùng lịch không
                    if (registeredClass.days_of_week == newClass.days_of_week)
                    {
                        if (newClass.first_period < registeredClass.first_period + registeredClass.class_period &&
                            registeredClass.first_period < newClass.first_period + newClass.class_period)
                        {
                            return true; // Có trùng lịch
                        }
                    }
                }
            }

            return false; // Không có trùng lịch
        }
    }
}
