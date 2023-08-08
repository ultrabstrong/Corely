namespace Corely.Core.Dates
{
    public static class DayOfMonthCalculator
    {
        public static DateTime GetLastBusinessDayOfMonth(DateTime date)
        {
            DateTime lastWorkingDay = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            if (lastWorkingDay.DayOfWeek == DayOfWeek.Sunday)
            {
                lastWorkingDay = lastWorkingDay.AddDays(-2);
            }
            else if (lastWorkingDay.DayOfWeek == DayOfWeek.Saturday)
            {
                lastWorkingDay = lastWorkingDay.AddDays(-1);
            }
            return lastWorkingDay;
        }
    }
}
