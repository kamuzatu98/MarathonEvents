using System.ComponentModel.DataAnnotations;
namespace MarathonEvents.API.DTO
{
    public class CreateEventDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime EventDate { get; set; }
        [Required]      
        public string Location { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Distance { get; set; } = string.Empty;
        [Required]
        [Range(0.1, 100)]
        public string EventType { get; set; } = string.Empty;//Runs, walk or cycle....
        [Required]
        public string BannerImageUrl { get; set; } = string.Empty;
    }
}
