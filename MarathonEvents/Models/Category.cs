using System.ComponentModel.DataAnnotations.Schema;

namespace MarathonEvents.Models
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string EventId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryType { get; set; }
        public int minAge { get; set; }
        public int maxAge { get; set; }
        public string Distance { get; set; }

        //Navigation property to the Event entity
        [ForeignKey("EventId")]
        public Event Vent { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}
