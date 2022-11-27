using MeetingSchedulerNet6.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingSchedulerNet6
{
    public class MeetingContext : DbContext
    {
        public MeetingContext(DbContextOptions options) : base(options)
        {
        }

        protected override void  OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        DbSet<Meeting> MeetingList { get; set; }
    }
}
