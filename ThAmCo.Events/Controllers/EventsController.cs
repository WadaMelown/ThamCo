using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;
using static ThAmCo.Events.Models.EventsViewModel;

namespace ThAmCo.Events.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var eventTypeInfo = new List<EventDto>().AsEnumerable();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:22263/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            HttpResponseMessage response = await client.GetAsync("api/eventtypes");

            var events = await _context.Events
                .Include(g => g.Bookings)
                .Include(sb => sb.StaffBookings)
                .Select(g => new Models.EventsViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Date = g.Date,
                    Duration = g.Duration,
                    TypeId = g.TypeId,
                    Bookings = g.Bookings,
                    TypeValue = eventTypeInfo.Where(h => h.id == g.TypeId).Select(n => n.title).FirstOrDefault(),
                    VenueCode = g.VenueCode,
                    StaffBookings = g.StaffBookings
                }).ToListAsync();

            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            var eventTypeInfo = new List<EventDto>().AsEnumerable();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:22263/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            HttpResponseMessage response = await client.GetAsync("api/eventtypes");

           // if (response.IsSuccessStatusCode)
            //{
               // eventTypeInfo = await response.Content.ReadAsAsync<IEnumerable<EventDto>>();
           // }
          // else
            //{
           //     throw new Exception();
           // }

           // ViewData["TypeId"] = new SelectList(eventTypeInfo.ToList(), "id", "title", eventTypeInfo.Select(h => h.id == "CON"));
            //ViewData["StaffId"] = new SelectList((from s in _context.Staff
                                                  //select new { Id = s.Id, FullName = s.FirstName + " " + s.Surname }), "Id","FullName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Date,Duration,TypeId,VenueCode,StaffId")] CreateEventsViewModel @event)
        {
            if (ModelState.IsValid)
            {
                Models.EventsViewModel tempEvent = new Models.EventsViewModel
                {
                    Id = @event.Id,
                    Title = @event.Title,
                    Date = @event.Date,
                    Duration = @event.Duration,
                    TypeId = @event.TypeId,
                    VenueCode = @event.VenueCode
                };

                Reservations resdto = new Reservations
                {
                    EventDate = @event.Date.Date,
                    VenueCode = @event.VenueCode,
                    StaffId = @event.StaffId.ToString()
                };

                _context.Add(tempEvent);
                await _context.SaveChangesAsync();

                HttpClient client = new HttpClient();
                var response = client.PostAsync("http://localhost:22263/api/Reservations", new StringContent(JsonConvert.SerializeObject(resdto), Encoding.UTF8, "application/json"));

                Debug.WriteLine(await response.Result.Content.ReadAsStringAsync());

                return RedirectToAction(nameof(Index));
            }

            return View(@event);
        }
        public async Task<IEnumerable<Venuedto>> getVenues([FromQuery, MinLength(3), MaxLength(3), Required] string eventCode, [FromQuery, Required] string datePassed)
        {
            var eventTypeInfo = new List<Venuedto>().AsEnumerable();

            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:22263/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            HttpResponseMessage response = await client.GetAsync("api/Availability?eventType=" + eventCode + "&beginDate=" + datePassed + "&endDate=" + datePassed);

            if (response.IsSuccessStatusCode)
            {
                eventTypeInfo = await response.Content.ReadAsAsync<IEnumerable<Venuedto>>();
            }
            else
            {
                throw new Exception();
            }

            return eventTypeInfo;
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Date,Duration,TypeId")] Models.EventsViewModel @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
        
    }
}
