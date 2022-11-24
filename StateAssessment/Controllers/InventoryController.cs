using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StateAssessment.Data;
using StateAssessment.Models;

namespace StateAssessment.Controllers
{
    [Route("inventory")]
    public class InventoryController : Controller
    {
        private readonly InventoryDbContext _context;

        public InventoryController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Inventory
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var inventoryDbContext = _context.Inventories.Include(i => i.ParentInventory);
            return View(await inventoryDbContext.ToListAsync());
        }

        // GET: Inventory/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.ParentInventory)
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }


        // GET: Inventory/5/Questions
        [Authorize]
        [HttpGet("{id}/questions")]
        public async Task<IActionResult> Questions(long? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.ParentInventory)
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventory/Create
        [Authorize]
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["ParentInventoryId"] = new SelectList(_context.Inventories, "InventoryId", "InventoryId");
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("InventoryId,SectionName,InventoryName,InventoryDescription,TimeRequiredInMinutes,ParentInventoryId")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentInventoryId"] = new SelectList(_context.Inventories, "InventoryId", "InventoryId", inventory.ParentInventoryId);
            return View(inventory);
        }

        // GET: Inventory/5/Edit
        [Authorize]
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["ParentInventoryId"] = new SelectList(_context.Inventories, "InventoryId", "InventoryId", inventory.ParentInventoryId);
            return View(inventory);
        }

        // POST: Inventory/5/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost("{id}/edit")]
        public async Task<IActionResult> Edit(long id, [Bind("InventoryId,SectionName,InventoryName,InventoryDescription,TimeRequiredInMinutes,ParentInventoryId")] Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.InventoryId))
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
            ViewData["ParentInventoryId"] = new SelectList(_context.Inventories, "InventoryId", "InventoryId", inventory.ParentInventoryId);
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Inventories == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .Include(i => i.ParentInventory)
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Inventories == null)
            {
                return Problem("Entity set 'InventoryDbContext.Inventories'  is null.");
            }
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(long id)
        {
          return _context.Inventories.Any(e => e.InventoryId == id);
        }
    }
}
