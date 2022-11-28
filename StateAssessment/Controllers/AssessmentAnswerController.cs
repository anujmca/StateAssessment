using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StateAssessment.Controllers
{
    public class AssessmentAnswerController : Controller
    {
        // GET: AssessmentAnswerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AssessmentAnswerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AssessmentAnswerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssessmentAnswerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssessmentAnswerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AssessmentAnswerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AssessmentAnswerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AssessmentAnswerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
