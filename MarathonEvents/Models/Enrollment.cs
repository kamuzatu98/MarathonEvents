using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarathonEvents.Models
{
    public class Enrollment
    {
        [Key]
      
        public string EnrollmentId { get; set; }
        [Required]
        public string Participant_Id { get; set; }
        [Required]
        public int EventId { get; set; }
        [Required]
        public string CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string EnrollmentStatus { get; set; }
        [Required]
        public string EnrolledAt { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;

        public ICollection<Results> Results { get; set; } = new List<Results>();
    }
}
