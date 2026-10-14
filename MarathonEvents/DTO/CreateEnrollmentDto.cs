using System.ComponentModel.DataAnnotations;

namespace MarathonEvents.API.DTOs
{
    public class CreateEnrollmentDto
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}