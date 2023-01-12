using AutoMapper;
using MeetingSchedulerNet6.Helpers;
using MeetingSchedulerNet6.Models;
using MeetingSchedulerNet6.Models.Dtos;
using MeetingSchedulerNet6.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSchedulerNet6.Controllers
{
    [ApiController]
    [Route("api/[Controller]/")]
    public class MeetingController : ControllerBase
    {
        public MeetingController(MeetingContext context, IMeetingService<Meeting> meetingService, IMapper mapper)
        {
            _context = context;
            _meetingService = meetingService;
            _mapper = mapper;
        }

        public MeetingContext _context { get; }
        public IMeetingService<Meeting> _meetingService { get; }
        public IMapper _mapper { get; }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Meeting>>> ListMeetings(DateTime startTime, DateTime endTime)
        {
            var meetings = await _meetingService.ListMeeting(startTime, endTime);

            return Ok(meetings);
        }

        //[HttpGet]
        //public async Task<IReadOnlyList<AvailableSlotsDto>> ListAvailableSlots(AvailableSlotsDto slot)
        //{

        //}

        [HttpPost]
        public async Task<ActionResult> CreateMeetingAsync(CreateMeetingDto meeting)
        {
            var meetings = _context.MeetingList.OrderBy(x => x.StartTime).ToList();

            if (!ModelState.IsValid) return BadRequest();

            //checks if date is within the present day
            if ((meeting.StartTime.Date != DateTime.Today) && (meeting.EndTime.Date != DateTime.Today))
            {
                return BadRequest("The date should be within today");
            }

            //Updates to 00/30 minutes timestamp 
            switch (meeting.StartTime.Minute)
            {
                case < 30:
                    meeting.StartTime.AddSeconds(60-meeting.StartTime.Second);
                    meeting.StartTime.AddMinutes(30 - meeting.StartTime.Minute);
                    break;
                case > 30:
                    meeting.StartTime.AddSeconds(60 - meeting.StartTime.Second);
                    meeting.StartTime.AddMinutes(60 - meeting.StartTime.Minute);
                    break;
            }

            switch (meeting.EndTime.Minute)
            {
                case < 30:
                    meeting.EndTime.AddSeconds(60 - meeting.EndTime.Second);
                    meeting.EndTime.AddMinutes(30 - meeting.EndTime.Minute);
                    break;
                case > 30:
                    meeting.EndTime.AddSeconds(60 - meeting.EndTime.Second);
                    meeting.EndTime.AddMinutes(60 - meeting.EndTime.Minute);
                    break;
            }

            //checks if the time period is valid
            if (meeting.EndTime <= meeting.StartTime) return BadRequest();

            //Checks if the time period is within the stated working hours of the day
            if ((meeting.StartTime.TimeOfDay < TimeSpan.FromHours(9)) && (meeting.EndTime.TimeOfDay > TimeSpan.FromHours(17)))
            {
                return BadRequest("The time period should be within working hours of 9:00am to 17:00pm");
            }

            TimeSpan meetingLength = meeting.EndTime - meeting.StartTime;
            if (meetingLength.TotalHours > 2)
            {
                meeting.EndTime = meeting.StartTime.AddHours(2);
            }
            
            //Checks if the time period has been taken
            foreach (var meetingA in meetings)
            {
                var range = new DateRange(meetingA.StartTime, meetingA.EndTime);

                if ((range.WithInRange(meeting.StartTime) == true) && (range.WithInRange(meeting.StartTime) == true)) return BadRequest("This period has been taken");
            }

            var mapped = _mapper.Map<Meeting>(meeting);

            _meetingService.CreateMeeting(mapped);

            return Ok(meeting);
        }
    }
}
