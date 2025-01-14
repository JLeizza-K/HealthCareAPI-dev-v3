namespace HealthCareApi_dev_v3.Models.Entities
{
    public class TimeSlot
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; } = TimeSpan.FromMinutes(30);
        public bool Status { get; set; }
    }
}
