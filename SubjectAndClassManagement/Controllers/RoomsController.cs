using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SubjectAndClassManagement.Models;

namespace SubjectAndClassManagement.Controllers
{
    public class RoomsController : Controller
    {
        private readonly SchoolContext _context;

        public RoomsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
              return _context.Rooms != null ? 
                          View(await _context.Rooms.ToListAsync()) :
                          Problem("Entity set 'SchoolContext.Rooms'  is null.");
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.room_id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("room_id,room_name,room_capacity,building_name")] Room room)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("room_id,room_name,room_capacity,building_name")] Room room)
        {
            try
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.room_id))
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

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.room_id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'SchoolContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(string id)
        {
          return (_context.Rooms?.Any(e => e.room_id == id)).GetValueOrDefault();
        }

        public IActionResult ExportToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Rooms");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Room ID";
                worksheet.Cell(currentRow, 2).Value = "Room Capacity";
                worksheet.Cell(currentRow, 3).Value = "Building Name";

                var rooms = _context.Rooms.ToList();
                foreach (var room in rooms)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = room.room_id;
                    worksheet.Cell(currentRow, 2).Value = room.room_capacity;
                    worksheet.Cell(currentRow, 3).Value = room.building_name;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Rooms.xlsx");
                }
            }
        }

        // Action for importing rooms from Excel
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
                        var roomId = worksheet.Cell(row, 1).Value.ToString().Trim();
                        var roomCapacityString = worksheet.Cell(row, 2).Value.ToString().Trim();
                        var buildingName = worksheet.Cell(row, 3).Value.ToString().Trim();

                        if (!int.TryParse(roomCapacityString, out int roomCapacity))
                        {
                            // Handle the case where room capacity is not a valid integer
                            continue; // Skip this row
                        }

                        var room = _context.Rooms.FirstOrDefault(r => r.room_id == roomId);
                        if (room == null)
                        {
                            // Room does not exist, add a new one
                            room = new Room
                            {
                                room_id = roomId,
                                room_capacity = roomCapacity,
                                building_name = buildingName
                            };
                            _context.Rooms.Add(room);
                        }
                        else
                        {
                            // Room already exists, update the existing one
                            room.room_capacity = roomCapacity;
                            room.building_name = buildingName;
                            _context.Rooms.Update(room);
                        }
                    }
                    _context.SaveChanges();
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
