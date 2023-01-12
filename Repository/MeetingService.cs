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
            var meetings = _context.MeetingList.ToList();
            meetings.Where(x => (x.StartTime != DateTime.Today) && (x.EndTime != DateTime.Today));
            meetings.RemoveAll(x => (x.StartTime != DateTime.Today) && (x.EndTime != DateTime.Today));

            return meetings.Where(x => x.StartTime >= startTime && x.EndTime <= endTime).ToList();
        }

        //public async Task<IReadOnlyList<Meeting>> ListAvailableSlots(DateTime startTime, DateTime endTime)
        //{
        //    ;
        //}

        public void CreateMeeting(Meeting meeting)
        {
            _context.MeetingList.Add(meeting);
            SaveChanges();
        }
        
       // public void DeleteMeeting(Guid meetingId) => DeleteMeeting(meetingId);
       public void SaveChanges() => _context.SaveChangesAsync();
    }
}
