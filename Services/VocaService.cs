using System.Text;
using VocaSRS.Context;
using VocaSRS.Context.Entities;
using VocaSRS.Extensions;
using VocaSRS.Models;

namespace VocaSRS.Services
{
    public interface IVocaService
    {
        DashboardResponseModel GetDashboardInfo();

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

        public void AddVocabulary(VocabularyRequestModel model)
        {
            _context.Vocabularies.Add(new Vocabulary
            {
                LeftSideSentence = model.LeftSideSentence,
                RightSideSentence = model.RightSideSentence,
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
                OriginalSentence = BuildSentence(word.LeftSideSentence, word.RightSideSentence, word.ForeignWord),
                MixedSentence = BuildSentence(word.LeftSideSentence, word.RightSideSentence, word.NativeWord.HtmlHighLight()),
                ForeignWord = word.ForeignWord
            };
        }

        private string BuildSentence(string left, string right, string vocabulary)
        {
            var builder = new StringBuilder();

            var shouldAppendSpace = false;

            if (!string.IsNullOrWhiteSpace(left))
            {
                builder.Append(left);
                shouldAppendSpace = true;
            }

            if (!string.IsNullOrWhiteSpace(vocabulary))
            {
                if (shouldAppendSpace)
                {
                    builder.Append(" ");
                }

                builder.Append(vocabulary);
            }

            if (!string.IsNullOrWhiteSpace(right))
            {
                builder.Append(" ");
                builder.Append(right);
            }

            return builder.ToString();
        }

        public bool CheckAnswer(PracticeRequestModel model)
        {
            var vocabulary = _context.Vocabularies.SingleOrDefault(p => p.Id == model.Id);
            if (vocabulary == null)
            {
                throw new Exception();
            }

            if (vocabulary.ForeignWord == model.Answer.Trim())
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
