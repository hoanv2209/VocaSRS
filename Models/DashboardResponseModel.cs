namespace VocaSRS.Models
{
    public class DashboardResponseModel
    {
        public int Total { get; set; }
        public int Memorized { get; set; }
        public int Unseen { get; set; }
        public int FirstReview { get; set; }
        public int SecondReview { get; set; }
        public int ThirdReview { get; set; }
        public int FourthReview { get; set; }
        public int FifthReview { get; set; }
    }
}