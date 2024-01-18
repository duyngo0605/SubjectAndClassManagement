using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectAndClassManagement.Models;
using System.Security.Claims;

namespace SubjectAndClassManagement.Controllers
{
    public class StuGradeController : Controller
    {
        private readonly SchoolContext _context;

        public StuGradeController(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách lớp để hiển thị trong dropdown
            ViewBag.Classes = _context.Classes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(string classId)
        {
            // Lấy danh sách sinh viên của lớp đã chọn
            var students = _context.Students
                .Where(s => s.Registrations.Any(r => r.class_id == classId))
                .ToList();

            ViewBag.ClassId = classId;
            return View(students);
        }

        [HttpPost]
        public IActionResult UpdateGrade(string studentId, string classId, float grade)
        {
            // Xử lý cập nhật điểm của sinh viên
            // ...

            // Chuyển hướng về trang nhập điểm
            return RedirectToAction("Index", new { classId });
        }

        public async Task<IActionResult> Details(string id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.StudentResult)
                        .ThenInclude(r => r.ResultColumns)
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.Class)
                        .ThenInclude(c => c.Subject)
                .FirstOrDefaultAsync(m => m.student_id == id);

            return View(student);
        }

    }
}
