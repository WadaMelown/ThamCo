using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class StaffBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: StaffBookings
        public async Task<IActionResult> Index()
        {
            var eventsDbContext = _context.StaffBooking.Include(s => s.Event).Include(s => s.StaffInfo);
            return View(await eventsDbContext.ToListAsync());
        }

        // GET: StaffBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffBooking = await _context.StaffBooking
                .Include(s => s.Event)
                .Include(s => s.StaffInfo)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staffBooking == null)
            {
                return NotFound();
            }

            return View(staffBooking);
        }

        // GET: StaffBookings/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList((from s in _context.Staff
                select new
                {
                  Id = s.Id,
                  FullName = s.FirstName + " " + s.Surname
                }),
                "Id",
                    "FullName");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title");
            return View();
        }

        // POST: StaffBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,EventId")] StaffBooking staffBooking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staffBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", staffBooking.EventId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email", staffBooking.StaffId);
            return View(staffBooking);
        }

        // GET: StaffBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffBooking = await _context.StaffBooking.FindAsync(id);
            if (staffBooking == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", staffBooking.EventId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email", staffBooking.StaffId);
            return View(staffBooking);
        }

        // POST: StaffBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffId,EventId")] StaffBooking staffBooking)
        {
            if (id != staffBooking.StaffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffBookingExists(staffBooking.StaffId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", staffBooking.EventId);
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Email", staffBooking.StaffId);
            return View(staffBooking);
        }

        // GET: StaffBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffBooking = await _context.StaffBooking
                .Include(s => s.Event)
                .Include(s => s.StaffInfo)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staffBooking == null)
            {
                return NotFound();
            }

            return View(staffBooking);
        }

        // POST: StaffBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staffBooking = await _context.StaffBooking.FindAsync(id);
            _context.StaffBooking.Remove(staffBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffBookingExists(int id)
        {
            return _context.StaffBooking.Any(e => e.StaffId == id);
        }
    }
}
