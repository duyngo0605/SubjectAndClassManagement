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

            // Lấy danh sách sinh viên của lớp học cùng với điểm
            var students = await _context.StudentRegistrations
                .Include(sr => sr.Student)
                .Include(sr => sr.StudentResult)
                    .ThenInclude(sr => sr.ResultColumns)
                .Where(sr => sr.class_id == id)
                .ToListAsync();

            // Lấy trọng số từ ClassWeights
            var classWeights = await _context.ClassWeights.FirstOrDefaultAsync(cw => cw.class_id == id);

            // Chuyển đổi danh sách sinh viên thành danh sách DTO
            var studentResults = students.Select(sr => new StudentResultDTO
            {
                Student = sr.Student,
                AttendanceGrade = sr.StudentResult?.ResultColumns.FirstOrDefault(r => r.column_name == "Attendance")?.grade ?? 0,
                MidtermGrade = sr.StudentResult?.ResultColumns.FirstOrDefault(r => r.column_name == "Midterm")?.grade ?? 0,
                FinalGrade = sr.StudentResult?.ResultColumns.FirstOrDefault(r => r.column_name == "Final")?.grade ?? 0,
            }).ToList();

            // Tính điểm trung bình cho từng sinh viên
            foreach (var studentResult in studentResults)
            {
                studentResult.AverageGrade = (studentResult.AttendanceGrade + studentResult.MidtermGrade + studentResult.FinalGrade) / 3;
            }

            ViewData["ClassName"] = $"{sclass.Subject.subject_name} - {sclass.Teacher.teacher_name} - {sclass.class_id}";

            return View("EditResults", new { StudentResults = studentResults, ClassWeights = classWeights }); // Truyền danh sách sinh viên và trọng số vào view
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

        [HttpPost]
        public async Task<IActionResult> UpdateGrades(
    string studentId,
    double attendanceGrade,
    double midtermGrade,
    double finalGrade)
        {
            // Tìm StudentRegistration dựa trên studentId
            var registration = await _context.StudentRegistrations
                .Include(sr => sr.StudentResult)
                    .ThenInclude(sr => sr.ResultColumns)
                .FirstOrDefaultAsync(sr => sr.Student.student_id == studentId);

            if (registration == null)
            {
                return NotFound("Student registration not found.");
            }

            // Đảm bảo StudentResult được khởi tạo
            if (registration.StudentResult == null)
            {
                registration.StudentResult = new StudentResult
                {
                    student_results_id = "R_" + registration.registration_id,
                    registration_id = registration.registration_id,
                    ResultColumns = new List<ResultColumn>()
                };
                _context.StudentResults.Add(registration.StudentResult);
            }

            // Đảm bảo ResultColumns được khởi tạo
            if (registration.StudentResult.ResultColumns == null)
            {
                registration.StudentResult.ResultColumns = new List<ResultColumn>();
            }

            // Cập nhật điểm cho các ResultColumns
            UpdateOrAddColumn(registration.StudentResult, "Attendance", attendanceGrade);
            UpdateOrAddColumn(registration.StudentResult, "Midterm", midtermGrade);
            UpdateOrAddColumn(registration.StudentResult, "Final", finalGrade);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng về trang EditResults
            return RedirectToAction("EditResults", new { id = registration.class_id });
        }


        private void UpdateOrAddColumn(StudentResult studentResult, string columnName, double grade)
        {
            // Đảm bảo ResultColumns được khởi tạo (dự phòng)
            if (studentResult.ResultColumns == null)
            {
                studentResult.ResultColumns = new List<ResultColumn>();
            }

            // Cập nhật hoặc thêm mới cột
            var column = studentResult.ResultColumns.FirstOrDefault(rc => rc.column_name == columnName);
            if (column != null)
            {
                column.grade = grade;
            }
            else
            {
                studentResult.ResultColumns.Add(new ResultColumn
                {
                    column_name = columnName,
                    grade = grade,
                });
            }
        }




        [HttpPost]
        public async Task<IActionResult> UpdateWeights(string classId, double attendanceWeight, double midtermWeight, double finalWeight)
        {

            if (Math.Abs(attendanceWeight + midtermWeight + finalWeight - 1.0) > 0.01)
            {
                return BadRequest("The sum of weights must be equal to 1.");
            }

            var classWeights = await _context.ClassWeights.FirstOrDefaultAsync(cw => cw.class_id == classId);
            if (classWeights == null)
            {
                return NotFound();
            }

            // Cập nhật trọng số
            classWeights.attendance_weight = attendanceWeight;
            classWeights.midterm_weight = midtermWeight;
            classWeights.final_weight = finalWeight;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return RedirectToAction("EditResults", new { id = classId });
        }

    }
}