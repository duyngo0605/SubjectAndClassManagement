using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectAndClassManagement.Models;
using System.Net;
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
        // GET: StuGrade/EditResults/5
        public async Task<IActionResult> EditResults(string id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var sclass = await _context.Classes
                .Include(s => s.Room)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.class_id == id);

            if (sclass == null)
            {
                return NotFound();
            }

            // Lấy danh sách sinh viên của lớp học
            var students = await _context.StudentRegistrations
                .Include(sr => sr.Student)
                .Include(sr => sr.StudentResult)
                .ThenInclude(sr => sr.ResultColumns)
                .Where(sr => sr.class_id == id)
                .ToListAsync();

            ViewData["ClassName"] = $"{sclass.Subject.subject_name} - {sclass.Teacher.teacher_name} - {sclass.class_id}";

            return View("EditResults", students);
        }

        // POST: StuGrade/EditResults/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResults(string id, Dictionary<string, Dictionary<string, Dictionary<string, string>>> grades)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var registrationId in grades.Keys)
                {
                    var resultColumnsMap = grades[registrationId];

                    // Retrieve StudentResult based on registrationId
                    var registration = await _context.StudentRegistrations
                        .Include(sr => sr.Class)
                        .Include(sr => sr.StudentResult)
                            .ThenInclude(sr => sr.ResultColumns)
                        .FirstOrDefaultAsync(sr => sr.class_id == id && sr.registration_id == registrationId);

                    if (registration != null)
                    {
                        // Update ResultColumns for the student
                        foreach (var columnName in resultColumnsMap.Keys)
                        {
                            var columnValues = resultColumnsMap[columnName];

                            if (registration.StudentResult == null)
                            {
                                registration.StudentResult = new StudentResult {
                                    student_results_id = "R_"+ registration.registration_id, 
                                    registration_id = registration.registration_id 
                                };
                                _context.StudentResults.Add(registration.StudentResult);
                            }

                            // Find or create the ResultColumn
                            var resultColumn = registration.StudentResult.ResultColumns.FirstOrDefault(rc => rc.column_name == columnName);

                            if (resultColumn == null)
                            {
                                resultColumn = new ResultColumn { column_name = columnName };
                                registration.StudentResult.ResultColumns.Add(resultColumn);
                            }

                            // Update grade and weight for the ResultColumn
                            resultColumn.grade = Convert.ToDouble(columnValues["grade"]);
                            resultColumn.weight = Convert.ToDouble(columnValues["weight"]);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                // Lưu thông báo thành công vào TempData
                TempData["SuccessMessage"] = "Edit results successfully!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            string decodedId = WebUtility.UrlDecode(id);

            return RedirectToAction("DisplayStudents", "Classes", new { id = decodedId });

        }



    }
}