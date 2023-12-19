using System;
using System.Collections.Generic;
using System.Linq;
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

        private bool StudentRegistrationExists(string id)
        {
          return (_context.StudentRegistrations?.Any(e => e.registration_id == id)).GetValueOrDefault();
        }
    }
}
