namespace MarathonEvents.API.DTOs
{
    public class CaptureResultDto
    {
        public string EnrollmentId { get; set; } = string.Empty;
        public string FinishTime { get; set; } = string.Empty;
        public string FinishPosition { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
    }
}
