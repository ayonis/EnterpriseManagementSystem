using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseManagementSystem.Application.Common.Dtos
{
    public class TimeDto
    {
        public TimeDto()
            {
            }

            public TimeDto(int hour, int minute)
            {
                if (hour < 0 || hour > 23)
                    throw new ArgumentOutOfRangeException(nameof(hour), "Hour must be between 0 and 23.");

                if (minute < 0 || minute > 59)
                    throw new ArgumentOutOfRangeException(nameof(minute), "Minute must be between 0 and 59.");

                Hour = hour;
                Minute = minute;
            }

            public int Hour { get; private set; }
            public int Minute { get; private set; }

            public static TimeDto FromTimeSpan(TimeSpan timeSpan)
            {
                return new TimeDto(timeSpan.Hours, timeSpan.Minutes);
            }

            public static TimeDto FromString(string time)
            {
                if (TimeSpan.TryParse(time, out var ts))
                {
                    return new TimeDto(ts.Hours, ts.Minutes);
                }

                throw new FormatException("Time string must be in 'HH:mm' format.");
            }

            public TimeSpan ToTimeSpan() => new TimeSpan(Hour, Minute, 0);

            public override string ToString() => $"{Hour:D2}:{Minute:D2}";

            public void Deconstruct(out int hour, out int minute)
            {
                hour = Hour;
                minute = Minute;
            }

            public void SetTime(int hour, int minute)
            {
                if (hour < 0 || hour > 23)
                    throw new ArgumentOutOfRangeException(nameof(hour), "Hour must be between 0 and 23.");

                if (minute < 0 || minute > 59)
                    throw new ArgumentOutOfRangeException(nameof(minute), "Minute must be between 0 and 59.");

                Hour = hour;
                Minute = minute;
            }
        }
    }
