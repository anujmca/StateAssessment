using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StateAssessment.Data;
using StateAssessment.Models;
using StateAssessment.Models.ViewModels;

namespace StateAssessment.Controllers
{
    [Route("inventory")]
    public class InventoryController : Controller
    {
        private readonly SAContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InventoryController(SAContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        // GET: Inventory
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var inventoryDbContext = _context.Inventories.Include(i => i.ParentInventory).Include(i => i.Questions);
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
            var user = _userManager.Users.First();

            var inventory = await _context.Inventories
                .Include(i => i.ParentInventory)
                .Include(q => q.Questions)
                    .ThenInclude(q=>q.QuestionSuggestedAnswers)
                .Include(q=>q.Questions)
                    .ThenInclude(q=>q.AssessmentAnswers.Where(a=>a.AssesseeUserId.Equals(user.Id)))
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }
                        
            //var assessment = _context.Assessments.FirstOrDefault(a => a.User.Id.Equals(user.Id) && a.InventoryId.Equals(inventory.InventoryId));
            var assessment = await _context.Assessments
                            .Include(q => q.AssessmentAnswers)
                            .FirstOrDefaultAsync(a => a.User.Id.Equals(user.Id) && a.InventoryId.Equals(inventory.InventoryId));

            if (assessment == null)
            {
                assessment = new Assessment();
                assessment.StartedOn = DateTime.Now.ToUniversalTime();
                assessment.AssesseeUserId = user.Id;   
                assessment.InventoryId = inventory.InventoryId;

                _context.Add(assessment);
                var assessmentId = await _context.SaveChangesAsync();
            }

           var inventoryAssessment = new Models.ViewModels.InventoryAssessment(inventory, assessment);

            return View(inventoryAssessment);
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

        public record AddAssessmentAnswerRequest(int AssessmentId, int QuestionId, string Answer);

        [HttpPost("CaptureAnswer")]
        public async Task<JsonResult> CaptureAnswer([FromBody] AddAssessmentAnswerRequest req)
        {
            try
            {
                var user = _userManager.Users.First();

                var question = _context.Questions.FirstOrDefault(q => q.QuestionId.Equals(req.QuestionId));
                if (question != null)
                {
                    var ans = _context.AssessmentAnswers.FirstOrDefault(a => a.AssessmentId.Equals(req.AssessmentId) && a.QuestionId.Equals(req.QuestionId) && a.AssesseeUserId.Equals(user.Id));
                    var isNew = false;
                    if (ans == null)
                    {
                        ans = new AssessmentAnswer();
                        ans.AssessmentId = req.AssessmentId;
                        ans.QuestionId = req.QuestionId;
                        ans.AssesseeUserId = user.Id;
                        isNew = true;
                    }
                    ans.SubmittedOn = DateTime.Now.ToUniversalTime();
                    switch (question.QuestionTypeCode)
                    {
                        case Common.Constants.QUESTION_TYPE__YesNoUnknown:
                        case Common.Constants.QUESTION_TYPE__Numeric:
                        case Common.Constants.QUESTION_TYPE__Text:
                            ans.AnswerValue = req.Answer;
                            break;
                        case Common.Constants.QUESTION_TYPE__SingleChoice: 
                        case Common.Constants.QUESTION_TYPE__MultipleChoice:
                            var selectedSuggestedAnswerId = long.Parse(req.Answer);
                            var suggestedAnswer = _context.QuestionSuggestedAnswers.FirstOrDefault(s => s.QuestionSuggestedAnswerId.Equals(selectedSuggestedAnswerId));
                            if (suggestedAnswer != null)
                                ans.SuggestedAnswerId = suggestedAnswer.QuestionSuggestedAnswerId;
                            break;
                    }
                    if (isNew)
                        _context.Add(ans);
                    else
                        _context.Update(ans);
                    var assessmentAnswerId = await _context.SaveChangesAsync();
                    //ans.AssessmentId = assessmentAnswerId;

                    return Json(new
                    {
                        assessmentAnswerId = assessmentAnswerId,
                        questionId = req.QuestionId
                    });
                }
                else
                    return Json("Invalid Question Id");
            }
            catch (Exception ex) {
                string abc = ex.ToString();
            }
            return Json("aa");
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
