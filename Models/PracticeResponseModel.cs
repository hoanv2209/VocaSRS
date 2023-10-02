namespace VocaSRS.Models
{
    public class PracticeResponseModel
    {
        public int Id { get; set; }

        public string OriginalSentence { get; set; }

        public string MixedSentence { get; set; }

        public string ForeignWord { get; set; }
    }
}