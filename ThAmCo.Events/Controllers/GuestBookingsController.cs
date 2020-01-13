using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Controllers
{
    public class GuestBookingsController : Controller
    {
        private readonly EventsDbContext _context;

        public GuestBookingsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: GuestBookings
        public async Task<IActionResult> Index(int? id)
        {
            var eventsDbContext = (id == null) ? _context.Guests.Include(g => g.Customer).Include(g => g.Event) : _context.Guests.Include(g => g.Customer).Include(g => g.Event).Where(e => e.EventId == id);
            return View(await eventsDbContext.OrderBy(a => a.EventId).ToListAsync());
        }

        // GET: GuestBookings/Details/5
        public async Task<IActionResult> Details(int? id, int? id2)
        {
            if (id == null || id2 == null)
            {
                return NotFound();
            }

            var guestBooking = await _context.Guests
                .Include(g => g.Customer)
                .Include(g => g.Event)
                .Where(e => e.EventId == id)
                .FirstOrDefaultAsync(m => m.CustomerId == id2);
            if (guestBooking == null)
            {
                return NotFound();
            }

            return View(guestBooking);
        }

        // GET: GuestBookings/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList((from c in _context.Customers
               select new{Id = c.Id,
                                                         FullName = c.FirstName + " " + c.Surname
                                                     }),
                "Id",
                "FullName");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title");
            return View();
        }

        // POST: GuestBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,EventId,Attended")] GuestBooking guestBooking)
        {
            if (ModelState.IsValid)
            {
                if (!GuestBookingExists(guestBooking.CustomerId, guestBooking.EventId))
                {
                    _context.Add(guestBooking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Debug.WriteLine("Guest Booking already Exists");
                }
            }
            ViewData["CustomerId"] = new SelectList((from c in _context.Customers
                 select new{ Id = c.Id, FullName = c.FirstName + " " + c.Surname }), "Id", "FullName", guestBooking.CustomerId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", guestBooking.EventId);
            return View(guestBooking);
        }

        // GET: GuestBookings/Edit/5
        public async Task<IActionResult> Edit(int? id, int? id2)
        {
            if (id == null || id2 == null)
            {
                return NotFound();
            }

            var guestBooking = await _context.Guests.FindAsync(id2, id);
            if (guestBooking == null)
            {
                return NotFound();
            }
            return View(guestBooking);
        }

        // POST: GuestBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int id2, [Bind("CustomerId,EventId,Attended")] GuestBooking guestBooking)
        {
            if (id2 != guestBooking.CustomerId || id != guestBooking.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guestBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestBookingExists(guestBooking.CustomerId, guestBooking.EventId))
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
            ViewData["CustomerId"] = new SelectList((from c in _context.Customers
            select new { Id = c.Id, FullName = c.FirstName + " " + c.Surname }), "Id", "FullName", guestBooking.CustomerId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Title", guestBooking.EventId);
            return View(guestBooking);
        }

        // GET: GuestBookings/Delete/5
        public async Task<IActionResult> Delete(int? id, int? id2)
        {
            if (id == null || id2 == null)
            {
                return NotFound();
            }

            var guestBooking = await _context.Guests
                .Include(g => g.Customer)
                .Include(g => g.Event)
                .Where(e => e.EventId == id)
                .FirstOrDefaultAsync(m => m.CustomerId == id2);
            if (guestBooking == null)
            {
                return NotFound();
            }

            return View(guestBooking);
        }

        // POST: GuestBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int id2)
        {
            var guestBooking = await _context.Guests.FindAsync(id2, id);
            _context.Guests.Remove(guestBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuestBookingExists(int custId, int eventId)
        {
            foreach (GuestBooking booking in _context.Guests)
            {
                if (booking.CustomerId == custId && booking.EventId == eventId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
