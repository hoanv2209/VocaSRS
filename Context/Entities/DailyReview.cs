using System.ComponentModel.DataAnnotations;

namespace VocaSRS.Context.Entities
{
    public class DailyReview
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PracticeStatus Status { get; set; }

        public DateTime? PracticeDate { get; set; }
    }
}
