namespace HealthCareApi_dev_v3.Models.Entities
{
    public class TimeSlot
    {
        public Guid Id { get; set; }
        public Guid AvailabilityId { get; set; }
        public string Status { get; set; }

    }
}
