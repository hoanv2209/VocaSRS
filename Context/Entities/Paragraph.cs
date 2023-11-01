using System.ComponentModel.DataAnnotations;

namespace VocaSRS.Context.Entities
{
    public class Paragraph
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SoundPath { get; set; }

        public int Times { get; set; }
    }
}
