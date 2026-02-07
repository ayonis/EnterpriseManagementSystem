using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseManagementSystem.Application.Common.Dtos
{
    public class DateDto
    {
        public DateDto()
        {

        }
        public DateDto(int year, int month, int day, DayOfWeek dayOfWeek)
        {
            Year = year;
            Month = month;
            Day = day;
        }
        public DateTime? ToDateTime()
        {
            if (Year <= 0 || Month <= 0 || Day <= 0)
                return null;

            try
            {
                return new DateTime(Year, Month, Day);
            }
            catch
            {
                return null;
            }
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }

}
