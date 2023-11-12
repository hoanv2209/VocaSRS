using Microsoft.AspNetCore.Mvc;
using VocaSRS.Services;

namespace VocaSRS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVocaService _service;

        public HomeController(IVocaService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            ViewBag.VocabularyInfo = _service.GetVocabularyDashboardInfo();
            ViewBag.ParagraphInfo = _service.GetParagraphDashboardInfo();
            ViewBag.DailyReviewInfo = _service.GetDailyReviewDashboardInfo();

            return View();
        }
    }
}