using System;
using RestaurantManagement.InventoryService.Domain.Exceptions;

namespace RestaurantManagement.InventoryService.Domain.ValueObjects
{
    public class DateRange
    {
        private DateRange() { }

        public DateRange(DateTime start, DateTime end)
        {
            if (end < start)
                throw new InventoryDomainException("End date cannot be before start date");

            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public static DateRange Create(DateTime start, DateTime end)
        {
            return new DateRange(start, end);
        }

        public static DateRange CreateForDay(DateTime date)
        {
            DateTime startOfDay = date.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            return new DateRange(startOfDay, endOfDay);
        }

        public static DateRange CreateForMonth(int year, int month)
        {
            DateTime startOfMonth = new DateTime(year, month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);
            return new DateRange(startOfMonth, endOfMonth);
        }

        public static DateRange CreateForYear(int year)
        {
            DateTime startOfYear = new DateTime(year, 1, 1);
            DateTime endOfYear = startOfYear.AddYears(1).AddTicks(-1);
            return new DateRange(startOfYear, endOfYear);
        }

        public static DateRange CreateForQuarter(int year, int quarter)
        {
            if (quarter < 1 || quarter > 4)
                throw new InventoryDomainException("Quarter must be between 1 and 4");

            int startMonth = (quarter - 1) * 3 + 1;
            DateTime startOfQuarter = new DateTime(year, startMonth, 1);
            DateTime endOfQuarter = startOfQuarter.AddMonths(3).AddTicks(-1);
            return new DateRange(startOfQuarter, endOfQuarter);
        }

        public static DateRange CreateForWeek(int year, int weekNumber)
        {
            if (weekNumber < 1 || weekNumber > 53)
                throw new InventoryDomainException("Week number must be between 1 and 53");

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;
            DateTime firstSunday = jan1.AddDays(daysOffset);
            if (firstSunday < jan1)
                firstSunday = firstSunday.AddDays(7);

            DateTime startOfWeek = firstSunday.AddDays((weekNumber - 1) * 7);
            DateTime endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);
            return new DateRange(startOfWeek, endOfWeek);
        }

        public static DateRange CreateForLast(int days)
        {
            if (days <= 0)
                throw new InventoryDomainException("Days must be greater than 0");

            DateTime now = DateTime.UtcNow;
            DateTime start = now.AddDays(-days).Date;
            DateTime end = now;
            return new DateRange(start, end);
        }

        public int GetDayCount()
        {
            return (int)(End - Start).TotalDays + 1;
        }

        public bool Contains(DateTime date)
        {
            return date >= Start && date <= End;
        }

        public bool Overlaps(DateRange other)
        {
            return Start <= other.End && End >= other.Start;
        }

        public DateRange Intersect(DateRange other)
        {
            if (!Overlaps(other))
                throw new InventoryDomainException("DateRanges do not overlap");

            DateTime maxStart = Start > other.Start ? Start : other.Start;
            DateTime minEnd = End < other.End ? End : other.End;
            return new DateRange(maxStart, minEnd);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            DateRange other = (DateRange)obj;
            return Start == other.Start && End == other.End;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        public override string ToString()
        {
            return $"{Start:yyyy-MM-dd} to {End:yyyy-MM-dd}";
        }
    }
}
