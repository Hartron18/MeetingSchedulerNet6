using Microsoft.AspNetCore.Identity;

namespace MeetingSchedulerNet6.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
