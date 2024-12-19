using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SubjectAndClassManagement.Models;
using System.Security.Claims;

namespace SubjectAndClassManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            if (User.IsInRole("student"))
            {
                await Details(User.FindFirstValue("StudentId"));
                return View("Details");
            }
            else
            {
                var studentsQuery = _context.Students.AsQueryable();

                if (!String.IsNullOrEmpty(searchString))
                {
                    studentsQuery = studentsQuery.Where(s =>
                        s.student_name.Contains(searchString)
                        || s.email.Contains(searchString)
                        || s.phone_number.Contains(searchString)
                    );
                }

                var students = await studentsQuery.ToListAsync();
                return View(students);
            }
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
            .Include(s => s.User)               // Include User
            .ThenInclude(u => u.Profile)    // ThenInclude Profile inside User
            .Include(s => s.Registrations)
            .ThenInclude(s=>s.Class)
                .ThenInclude(s=>s.Subject)
            .Include(s => s.Registrations)
            .ThenInclude(s => s.Class)
                .ThenInclude(s => s.Teacher)
            .FirstOrDefaultAsync(m => m.student_id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("student_id,student_name,email,phone_number, academic_year")] Student student)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Action for export to Excel
        // Action for export to Excel
        public IActionResult ExportToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Students");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Student ID";
                worksheet.Cell(currentRow, 2).Value = "Student Name";
                worksheet.Cell(currentRow, 3).Value = "Email";
                worksheet.Cell(currentRow, 4).Value = "Phone Number";
                worksheet.Cell(currentRow, 5).Value = "Academic Year"; // Add a header for Academic Year

                var students = _context.Students.ToList();
                foreach (var student in students)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = student.student_id;
                    worksheet.Cell(currentRow, 2).Value = student.student_name;
                    worksheet.Cell(currentRow, 3).Value = student.email;
                    worksheet.Cell(currentRow, 4).Value = student.phone_number;
                    worksheet.Cell(currentRow, 5).Value = student.academic_year; // Add the academic year value
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid Excel file.";
                return RedirectToAction(nameof(Index));
            }

            var students = new List<Student>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        TempData["ErrorMessage"] = "The Excel file is empty or invalid.";
                        return RedirectToAction(nameof(Index));
                    }

                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Bỏ qua dòng đầu tiên (header)
                    {
                        var studentId = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        var studentName = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                        var email = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                        var phoneNumber = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                        var academicYearString = worksheet.Cells[row, 5].Value?.ToString()?.Trim();

                        // Chuyển đổi academic_year từ string sang int
                        if (!int.TryParse(academicYearString, out int academicYear))
                        {
                            continue; // Bỏ qua dòng nếu giá trị academic_year không hợp lệ
                        }

                        if (!string.IsNullOrEmpty(studentId) &&
                            !string.IsNullOrEmpty(studentName) &&
                            academicYear > 0)
                        {
                            var student = new Student
                            {
                                student_id = studentId,
                                student_name = studentName,
                                email = email,
                                phone_number = phoneNumber,
                                academic_year = academicYear
                            };

                            students.Add(student);
                        }
                    }
                }
            }

            if (students.Any())
            {
                // Tránh thêm trùng lặp
                foreach (var student in students)
                {
                    if (!_context.Students.Any(s => s.student_id == student.student_id))
                    {
                        _context.Students.Add(student);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Students imported successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "No valid data found in the Excel file.";
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
               .Include(s => s.User)               // Include User
               .ThenInclude(u => u.Profile)    // ThenInclude Profile inside User
               .FirstOrDefaultAsync(m => m.student_id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("student_id,student_name,email,phone_number, academic_year, User")] Student student)
        {
            try
            {
                
                _context.Update(student);
                if (User.IsInRole("student"))
                { 
                    var profile = student.User.Profile; 
                    _context.Update(profile); 
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.student_id))
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

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.student_id == id);

            // Check if there are linked users
            var user = await _context.Users.FirstOrDefaultAsync(u => u.student_id == id);

            if (user != null)
            {
                // Display confirmation dialog
                ViewData["LinkedUser"] = user;
                return View("Delete", student);
            }

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'SchoolContext.Students'  is null.");
            }
            var student = await _context.Students
                .Include(u => u.User)
                .ThenInclude(u => u.Profile)
                .FirstOrDefaultAsync(m => m.student_id == id);
            if (student != null)
            {
                if (student.User != null)
                {
                    _context.Profiles.Remove(student.User.Profile);
                    _context.Users.Remove(student.User);
                }    
                _context.Students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
          return (_context.Students?.Any(e => e.student_id == id)).GetValueOrDefault();
        }


    }
}
