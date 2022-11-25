using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StateAssessment.Data;
using StateAssessment.Models;

namespace StateAssessment.Controllers
{
    public class QuestionTypeController : Controller
    {
        private readonly SAContext _context;

        public QuestionTypeController(SAContext context)
        {
            _context = context;
        }

        // GET: QuestionType
        public async Task<IActionResult> Index()
        {
              return View(await _context.QuestionTypes.ToListAsync());
        }

        // GET: QuestionType/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.QuestionTypes == null)
            {
                return NotFound();
            }

            var questionType = await _context.QuestionTypes
                .FirstOrDefaultAsync(m => m.QuestionTypeCode == id);
            if (questionType == null)
            {
                return NotFound();
            }

            return View(questionType);
        }

        // GET: QuestionType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuestionType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionTypeCode,QuestionTypeName")] QuestionType questionType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionType);
        }

        // GET: QuestionType/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.QuestionTypes == null)
            {
                return NotFound();
            }

            var questionType = await _context.QuestionTypes.FindAsync(id);
            if (questionType == null)
            {
                return NotFound();
            }
            return View(questionType);
        }

        // POST: QuestionType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("QuestionTypeCode,QuestionTypeName")] QuestionType questionType)
        {
            if (id != questionType.QuestionTypeCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionTypeExists(questionType.QuestionTypeCode))
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
            return View(questionType);
        }

        // GET: QuestionType/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.QuestionTypes == null)
            {
                return NotFound();
            }

            var questionType = await _context.QuestionTypes
                .FirstOrDefaultAsync(m => m.QuestionTypeCode == id);
            if (questionType == null)
            {
                return NotFound();
            }

            return View(questionType);
        }

        // POST: QuestionType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.QuestionTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.QuestionType'  is null.");
            }
            var questionType = await _context.QuestionTypes.FindAsync(id);
            if (questionType != null)
            {
                _context.QuestionTypes.Remove(questionType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionTypeExists(string id)
        {
          return _context.QuestionTypes.Any(e => e.QuestionTypeCode == id);
        }
    }
}
