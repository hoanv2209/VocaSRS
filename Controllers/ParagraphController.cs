using Microsoft.AspNetCore.Mvc;
using VocaSRS.Models;
using VocaSRS.Services;

namespace VocaSRS.Controllers
{
    public class ParagraphController : Controller
    {
        private readonly IVocaService _service;

        public ParagraphController(IVocaService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ParagraphRequestModel model)
        {
            _service.AddParagraph(model);
            return RedirectToAction("Index");
        }

        public IActionResult Practice()
        {
            var paragraph = _service.GetRandomParagraph();
            if (paragraph == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Paragraph = paragraph;
            return View();
        }

        [HttpPost]
        public IActionResult Practice([FromForm] int id)
        {
            _service.PracticeParagraph(id);
            return RedirectToAction("Practice");
        }
    }
}
