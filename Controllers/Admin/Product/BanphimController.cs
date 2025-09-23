using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace TechShop.Controllers
{
    public class BanphimController : Controller
    {
        private readonly AppDbContext _context;

        public BanphimController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Banphim
        public async Task<IActionResult> Index()
        {
            return View(await _context.Banphims.ToListAsync());
        }

        // GET: Banphim/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banphim = await _context.Banphims
                .Include(b => b.MaHhNavigation)
                .Include(b => b.MaPlspNavigation)
                .FirstOrDefaultAsync(m => m.MaBp == id);
            if (banphim == null)
            {
                return NotFound();
            }

            return View(banphim);
        }

        // GET: Banphim/Create
        public IActionResult Create()
        {
            ViewData["MaHh"] = new SelectList(_context.Hanghoas, "MaHh", "MaHh");
            ViewData["MaPlsp"] = new SelectList(_context.PhanloaiSps, "MaPlsp", "MaPlsp");
            return View();
        }

        // POST: Banphim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBp,TenBp,HangBp,GiaBp,LoaiBp,KieuketnoiBp,DenledBp,XuatxuBp,NgaysanxuatBp,TinhtrangBp,MaNcc,MaPlsp,MaHh")] Banphim banphim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banphim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHh"] = new SelectList(_context.Hanghoas, "MaHh", "MaHh", banphim.MaHh);
            ViewData["MaPlsp"] = new SelectList(_context.PhanloaiSps, "MaPlsp", "MaPlsp", banphim.MaPlsp);
            return View(banphim);
        }

        // GET: Banphim/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banphim = await _context.Banphims.FindAsync(id);
            if (banphim == null)
            {
                return NotFound();
            }
            ViewData["MaHh"] = new SelectList(_context.Hanghoas, "MaHh", "MaHh", banphim.MaHh);
            ViewData["MaPlsp"] = new SelectList(_context.PhanloaiSps, "MaPlsp", "MaPlsp", banphim.MaPlsp);
            return View(banphim);
        }

        // POST: Banphim/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaBp,TenBp,HangBp,GiaBp,LoaiBp,KieuketnoiBp,DenledBp,XuatxuBp,NgaysanxuatBp,TinhtrangBp,MaNcc,MaPlsp,MaHh")] Banphim banphim)
        {
            if (id != banphim.MaBp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banphim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BanphimExists(banphim.MaBp))
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
            ViewData["MaHh"] = new SelectList(_context.Hanghoas, "MaHh", "MaHh", banphim.MaHh);
            ViewData["MaPlsp"] = new SelectList(_context.PhanloaiSps, "MaPlsp", "MaPlsp", banphim.MaPlsp);
            return View(banphim);
        }

        // GET: Banphim/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banphim = await _context.Banphims
                .Include(b => b.MaHhNavigation)
                .Include(b => b.MaPlspNavigation)
                .FirstOrDefaultAsync(m => m.MaBp == id);
            if (banphim == null)
            {
                return NotFound();
            }

            return View(banphim);
        }

        // POST: Banphim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var banphim = await _context.Banphims.FindAsync(id);
            _context.Banphims.Remove(banphim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BanphimExists(string id)
        {
            return _context.Banphims.Any(e => e.MaBp == id);
        }
    }
}