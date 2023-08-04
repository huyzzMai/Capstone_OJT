using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Utilities
{
    public static class DateTimeService
    {        
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow.AddHours(7);
        }
        public static DateTime ConvertStringToDateTime(string dateString)
        {
            string[] dateFormats = { "d/M/yyyy", "dd/MM/yyyy" };
            if (DateTime.TryParseExact(dateString, dateFormats, null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Invalid date format. The input string must be in the format 'd/M/yyyy' or 'dd/MM/yyyy'.");
            }
        }
        public static string ConvertToDateString(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                string formattedDate = dateTime.Value.ToString("dd/MM/yyyy");
                return formattedDate;
            }

            return null;
        }
        public static TimeSpan ConvertToTimeSpan(double hours)
        {
            int totalMinutes = (int)(hours * 60);
            int hoursPart = totalMinutes / 60;
            int minutesPart = totalMinutes % 60;
            TimeSpan timeSpan = new TimeSpan(hoursPart, minutesPart, 0);
            return timeSpan;
        }
    }
}
