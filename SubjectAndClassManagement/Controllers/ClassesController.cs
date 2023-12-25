using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using SubjectAndClassManagement.Models;

namespace SubjectAndClassManagement.Controllers
{
    public class ClassesController : Controller
    {
        private readonly SchoolContext _context;

        public ClassesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("student"))
            {
                // Assuming studentId is stored as a claim
                string studentId = User.FindFirstValue("StudentId");

                var studentClasses = _context.Classes
                    .Where(c => c.StudentRegistrations.Any(sr => sr.student_id == studentId))
                    .Include(s => s.Room)
                    .Include(s => s.Subject)
                    .Include(s => s.Teacher);

                return View(await studentClasses.ToListAsync());
            }
            else if (User.IsInRole("teacher"))
            {
                // Assuming teacherId is stored as a claim
                string teacherId = User.FindFirstValue("TeacherId");

                var teacherClasses = _context.Classes
                    .Where(c => c.teacher_id == teacherId)
                    .Include(s => s.Room)
                    .Include(s => s.Subject)
                    .Include(s => s.Teacher);

                return View(await teacherClasses.ToListAsync());
            }
            else
            {
                // If the user is not in the student or teacher role, show all classes
                var allClasses = _context.Classes
                    .Include(s => s.Room)
                    .Include(s => s.Subject)
                    .Include(s => s.Teacher);

                return View(await allClasses.ToListAsync());
            }
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(string id)
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

            return View(sclass);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            ViewData["room_id"] = new SelectList(_context.Rooms, "room_id", "room_id");
            ViewData["subject_id"] = new SelectList(_context.Subjects, "subject_id", "subject_id");
            ViewData["teacher_id"] = new SelectList(_context.Teachers, "teacher_id", "teacher_id");
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("class_id,subject_id,room_id,teacher_id,number_of_members,days_of_week,class_period,start_date,end_date,year")] Class sclass)
        {
            _context.Add(sclass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var sclass = await _context.Classes.FindAsync(id);
            if (sclass == null)
            {
                return NotFound();
            }
            ViewData["room_id"] = new SelectList(_context.Rooms, "room_id", "room_id", sclass.room_id);
            ViewData["subject_id"] = new SelectList(_context.Subjects, "subject_id", "subject_id", sclass.subject_id);
            ViewData["teacher_id"] = new SelectList(_context.Teachers, "teacher_id", "teacher_id", sclass.teacher_id);
            return View(sclass);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("class_id,subject_id,room_id,teacher_id,number_of_members,days_of_week,class_period,start_date,end_date,year")] Class sclass)
        {
            if (id != sclass.class_id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(sclass);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(sclass.class_id))
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

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(string id)
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

            return View(sclass);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'SchoolContext.Classes'  is null.");
            }
            var sclass = await _context.Classes.FindAsync(id);
            if (sclass != null)
            {
                _context.Classes.Remove(sclass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(string id)
        {
            return (_context.Classes?.Any(e => e.class_id == id)).GetValueOrDefault();
        }
    }
}
