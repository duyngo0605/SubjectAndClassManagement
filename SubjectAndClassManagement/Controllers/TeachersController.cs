using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        // Action for import from Excel
        [HttpPost]
        public IActionResult ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "The file was not uploaded.");
                return View("Index"); // Return to the Index view if there's an error
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheets.First();
                    var rowCount = worksheet.RowCount();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var teacherId = worksheet.Cell(row, 1).Value.ToString().Trim();
                        var existingTeacher = _context.Teachers.Find(teacherId);

                        if (existingTeacher == null)
                        {
                            // Teacher does not exist, add a new one
                            var newTeacher = new Teacher
                            {
                                teacher_id = teacherId,
                                teacher_name = worksheet.Cell(row, 2).Value.ToString().Trim(),
                                email = worksheet.Cell(row, 3).Value.ToString().Trim(),
                                phone_number = worksheet.Cell(row, 4).Value.ToString().Trim()
                            };
                            _context.Teachers.Add(newTeacher);
                        }
                        else
                        {
                            // Teacher already exists, update the existing one
                            existingTeacher.teacher_name = worksheet.Cell(row, 2).Value.ToString().Trim();
                            existingTeacher.email = worksheet.Cell(row, 3).Value.ToString().Trim();
                            existingTeacher.phone_number = worksheet.Cell(row, 4).Value.ToString().Trim();
                            // No need to set the state to Modified; EF tracks changes automatically
                        }
                    }
                    _context.SaveChanges();
                }
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
