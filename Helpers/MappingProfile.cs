using AutoMapper;
using MeetingSchedulerNet6.Models;
using MeetingSchedulerNet6.Models.Dtos;

namespace MeetingSchedulerNet6.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMeetingDto, Meeting>();
            CreateMap<Meeting, ListMeetingDto>();
            CreateMap<Meeting, AvailableSlotsDto>();
        }

        
    }
}
