using VocaSRS.Models;

namespace VocaSRS.Services
{
    public interface IVocaService
    {
        DashboardResponseModel GetVocabularyDashboardInfo();

        void AddVocabulary(VocabularyRequestModel model);

        PracticeResponseModel GetQuiz();

        PracticeResponseModel GetFlashcard();

        void LearningFlashcard(FlashcardRequestModel model);

        void LearningQuiz(PracticeRequestModel model);
    }
}
