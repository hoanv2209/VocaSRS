using VocaSRS.Context;
using VocaSRS.Context.Entities;
using VocaSRS.Extensions;
using VocaSRS.Models;

namespace VocaSRS.Services
{
    public interface IVocaService
    {
        DashboardResponseModel GetDashboardInfo();

        IEnumerable<VocabularyResponseModel> GetVocabularies(int pageSize, int skip, string filter);

        void AddVocabulary(VocabularyRequestModel model);

        PracticeResponseModel GetPracticeVocabulary();

        bool CheckAnswer(PracticeRequestModel model);
    }

    public class VocaService : IVocaService
    {
        private readonly VocaContext _context;

        public VocaService(VocaContext context)
        {
            _context = context;
        }

        public DashboardResponseModel GetDashboardInfo()
        {
            return new DashboardResponseModel()
            {
                TotalVocabularies = _context.Vocabularies.Count(),
                MemorizedVocabularies= _context.Vocabularies.Where(p => p.Status == PracticeStatus.Memorized).Count(),
                UnseenVocabularies = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Unseen).Count(),
                FirstReviewVocabularies = _context.Vocabularies.Where(p => p.Status == PracticeStatus.First).Count(),
                SecondReviewVocabularies= _context.Vocabularies.Where(p => p.Status == PracticeStatus.Second).Count(),
                ThirdReviewVocabularies= _context.Vocabularies.Where(p => p.Status == PracticeStatus.Third).Count(),
                FourthReviewVocabularies= _context.Vocabularies.Where(p => p.Status == PracticeStatus.Fourth).Count(),
                FifthReviewVocabularies= _context.Vocabularies.Where(p => p.Status == PracticeStatus.Fifth).Count()
            };
        }

        public IEnumerable<VocabularyResponseModel> GetVocabularies(int pageSize, int skip, string filter)
        {
            return _context.Vocabularies.Where(p => p.NativeWord.Contains(filter)).Skip(skip).Take(pageSize).Select(p => new VocabularyResponseModel
            {
                Id = p.Id,
                NativeWord = p.NativeWord,
                ForeignWord = p.ForeignWord,
                Sentence = p.Sentence
            });
        }

        public void AddVocabulary(VocabularyRequestModel model)
        {
            _context.Vocabularies.Add(new Vocabulary
            {
                Sentence = model.Sentence,
                NativeWord = model.NativeWord,
                ForeignWord = model.ForeignWord,
                Status = PracticeStatus.Unseen,
                PracticeDate = null
            });
            _context.SaveChanges();
        }

        public PracticeResponseModel GetPracticeVocabulary()
        {
            var today = DateTime.UtcNow.Date;
            var firstBoxReviewDate = today.AddDays(-1);
            var secondBoxReviewDate = today.AddDays(-3);
            var thirdBoxReviewDate = today.AddDays(-10);
            var fourthBoxReviewDate = today.AddDays(-30);
            var fifthBoxReviewDate = today.AddDays(-90);

            var word = _context.Vocabularies.Where(p => p.Status == PracticeStatus.Unseen
                                                || (p.Status == PracticeStatus.First && p.PracticeDate <= firstBoxReviewDate)
                                                || (p.Status == PracticeStatus.Second && p.PracticeDate <= secondBoxReviewDate)
                                                || (p.Status == PracticeStatus.Third && p.PracticeDate <= thirdBoxReviewDate)
                                                || (p.Status == PracticeStatus.Fourth && p.PracticeDate <= fourthBoxReviewDate)
                                                || (p.Status == PracticeStatus.Fifth && p.PracticeDate <= fifthBoxReviewDate))
                                    .FirstOrDefault();

            if (word == null)
            {
                return null;
            }

            return new PracticeResponseModel
            {
                Id = word.Id,
                NativeSentence = String.Format(word.Sentence, word.NativeWord),
                MixedSentence = String.Format(word.Sentence, word.ForeignWord.HtmlHighLight()),
                NativeWord = word.NativeWord,
            };
        }

        public bool CheckAnswer(PracticeRequestModel model)
        {
            var vocabulary = _context.Vocabularies.SingleOrDefault(p => p.Id == model.Id);
            if (vocabulary == null)
            {
                throw new Exception();
            }

            if (vocabulary.NativeWord == model.Answer)
            {
                vocabulary.PracticeDate = DateTime.UtcNow.Date;
                vocabulary.Status = vocabulary.Status.Next();
                _context.SaveChanges();

                return true;
            }

            vocabulary.PracticeDate = null;
            vocabulary.Status = PracticeStatus.Unseen;
            _context.SaveChanges();

            return false;
        }
    }

}
