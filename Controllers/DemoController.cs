using Admin_Management.Data;
using Admin_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin_Management.Controllers
{
    public class DemoController : Controller
    {
        private readonly Admin_dbContext _context;

        public DemoController(Admin_dbContext context)
        {
            _context = context;
        }

        // GET: Demo
        public async Task<IActionResult> Index()
        {
            return View(await _context.TblPage.ToListAsync());
        }

        // GET: Demo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblPage == null)
            {
                return NotFound();
            }

            var tblPage = await _context.TblPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblPage == null)
            {
                return NotFound();
            }

            return View(tblPage);
        }

        // GET: Demo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Demo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Url,Title,About,PageContent,RegDate,IsActive")] TblPage tblPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblPage);
        }

        // GET: Demo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblPage == null)
            {
                return NotFound();
            }

            var tblPage = await _context.TblPage.FindAsync(id);
            if (tblPage == null)
            {
                return NotFound();
            }
            return View(tblPage);
        }

        // POST: Demo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Url,Title,About,PageContent,RegDate,IsActive")] TblPage tblPage)
        {
            if (id != tblPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblPageExists(tblPage.Id))
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
            return View(tblPage);
        }

        // GET: Demo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblPage == null)
            {
                return NotFound();
            }

            var tblPage = await _context.TblPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblPage == null)
            {
                return NotFound();
            }

            return View(tblPage);
        }

        // POST: Demo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblPage == null)
            {
                return Problem("Entity set 'Admin_dbContext.TblPage'  is null.");
            }
            var tblPage = await _context.TblPage.FindAsync(id);
            if (tblPage != null)
            {
                _context.TblPage.Remove(tblPage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblPageExists(int id)
        {
            return _context.TblPage.Any(e => e.Id == id);
        }
    }
}
