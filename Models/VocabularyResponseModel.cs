namespace VocaSRS.Models
{
    public class VocabularyResponseModel
    {
        public int Id { get; set; }

        public string Sentence { get; set; }

        public string NativeWord { get; set; }

        public string ForeignWord { get; set; }
    }
}