namespace MeetingSchedulerNet6.Models.Dtos
{
    public class CreateMeetingDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //public AppUser User { get; set; }
        public string Name { get; set; }
    }
}
