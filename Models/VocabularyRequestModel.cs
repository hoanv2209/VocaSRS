namespace VocaSRS.Models
{
    public class VocabularyRequestModel
    {
        public string RightSideSentence { get; set; }

        public string LeftSideSentence { get; set; }

        public string NativeWord { get; set; }

        public string ForeignWord { get; set; }
    }
}