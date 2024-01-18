using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubjectAndClassManagement.Models;

using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.IO; 

namespace SubjectAndClassManagement.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SchoolContext _context;

        public SubjectsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("teacher"))
            {
                await Details(User.FindFirstValue("TeacherId"));
                return View("Details");
            }
            return _context.Subjects != null ? 
                          View(await _context.Subjects.ToListAsync()) :
                          Problem("Entity set 'SchoolContext.Subjects'  is null.");
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.subject_id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("subject_id,subject_name,subject_description,credit")] Subject subject)
        {
            _context.Add(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("subject_id,subject_name,subject_description,credit")] Subject subject)
        {
            try
            {
                _context.Update(subject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(subject.subject_id))
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

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.subject_id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Subjects == null)
            {
                return Problem("Entity set 'SchoolContext.Subjects'  is null.");
            }
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(string id)
        {
          return (_context.Subjects?.Any(e => e.subject_id == id)).GetValueOrDefault();
        }
        public IActionResult ExportToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Subjects");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Subject ID";
                worksheet.Cell(currentRow, 2).Value = "Subject Name";
                worksheet.Cell(currentRow, 3).Value = "Subject Description";
                worksheet.Cell(currentRow, 4).Value = "Credit";

                var subjects = _context.Subjects.ToList();
                foreach (var subject in subjects)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = subject.subject_id;
                    worksheet.Cell(currentRow, 2).Value = subject.subject_name;
                    worksheet.Cell(currentRow, 3).Value = subject.subject_description;
                    worksheet.Cell(currentRow, 4).Value = subject.credit;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Subjects.xlsx");
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
                        var subjectId = worksheet.Cell(row, 1).Value.ToString().Trim();
                        var subjectName = worksheet.Cell(row, 2).Value.ToString().Trim();
                        var subjectDescription = worksheet.Cell(row, 3).Value.ToString().Trim();
                        var creditString = worksheet.Cell(row, 4).Value.ToString().Trim();

                        if (!int.TryParse(creditString, out int credit))
                        {
                            // Handle when can't parse
                            continue;
                        }

                        var subject = _context.Subjects.FirstOrDefault(s => s.subject_id == subjectId);
                        if (subject == null)
                        {
                            // Subject does not exist, add a new one
                            subject = new Subject
                            {
                                subject_id = subjectId,
                                subject_name = subjectName,
                                subject_description = subjectDescription,
                                credit = credit
                            };
                            _context.Subjects.Add(subject);
                        }
                        else
                        {
                            // Subject already exists, update the existing one
                            subject.subject_name = subjectName;
                            subject.subject_description = subjectDescription;
                            subject.credit = credit;
                            _context.Subjects.Update(subject);
                        }
                    }
                    _context.SaveChanges();
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
