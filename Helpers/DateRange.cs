namespace MeetingSchedulerNet6.Helpers
{
    public class DateRange
    {
        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; }
        public DateTime End { get; }

        public bool WithInRange(DateTime value)
        {
            return (value >= Start) && (value <= End);
        }
    }
}
