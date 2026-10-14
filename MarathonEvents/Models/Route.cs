using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarathonEvents.Models;

    public class Route
    {
    [Key]
        public string RouteId { get; set; }
    [Required]
        public int EventId { get; set; }
    [Required]
    [MaxLength(100)]
        public string RouteName { get; set; }
        public string Description { get; set; }
        public string MapURL { get; set; }
    [ForeignKey("EventId")]
    public Event Event { get; set; } = null!;
}

