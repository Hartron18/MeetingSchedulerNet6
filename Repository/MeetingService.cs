using MeetingSchedulerNet6.Models;

namespace MeetingSchedulerNet6.Repository
{
    public class MeetingService:IMeetingService<Meeting>
    {
        public MeetingContext _context { get; }

        public MeetingService(MeetingContext context)
        {
           _context = context;
        }

        public async Task<IReadOnlyList<Meeting>> ListMeeting(DateTime startTime, DateTime endTime)
        {
            return _context.MeetingList.ToList();
        }

        public async Task<IReadOnlyList<Meeting>> ListAvailableSlots(DateTime startTime, DateTime endTime)
        {
            return _context.MeetingList.ToList();
        }

        public void CreateMeeting(Meeting meeting) => _context.Add(meeting);
       // public void DeleteMeeting(Guid meetingId) => DeleteMeeting(meetingId);
       public void SaveChanges() => _context.SaveChangesAsync();
    }
}
