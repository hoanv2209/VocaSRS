using VocaSRS.Models;

namespace VocaSRS.Services
{
    public interface IVocaService
    {
        DashboardResponseModel GetVocabularyDashboardInfo();

        DashboardResponseModel GetDailyReviewDashboardInfo();

        DashboardResponseModel GetParagraphDashboardInfo();

        void AddVocabulary(VocabularyRequestModel model);

        PracticeResponseModel GetPracticeVocabulary();

        bool CheckAnswer(PracticeRequestModel model);

        void AddParagraph(ParagraphRequestModel model);

        ParagraphResponseModel GetRandomParagraph();

        void PracticeParagraph(int id);

        DailyReviewResponseModel GetDailyReviewLesson();

        void AddDailyReview(DailyReviewRequestModel model);

        void ReviewLesson(int id);
    }
}
