using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SubjectAndClassManagement.Models;

namespace SubjectAndClassManagement.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SchoolContext _context;

        public TeachersController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("teacher"))
            {
                await Details(User.FindFirstValue("TeacherId"));
                return View("Details");
            }
            else
            {
                return _context.Teachers != null ?
                          View(await _context.Teachers.ToListAsync()) :
                          Problem("Entity set 'SchoolContext.Teachers'  is null.");
            }
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
            .Include(s => s.User)               // Include User
            .ThenInclude(u => u.Profile)    // ThenInclude Profile inside User
            .FirstOrDefaultAsync(m => m.teacher_id == id);
            
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("teacher_id,teacher_name,email,phone_number")] Teacher teacher)
        {
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
               .Include(s => s.User)               // Include User
               .ThenInclude(u => u.Profile)    // ThenInclude Profile inside User
               .FirstOrDefaultAsync(m => m.teacher_id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("teacher_id,teacher_name,email,phone_number, User")] Teacher teacher)
        {
            try
            {
                var profile = teacher.User.Profile;
                _context.Update(teacher);
                _context.Update(profile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacher.teacher_id))
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

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.teacher_id == id);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.teacher_id == id);

            if (user != null)
            {
                // Display confirmation dialog
                ViewData["LinkedUser"] = user;
                return View("Delete", teacher);
            }

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'SchoolContext.Teachers'  is null.");
            }
            var teacher = await _context.Teachers
               .Include(u => u.User)
               .ThenInclude(u => u.Profile)
               .FirstOrDefaultAsync(m => m.teacher_id == id);
            if (teacher != null)
            {
                if (teacher.User != null)
                {
                    _context.Profiles.Remove(teacher.User.Profile);
                    _context.Users.Remove(teacher.User);
                }
                _context.Teachers.Remove(teacher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(string id)
        {
          return (_context.Teachers?.Any(e => e.teacher_id == id)).GetValueOrDefault();
        }

        // Action for export to Excel
        public IActionResult ExportToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Teachers");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Teacher ID";
                worksheet.Cell(currentRow, 2).Value = "Teacher Name";
                worksheet.Cell(currentRow, 3).Value = "Email";
                worksheet.Cell(currentRow, 4).Value = "Phone Number";

                var teachers = _context.Teachers.ToList();
                foreach (var teacher in teachers)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = teacher.teacher_id;
                    worksheet.Cell(currentRow, 2).Value = teacher.teacher_name;
                    worksheet.Cell(currentRow, 3).Value = teacher.email;
                    worksheet.Cell(currentRow, 4).Value = teacher.phone_number;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Teachers.xlsx");
                }
            }
        }
        // Adjusted ImportFromExcel method for TeachersController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid Excel file.";
                return RedirectToAction(nameof(Index));
            }

            var teachers = new List<Teacher>();

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

                    for (int row = 2; row <= rowCount; row++) // Skip the first row (header)
                    {
                        var teacherId = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        var teacherName = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                        var email = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                        var phoneNumber = worksheet.Cells[row, 4].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(teacherId) && !string.IsNullOrEmpty(teacherName))
                        {
                            var teacher = new Teacher
                            {
                                teacher_id = teacherId,
                                teacher_name = teacherName,
                                email = email,
                                phone_number = phoneNumber
                            };

                            teachers.Add(teacher);
                        }
                    }
                }
            }

            if (teachers.Any())
            {
                // Avoid duplicates
                foreach (var teacher in teachers)
                {
                    if (!_context.Teachers.Any(t => t.teacher_id == teacher.teacher_id))
                    {
                        _context.Teachers.Add(teacher);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Teachers imported successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "No valid data found in the Excel file.";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
