using AutoMapper;
using MeetingSchedulerNet6.Helpers;
using MeetingSchedulerNet6.Models;
using MeetingSchedulerNet6.Models.Dtos;
using MeetingSchedulerNet6.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingSchedulerNet6.Controllers
{
    [ApiController]
    [Route("api/[Controller]/")]
    public class MeetingController : ControllerBase
    {
        public MeetingController(MeetingContext context, IMeetingService meetingService, IMapper mapper)
        {
            _context = context;
            _meetingService = meetingService;
            _mapper = mapper;
        }

        public MeetingContext _context { get; }
        public IMeetingService _meetingService { get; }
        public IMapper _mapper { get; }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Meeting>>> ListMeetings(DateTime? startTime, DateTime? endTime)
        {
            var meetings = await _meetingService.ListMeeting(startTime, endTime);

            var mappedMeetings = _mapper.Map<IReadOnlyList<ListMeetingDto>>(meetings);

            return Ok(mappedMeetings);
        }

        [HttpGet]
        public async Task<IEnumerable<AvailableSlotsDto>> ListAvailableSlots(AvailableSlotsDto slot)
        {
            
            return await _meetingService.ListAvailableSlots(slot.StartTime, slot.EndTime);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMeetingAsync(CreateMeetingDto meeting)
        {
            if (!ModelState.IsValid) return BadRequest();

            //checks if date is within the present day
            if ((meeting.StartTime.Date.ToLocalTime() != DateTime.Today.ToLocalTime()) && (meeting.EndTime.Date.ToLocalTime() != DateTime.Today.ToLocalTime()))
            {
                return BadRequest("The date should be within today");
            }

            //Checks if the time period is within the stated working hours of the day
            if ((meeting.StartTime.Hour < 9))
            {
                return BadRequest("The time period should be within working hours of 9:00am to 17:00pm");
            }
            if ((meeting.EndTime.Hour > 17))
            {
                return BadRequest("The time period should be within working hours of 9:00am to 17:00pm");
            }

            //checks if the time period is valid
            if (meeting.EndTime <= meeting.StartTime) return BadRequest();

            //Updates to 00/30 minutes timestamp 
            var strtMinutes = meeting.StartTime.Minute;
            var strtSeconds = meeting.StartTime.Second;
            var minsInEnd = meeting.EndTime.Minute;
            var secsInEnd = meeting.EndTime.Second;

            if (strtMinutes < 30)
            {
                meeting.StartTime = meeting.StartTime.AddMinutes(29 - strtMinutes).ToLocalTime();
            }
            else
            {
                meeting.StartTime = meeting.StartTime.AddMinutes(59 - strtMinutes).ToLocalTime();
            }
            if (minsInEnd < 30)
            {
                meeting.EndTime = meeting.EndTime.AddMinutes(29 - minsInEnd).ToLocalTime();
            }
            else
            {
                meeting.EndTime = meeting.EndTime.AddMinutes(59 - minsInEnd).ToLocalTime();
            }
            meeting.StartTime = meeting.StartTime.AddSeconds(60 - strtSeconds).ToLocalTime();
            meeting.EndTime = meeting.EndTime.AddSeconds(60 - secsInEnd).ToLocalTime();

            //Checks if the Meeting period is within 2 hours
            TimeSpan meetingLength = meeting.EndTime - meeting.StartTime;
            if (meetingLength.TotalHours > 2)
            {
                return BadRequest("Meeting period should not be more than 2hrs");
            }
            
            var meetings = await _context.MeetingList.OrderBy(x => x.StartTime).ToListAsync();
            
            //Checks if the time period has been taken
            foreach (var meetingA in meetings)
            {
                var range = new DateRange(meetingA.StartTime.ToLocalTime(), meetingA.EndTime.ToLocalTime());

                if ((range.WithInRange(meeting.StartTime) == true) && (range.WithInRange(meeting.StartTime) == true)) return BadRequest("This period has been taken");
            }

            var mapped = _mapper.Map<Meeting>(meeting);

            _meetingService.CreateMeeting(mapped);

            return Ok(mapped);
        }
    }
}
