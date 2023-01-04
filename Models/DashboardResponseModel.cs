namespace VocaSRS.Models
{
    public class DashboardResponseModel
    {
        public int TotalVocabularies { get; set; }
        public int MemorizedVocabularies { get; set; }
        public int UnseenVocabularies { get; set; }
        public int FirstReviewVocabularies { get; set; }
        public int SecondReviewVocabularies { get; set; }
        public int ThirdReviewVocabularies { get; set; }
        public int FourthReviewVocabularies { get; set; }
        public int FifthReviewVocabularies { get; set; }
    }
}