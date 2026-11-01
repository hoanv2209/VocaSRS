namespace VocaSRS.Models
{
    public class PracticeResponseModel
    {
        public int Id { get; set; }

        public string ForeignWord { get; set; }

        public string NativeWord { get; set; }

        public string[] Options { get; set; }
    }
}