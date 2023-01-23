using MeetingSchedulerNet6.Models;
using MeetingSchedulerNet6.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MeetingSchedulerNet6.Repository
{
    public class MeetingService:IMeetingService
    {
        public MeetingContext _context { get; }

        public MeetingService(MeetingContext context)
        {
           _context = context;
        }

        public async Task<IReadOnlyList<Meeting>> ListMeeting(DateTime? startTime, DateTime? endTime)
        {
            var meetings = await _context.MeetingList.ToListAsync();

            if ((startTime == null) || (endTime == null))
            {
                return meetings;
            }

            return meetings.Where(x => x.StartTime >= startTime && x.EndTime <= endTime)
                                 .Where(x => (x.StartTime == DateTime.Today) && (x.EndTime == DateTime.Today))
                                 .ToList();
        }

        public async Task<IEnumerable<AvailableSlotsDto>> ListAvailableSlots(DateTime startTime, DateTime endTime)
        {
            var meetings =await _context.MeetingList.OrderBy(x => x.StartTime).ToArrayAsync();
            var availableSlots = new List<AvailableSlotsDto>().ToArray();
            for (int i = 0; i < meetings.Length; i++)
            {
                TimeSpan meeting = meetings[i+1].StartTime.ToLocalTime() - meetings[i].EndTime.ToLocalTime();
                //var availablePeriod = meeting.Subtract(TimeSpan.FromMinutes(30));
                var availableperiod = new AvailableSlotsDto();
                if (meeting.Hours > 1)
                {                 
                    availableperiod.StartTime =meetings[i].EndTime.AddMinutes(30);
                    availableperiod.EndTime =meetings[i+1].StartTime;  
                }
                availableSlots.Append(availableperiod);

            }
            
            return availableSlots.Where(x => (x.StartTime >= startTime) && (x.EndTime <= endTime));
        }

        public void CreateMeeting(Meeting meeting)
        {
            
            _context.MeetingList.Add(meeting);
            SaveChanges();
        }
        
       // public void DeleteMeeting(Guid meetingId) => DeleteMeeting(meetingId);
       public void SaveChanges() => _context.SaveChangesAsync();
    }
}
