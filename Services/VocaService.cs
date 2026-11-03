using VocaSRS.Context;
using VocaSRS.Context.Entities;
using VocaSRS.Extensions;
using VocaSRS.Models;

namespace VocaSRS.Services
{
    public class VocaService : IVocaService
    {
        private readonly VocaContext _context;

        public VocaService(VocaContext context)
        {
            _context = context;
        }

        public DashboardResponseModel GetVocabularyDashboardInfo()
        {
            return new DashboardResponseModel()
            {
                Total = _context.Vocabularies.Count(),
                Memorized = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Memorized).Count(),
                Unseen = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Unseen).Count(),
                FirstReview = _context.Vocabularies.Where(p => p.Status == PracticeStatus.First).Count(),
                SecondReview = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Second).Count(),
                ThirdReview = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Third).Count(),
                FourthReview = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Fourth).Count(),
                FifthReview = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Fifth).Count()
            };
        }

        public void AddVocabulary(VocabularyRequestModel model)
        {
            string nativeWord = model.NativeWord.Trim().ToLower();
            string foreignWord = model.ForeignWord.Trim().ToLower();

            Vocabulary? existingVocabulary = _context.Vocabularies.FirstOrDefault(v => v.NativeWord == nativeWord && v.ForeignWord == foreignWord);

            if (existingVocabulary != null)
            {
                return;
            }

            _context.Vocabularies.Add(new Vocabulary
            {
                NativeWord = nativeWord,
                ForeignWord = foreignWord,
                Status = PracticeStatus.Unseen,
                PracticeDate = null
            });

            _context.SaveChanges();
        }

        private PracticeResponseModel GetPracticeVocabulary()
        {
            DateTime today = DateTime.UtcNow.Date;
            DateTime firstBoxReviewDate = today.AddDays(-1);
            DateTime secondBoxReviewDate = today.AddDays(-2);
            DateTime thirdBoxReviewDate = today.AddDays(-4);
            DateTime fourthBoxReviewDate = today.AddDays(-6);
            DateTime fifthBoxReviewDate = today.AddDays(-10);

            Vocabulary? word = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Unseen
                                                || (p.Status == PracticeStatus.First && p.PracticeDate <= firstBoxReviewDate)
                                                || (p.Status == PracticeStatus.Second && p.PracticeDate <= secondBoxReviewDate)
                                                || (p.Status == PracticeStatus.Third && p.PracticeDate <= thirdBoxReviewDate)
                                                || (p.Status == PracticeStatus.Fourth && p.PracticeDate <= fourthBoxReviewDate)
                                                || (p.Status == PracticeStatus.Fifth && p.PracticeDate <= fifthBoxReviewDate))
                                    .OrderByDescending(p => p.Status)
                                    .ThenBy(p => Guid.NewGuid())
                                    .FirstOrDefault();

            if (word == null)
            {
                return null;
            }

            return new PracticeResponseModel
            {
                Id = word.Id,
                ForeignWord = word.ForeignWord,
                NativeWord = word.NativeWord
            };
        }

        public PracticeResponseModel GetQuiz()
        {
            DateTime today = DateTime.UtcNow.Date;

            Vocabulary? word = _context.Vocabularies
                                .Where(p => p.Status != PracticeStatus.Unseen
                                            && p.Status != PracticeStatus.Memorized
                                            && p.PracticeDate == today
                                            && p.Count < 3)
                                .OrderBy(p => p.Count)
                                .ThenBy(p => Guid.NewGuid())
                                .FirstOrDefault();

            if (word == null)
            {
                return null;
            }

            string[] options = _context.Vocabularies.Where(p => p != word)
                                    .OrderBy(p => Guid.NewGuid())
                                    .Take(3)
                                    .Select(p => p.ForeignWord)
                                    .ToArray();

            return new PracticeResponseModel
            {
                Id = word.Id,
                ForeignWord = word.ForeignWord,
                NativeWord = word.NativeWord,
                Options = options
            };
        }

        public PracticeResponseModel GetFlashcard()
        {
            DateTime today = DateTime.UtcNow.Date;
            int practicedWordCount = _context.Vocabularies.Count(p => p.Status == PracticeStatus.First && p.PracticeDate == today);

            if (practicedWordCount >= 50)
            {
                return null;
            }

            return GetPracticeVocabulary();
        }

        public void LearningFlashcard(FlashcardRequestModel model)
        {
            Vocabulary vocabulary = _context.Vocabularies.Single(p => p.Id == model.Id);

            vocabulary.PracticeDate = DateTime.UtcNow.Date;
            vocabulary.Count = 0;

            switch (model.PracticeStatus)
            {
                case 1:
                    vocabulary.Status = PracticeStatus.First;
                    break;
                case 2:
                    if (vocabulary.Status == PracticeStatus.Fifth)
                    {
                        vocabulary.Status = PracticeStatus.First;
                    }
                    else
                    {
                        vocabulary.Status = vocabulary.Status.Next();
                    }
                    break;
                case 3:
                    vocabulary.Status = PracticeStatus.Memorized;
                    break;
                default:
                    vocabulary.Status = PracticeStatus.Unseen;
                    break;
            }

            _context.SaveChanges();
        }

        public void LearningQuiz(PracticeRequestModel model)
        {
            Vocabulary vocabulary = _context.Vocabularies.Single(p => p.Id == model.Id);
            if (vocabulary.ForeignWord == model.Answer.Trim().ToLower())
            {
                vocabulary.Count++;
            }
            else
            {
                vocabulary.Count = 0;
            }

            _context.SaveChanges();
        }
    }
}
