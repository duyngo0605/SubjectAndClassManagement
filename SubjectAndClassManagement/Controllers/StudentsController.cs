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
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("student"))
            {
                await Details(User.FindFirstValue("StudentId"));
                return View("Details");
            }
            else
            {
                return _context.Students != null ?
                          View(await _context.Students.ToListAsync()) :
                          Problem("Entity set 'SchoolContext.Students'  is null.");
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
        public async Task<IActionResult> Create([Bind("student_id,student_name,email,phone_number")] Student student)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(string id, [Bind("student_id,student_name,email,phone_number, User")] Student student)
        {
            try
            {
                var profile = student.User.Profile;
                _context.Update(student);
                _context.Update(profile);
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
