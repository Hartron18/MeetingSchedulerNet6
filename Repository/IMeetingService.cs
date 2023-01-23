using MeetingSchedulerNet6.Models;
using MeetingSchedulerNet6.Models.Dtos;

namespace MeetingSchedulerNet6.Repository
{
    public interface IMeetingService
    {
        Task<IEnumerable<AvailableSlotsDto>> ListAvailableSlots(DateTime startTime, DateTime endTime);
        Task<IReadOnlyList<Meeting>> ListMeeting(DateTime? startTime, DateTime? endTime);
        void CreateMeeting(Meeting meeting);
        void SaveChanges();
    }
}
