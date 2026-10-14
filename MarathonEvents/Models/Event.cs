namespace MarathonEvents.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string OrganizerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Distance { get; set; }
        public string Event_Type { get; set; }
        public string BannerImage { get; set; }
        public string createAt { get; set; }
        public string updateAt { get; set; } 

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Route> Routes { get; set; } = new List<Route>();

    }
}
