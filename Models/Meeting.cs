namespace MeetingSchedulerNet6.Models
{
    public class Meeting
    {
        public Guid MeetingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //public AppUser User { get; set; }
        public string Name { get; set; }
    }
}
