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
    public class TuitionPaymentsController : Controller
    {
        private readonly SchoolContext _context;

        public TuitionPaymentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: TuitionPayments
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.TuitionPayments.Include(t => t.Student);
            return View(await schoolContext.ToListAsync());
        }

        // GET: TuitionPayments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TuitionPayments == null)
            {
                return NotFound();
            }

            var tuitionPayment = await _context.TuitionPayments
                .Include(t => t.Student)
                .FirstOrDefaultAsync(m => m.payment_id == id);
            if (tuitionPayment == null)
            {
                return NotFound();
            }

            return View(tuitionPayment);
        }

        // GET: TuitionPayments/Create
        public IActionResult Create()
        {
            ViewData["student_id"] = new SelectList(_context.Students, "student_id", "student_id");
            return View();
        }

        // POST: TuitionPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("payment_id,student_id,total_credits,tuition_fee,amount_to_pay,amount_paid,payment_time,excess_money,debt")] TuitionPayment tuitionPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tuitionPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["student_id"] = new SelectList(_context.Students, "student_id", "student_id", tuitionPayment.student_id);
            return View(tuitionPayment);
        }

        // GET: TuitionPayments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TuitionPayments == null)
            {
                return NotFound();
            }

            var tuitionPayment = await _context.TuitionPayments.FindAsync(id);
            if (tuitionPayment == null)
            {
                return NotFound();
            }
            ViewData["student_id"] = new SelectList(_context.Students, "student_id", "student_id", tuitionPayment.student_id);
            return View(tuitionPayment);
        }

        // POST: TuitionPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("payment_id,student_id,total_credits,tuition_fee,amount_to_pay,amount_paid,payment_time,excess_money,debt")] TuitionPayment tuitionPayment)
        {
            if (id != tuitionPayment.payment_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tuitionPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TuitionPaymentExists(tuitionPayment.payment_id))
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
            ViewData["student_id"] = new SelectList(_context.Students, "student_id", "student_id", tuitionPayment.student_id);
            return View(tuitionPayment);
        }

        // GET: TuitionPayments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TuitionPayments == null)
            {
                return NotFound();
            }

            var tuitionPayment = await _context.TuitionPayments
                .Include(t => t.Student)
                .FirstOrDefaultAsync(m => m.payment_id == id);
            if (tuitionPayment == null)
            {
                return NotFound();
            }

            return View(tuitionPayment);
        }

        // POST: TuitionPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TuitionPayments == null)
            {
                return Problem("Entity set 'SchoolContext.TuitionPayments'  is null.");
            }
            var tuitionPayment = await _context.TuitionPayments.FindAsync(id);
            if (tuitionPayment != null)
            {
                _context.TuitionPayments.Remove(tuitionPayment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TuitionPaymentExists(string id)
        {
          return (_context.TuitionPayments?.Any(e => e.payment_id == id)).GetValueOrDefault();
        }
    }
}
