using MeetingSchedulerNet6.Models;
namespace MeetingSchedulerNet6.Repository
{
    public interface IMeetingService<Meeting>
    {
        //Task<IReadOnlyList<Meeting>> ListAvailableSlots(DateTime startTime, DateTime endTime);
        Task<IReadOnlyList<Meeting>> ListMeeting(DateTime startTime, DateTime endTime);
        void CreateMeeting(Meeting meeting);
        void SaveChanges();
    }
}
