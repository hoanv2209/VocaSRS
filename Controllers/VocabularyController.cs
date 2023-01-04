using Microsoft.AspNetCore.Mvc;
using VocaSRS.Models;
using VocaSRS.Services;

namespace VocaSRS.Controllers
{
    public class VocabularyController : Controller
    {
        private readonly IVocaService _service;
        public VocabularyController(IVocaService vocaService)
        {
            _service = vocaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(VocabularyRequestModel model)
        {
            _service.AddVocabulary(model);
            return RedirectToAction("Index");
        }
    }
}
