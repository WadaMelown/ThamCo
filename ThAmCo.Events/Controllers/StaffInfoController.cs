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
    public class StaffInfoController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffInfoController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: StaffInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staff.ToListAsync());
        }

        // GET: StaffInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffInfo = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffInfo == null)
            {
                return NotFound();
            }

            return View(staffInfo);
        }

        // GET: StaffInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StaffInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,Surname,Email")] StaffInfo staffInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staffInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staffInfo);
        }

        // GET: StaffInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffInfo = await _context.Staff.FindAsync(id);
            if (staffInfo == null)
            {
                return NotFound();
            }
            return View(staffInfo);
        }

        // POST: StaffInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,Surname,Email")] StaffInfo staffInfo)
        {
            if (id != staffInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffInfoExists(staffInfo.Id))
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
            return View(staffInfo);
        }

        // GET: StaffInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffInfo = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staffInfo == null)
            {
                return NotFound();
            }

            return View(staffInfo);
        }

        // POST: StaffInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staffInfo = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staffInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffInfoExists(int id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }
    }
}
