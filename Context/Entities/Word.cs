using System.ComponentModel.DataAnnotations;

namespace VocaSRS.Context.Entities
{
    public class Vocabulary
    {
        [Key]
        public int Id { get; set; }

        public string RightSideSentence { get; set; }

        public string LeftSideSentence { get; set; }

        public string NativeWord { get; set; }

        public string ForeignWord { get; set; }

        public PracticeStatus Status { get; set; }

        public DateTime? PracticeDate { get; set; }
    }

    public enum PracticeStatus
    {
        Unseen = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5,
        Memorized = 6
    }
}
