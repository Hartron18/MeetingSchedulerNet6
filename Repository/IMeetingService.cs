namespace MeetingSchedulerNet6.Repository
{
    public interface IMeetingService<Meeting>
    {
        Task<IReadOnlyList<Models.Meeting>> ListAvailableSlots(DateTime startTime, DateTime endTime);
        Task<IReadOnlyList<Models.Meeting>> ListMeeting(DateTime startTime, DateTime endTime);
        void CreateMeeting(Models.Meeting meeting);
        void SaveChanges();
    }
}
