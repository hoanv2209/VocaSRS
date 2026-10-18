using System.ComponentModel.DataAnnotations;

namespace VocaSRS.Context.Entities
{
    public class Vocabulary
    {
        [Key]
        public int Id { get; set; }

        public string NativeWord { get; set; }

        public string ForeignWord { get; set; }

        public PracticeStatus Status { get; set; }

        public DateTime? PracticeDate { get; set; }
    }
}
