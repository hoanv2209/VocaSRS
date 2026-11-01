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

        public IActionResult Practice()
        {
            PracticeResponseModel vocabulary = _service.GetQuiz();
            if (vocabulary == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Vocabulary = vocabulary;
            return View();
        }

        [HttpPost]
        public IActionResult Practice(PracticeRequestModel model)
        {
            return RedirectToAction("Practice");
        }

        public IActionResult Quiz()
        {
            PracticeResponseModel vocabulary = _service.GetQuiz();
            if (vocabulary == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Vocabulary = vocabulary;
            return View();
        }

        [HttpPost]
        public IActionResult Quiz(PracticeRequestModel model)
        {
            return RedirectToAction("Quiz");
        }

        public IActionResult Flashcard()
        {
            PracticeResponseModel vocabulary = _service.GetFlashcard();
            if (vocabulary == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Vocabulary = vocabulary;
            return View();
        }

        [HttpPost]
        public IActionResult Flashcard(FlashcardRequestModel model)
        {
            _service.LearningFlashcard(model);
            return RedirectToAction("Flashcard");
        }
    }
}
