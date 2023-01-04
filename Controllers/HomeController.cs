using Microsoft.AspNetCore.Mvc;
using VocaSRS.Models;
using VocaSRS.Services;

namespace VocaSRS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVocaService _service;

        public HomeController(ILogger<HomeController> logger, IVocaService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            var model = _service.GetDashboardInfo();
            return View(model);
        }

        public IActionResult Practice()
        {
            var vocabulary = _service.GetPracticeVocabulary();
            if (vocabulary == null)
            {
                return RedirectToAction("Index", "Vocabulary");
            }

            ViewBag.Vocabulary = vocabulary;
            return View();
        }

        [HttpPost]
        public IActionResult Practice(PracticeRequestModel model)
        {
            _service.CheckAnswer(model);
            return RedirectToAction("Practice");
        }
    }
}