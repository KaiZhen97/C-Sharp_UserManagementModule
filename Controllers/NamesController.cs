using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Name_List.Data;
using Name_List.Models;

namespace Name_List.Controllers
{
    public class NamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Names
        public async Task<IActionResult> Index()
        {
            return View(await _context.Name.ToListAsync());
        }

        // GET: Jokes/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Name.Where(c => c.UserName.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: Names/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var name = await _context.Name
                .FirstOrDefaultAsync(m => m.Id == id);
            if (name == null)
            {
                return NotFound();
            }

            return View(name);
        }

        // GET: Names/Create
        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Names/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,PhoneNo")] Name name)
        {
            if (ModelState.IsValid)
            {
                _context.Add(name);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(name);
        }

        // GET: Names/Edit/5
        //[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var name = await _context.Name.FindAsync(id);
            if (name == null)
            {
                return NotFound();
            }
            return View(name);
        }

        // POST: Names/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,PhoneNo")] Name name)
        {
            if (id != name.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(name);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NameExists(name.Id))
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
            return View(name);
        }

        // GET: Names/Delete/5
        //[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var name = await _context.Name
                .FirstOrDefaultAsync(m => m.Id == id);
            if (name == null)
            {
                return NotFound();
            }

            return View(name);
        }

        // POST: Names/Delete/5
        //[Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var name = await _context.Name.FindAsync(id);
            _context.Name.Remove(name);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NameExists(int id)
        {
            return _context.Name.Any(e => e.Id == id);
        }
    }
}
