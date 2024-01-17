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
    public class RegistrationSessionsController : Controller
    {
        private readonly SchoolContext _context;

        public RegistrationSessionsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: RegistrationSessions
        public async Task<IActionResult> Index()
        {
              return _context.RegistrationSessions != null ? 
                          View(await _context.RegistrationSessions.ToListAsync()) :
                          Problem("Entity set 'SchoolContext.RegistrationSessions'  is null.");
        }

        // GET: RegistrationSessions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.RegistrationSessions == null)
            {
                return NotFound();
            }

            var registrationSession = await _context.RegistrationSessions
                .FirstOrDefaultAsync(m => m.session_id == id);
            if (registrationSession == null)
            {
                return NotFound();
            }

            return View(registrationSession);
        }

        // GET: RegistrationSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistrationSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("start_date,end_date,semester,academic_year,status")] RegistrationSession registrationSession)
        {
            registrationSession.session_id = $"Registration_{registrationSession.semester}_{registrationSession.start_date.Year}";
            _context.Add(registrationSession);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: RegistrationSessions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.RegistrationSessions == null)
            {
                return NotFound();
            }

            var registrationSession = await _context.RegistrationSessions.FindAsync(id);
            if (registrationSession == null)
            {
                return NotFound();
            }
            return View(registrationSession);
        }

        // POST: RegistrationSessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("session_id,start_date,end_date,semester,academic_year,status")] RegistrationSession registrationSession)
        {
            if (id != registrationSession.session_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrationSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationSessionExists(registrationSession.session_id))
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
            return View(registrationSession);
        }

        // GET: RegistrationSessions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.RegistrationSessions == null)
            {
                return NotFound();
            }

            var registrationSession = await _context.RegistrationSessions
                .FirstOrDefaultAsync(m => m.session_id == id);
            if (registrationSession == null)
            {
                return NotFound();
            }

            return View(registrationSession);
        }

        // POST: RegistrationSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.RegistrationSessions == null)
            {
                return Problem("Entity set 'SchoolContext.RegistrationSessions'  is null.");
            }
            var registrationSession = await _context.RegistrationSessions.FindAsync(id);
            if (registrationSession != null)
            {
                _context.RegistrationSessions.Remove(registrationSession);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationSessionExists(string id)
        {
          return (_context.RegistrationSessions?.Any(e => e.session_id == id)).GetValueOrDefault();
        }
    }
}
