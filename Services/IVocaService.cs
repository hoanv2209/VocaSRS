using VocaSRS.Models;

namespace VocaSRS.Services
{
    public interface IVocaService
    {
        DashboardResponseModel GetDashboardInfo();

        void AddVocabulary(VocabularyRequestModel model);

        PracticeResponseModel GetPracticeVocabulary();

        bool CheckAnswer(PracticeRequestModel model);

        void AddParagraph(ParagraphRequestModel model);

        ParagraphResponseModel GetRandomParagraph();

        void IncreaseParagraphPracticeTimes(int id);
    }
}
