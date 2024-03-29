﻿using System;
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

            if (User.IsInRole("teacher"))
            {
                var allClasses = _context.Classes
                    .Include(s => s.Room)
                    .Include(s => s.Subject)
                    .Include(s => s.Teacher)
                    .Where(s=> s.teacher_id== User.FindFirstValue("username"));
                return View(await allClasses.ToListAsync());
            }
            else
            {
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

            // Nếu không có phiên nào đang mở, tiếp tục hiển thị trang tạo mới
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
        public async Task<IActionResult> Create([Bind("class_id,subject_id,room_id,teacher_id,max_number_of_members, number_of_members,days_of_week,first_period,class_period,start_date,end_date,semester, academic_year")] Class sclass)
        {
            if (IsClassConflict(sclass))
            {
                ModelState.AddModelError(string.Empty, "Class time conflicts with an existing class in the same room.");
                ViewData["room_id"] = new SelectList(_context.Rooms, "room_id", "room_id");
                ViewData["subject_id"] = new SelectList(_context.Subjects, "subject_id", "subject_id");
                ViewData["teacher_id"] = new SelectList(_context.Teachers, "teacher_id", "teacher_id");
                return View(sclass);
            }

            if (IsTeacherScheduleConflict(sclass))
            {
                ModelState.AddModelError(string.Empty, "Teacher schedule conflicts with an existing class.");
                ViewData["room_id"] = new SelectList(_context.Rooms, "room_id", "room_id", sclass.room_id);
                ViewData["subject_id"] = new SelectList(_context.Subjects, "subject_id", "subject_id", sclass.subject_id);
                ViewData["teacher_id"] = new SelectList(_context.Teachers, "teacher_id", "teacher_id", sclass.teacher_id);
                return View(sclass);
            }

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
        public async Task<IActionResult> Edit(string id, [Bind("class_id,subject_id,room_id,teacher_id,max_number_of_members,number_of_members,days_of_week,first_period,class_period,start_date,end_date,semester, academic_year")] Class sclass)
        {
            if (id != sclass.class_id)
            {
                return NotFound();
            }

            if (IsClassConflict(sclass))
            {
                ModelState.AddModelError(string.Empty, "Class time conflicts with an existing class in the same room.");
                ViewData["room_id"] = new SelectList(_context.Rooms, "room_id", "room_id", sclass.room_id);
                ViewData["subject_id"] = new SelectList(_context.Subjects, "subject_id", "subject_id", sclass.subject_id);
                ViewData["teacher_id"] = new SelectList(_context.Teachers, "teacher_id", "teacher_id", sclass.teacher_id);
                return View(sclass);
            }

            if (IsTeacherScheduleConflict(sclass))
            {
                ModelState.AddModelError(string.Empty, "Teacher schedule conflicts with an existing class.");
                ViewData["room_id"] = new SelectList(_context.Rooms, "room_id", "room_id", sclass.room_id);
                ViewData["subject_id"] = new SelectList(_context.Subjects, "subject_id", "subject_id", sclass.subject_id);
                ViewData["teacher_id"] = new SelectList(_context.Teachers, "teacher_id", "teacher_id", sclass.teacher_id);
                return View(sclass);
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

        public async Task<IActionResult> Register()
        {
            var studentId = User.FindFirstValue("username");

            // Get the currently open registration session
            var openSession = await _context.RegistrationSessions
                .FirstOrDefaultAsync(s => s.status == "open");

            if (openSession == null)
            {
                TempData["NoOpenSession"] = "There's no available registration session!";
                return View();
            }
            else if (openSession.start_date>DateTime.Now)
            {
                TempData["NoOpenSession"] = $"The Registration date has not arrived yet. Please come back at {openSession.start_date} ";
                return View();
            }    
          

            // Lấy danh sách các lớp học thuộc phiên đăng ký học phần đang được mở
            var registeredClasses = _context.Classes
                .Include(s => s.Room)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Where(c => _context.StudentRegistrations.Any(sr => sr.class_id == c.class_id && sr.student_id == studentId) &&
                            c.semester == openSession.semester &&
                            c.academic_year == openSession.academic_year)
                .ToList();

            // Lấy danh sách các lớp học chưa đăng ký
            var unregisteredClasses = _context.Classes
                .Include(s => s.Room)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Where(c => !_context.StudentRegistrations.Any(sr => sr.class_id == c.class_id && sr.student_id == studentId) &&
                            c.semester == openSession.semester &&
                            c.academic_year == openSession.academic_year)
                .ToList();

            ViewBag.RegisteredClasses = registeredClasses;
            ViewBag.UnregisteredClasses = unregisteredClasses;

            return View();
        }

        // GET: Classes/Students/5
        public async Task<IActionResult> DisplayStudents(string id)
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

            // Lấy danh sách sinh viên của lớp học
            var students = await _context.StudentRegistrations
                .Include(sr => sr.Student)
                .Where(sr => sr.class_id == id)
                .Select(sr => sr.Student)
                .ToListAsync();

            ViewData["ClassName"] = $"{sclass.Subject.subject_name} - {sclass.Teacher.teacher_name} - {sclass.class_id}";
            TempData["ClassId"] = sclass.class_id;
            TempData.Keep("ClassId");
            return View(students);
            return View(students);
        }



        public IActionResult RegisteredClasses()
        {
            var registeredClasses = ViewBag.RegisteredClasses as List<Class>;
            return View(registeredClasses);
        }

        public IActionResult UnregisteredClasses()
        {
            var unregisteredClasses = ViewBag.UnregisteredClasses as List<Class>;
            return View(unregisteredClasses);
        }



        private bool ClassExists(string id)
        {
            return (_context.Classes?.Any(e => e.class_id == id)).GetValueOrDefault();
        }

        private bool IsClassConflict(Class newClass)
        {
            var existingClasses = _context.Classes
                .Where(c =>
                    (c.room_id == newClass.room_id) &&
                    ((c.start_date <= newClass.start_date && c.end_date >= newClass.start_date) ||
                     (c.start_date <= newClass.end_date && c.end_date >= newClass.end_date) ||
                     (c.start_date >= newClass.start_date && c.end_date <= newClass.end_date)))
                .ToList();

            foreach (var existingClass in existingClasses)
            {
                if (existingClass.class_id != newClass.class_id) // Đảm bảo không so sánh với chính nó
                {
                    // Kiểm tra xem có mâu thuẫn về thời gian hay không
                    if (existingClass.days_of_week == newClass.days_of_week)
                    {
                        // Nếu days_of_week giống nhau, kiểm tra xem tiết học có giao nhau hay không
                        if (newClass.first_period < existingClass.first_period + existingClass.class_period &&
                            existingClass.first_period < newClass.first_period + newClass.class_period)
                        {
                            return true; // Có mâu thuẫn về thời gian và phòng học
                        }
                    }
                }
            }

            return false; // Không có mâu thuẫn
        }

        private bool IsTeacherScheduleConflict(Class newClass)
        {
            var existingClasses = _context.Classes
                .Where(c =>
                    (c.teacher_id == newClass.teacher_id) &&
                    ((c.start_date <= newClass.start_date && c.end_date >= newClass.start_date) ||
                     (c.start_date <= newClass.end_date && c.end_date >= newClass.end_date) ||
                     (c.start_date >= newClass.start_date && c.end_date <= newClass.end_date)))
                .ToList();

            foreach (var existingClass in existingClasses)
            {
                if (existingClass.class_id != newClass.class_id) // Đảm bảo không so sánh với chính nó
                {
                    // Kiểm tra xem có mâu thuẫn về thời gian hay không
                    if (existingClass.days_of_week == newClass.days_of_week)
                    {
                        // Nếu days_of_week giống nhau, kiểm tra xem tiết học có giao nhau hay không
                        if (newClass.first_period < existingClass.first_period + existingClass.class_period &&
                            existingClass.first_period < newClass.first_period + newClass.class_period)
                        {
                            return true; // Có mâu thuẫn về thời gian và lịch giảng dạy của giáo viên
                        }
                    }
                }
            }

            return false; // Không có mâu thuẫn
        }
    }
}
