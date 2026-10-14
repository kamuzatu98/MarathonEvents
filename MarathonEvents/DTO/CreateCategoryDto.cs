using System.ComponentModel.DataAnnotations;

namespace MarathonEvents.API.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // e.g., "Senior", "Under 20", "10km"

        [Required]
        public string CategoryType { get; set; } = string.Empty; // "Age" or "Distance"

        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public double? DistanceKm { get; set; }
    }
}