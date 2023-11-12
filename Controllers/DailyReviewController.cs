using Microsoft.AspNetCore.Mvc;
using VocaSRS.Models;
using VocaSRS.Services;

namespace VocaSRS.Controllers
{
    public class DailyReviewController : Controller
    {
        private readonly IVocaService _service;

        public DailyReviewController(IVocaService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var lesson = _service.GetDailyReviewLesson();
            if (lesson == null)
            {
                return RedirectToAction("Add");
            }

            return View(lesson);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(DailyReviewRequestModel model)
        {
            _service.AddDailyReview(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Review([FromForm] int id)
        {
            _service.ReviewLesson(id);
            return RedirectToAction("Index");
        }
    }
}
