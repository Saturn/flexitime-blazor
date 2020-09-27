using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Schema;

namespace Flexitime.Blazor
{
    public class FlexitimeDayData
    {
        public DateTime StartTime { get; set; } = new DateTime();
        public DateTime EndTime { get; set; } = new DateTime();
        public DateTime BreakTime { get; set; } = new DateTime();
        public TimeSpan WorkedTimeDay { get; set; }
        public string WorkedTimeDayString => WorkedTimeDay.ToString(@"hh\:mm");

        public List<string> Validate()
        {
            TimeSpan startTimeSpan = new TimeSpan(0, StartTime.Hour, StartTime.Minute, 0);
            TimeSpan endTimeSpan = new TimeSpan(0, EndTime.Hour, EndTime.Minute, 0);
            TimeSpan breakTimeSpan = new TimeSpan(0, BreakTime.Hour, BreakTime.Minute, 0);

            WorkedTimeDay = endTimeSpan - startTimeSpan - breakTimeSpan;
            List<string> errors = new List<string>();
            if (StartTime > EndTime)
            {
                errors.Add("End time cannot be before Start time");
            }

            TimeSpan workedTime = endTimeSpan - startTimeSpan;
            if (workedTime.TotalSeconds < breakTimeSpan.TotalSeconds)
            {
                errors.Add("Break time cannot be longer than total work time");
            }

            if (errors.Any())
            {
                WorkedTimeDay = TimeSpan.Zero;
            }

            return errors;
        }
    }
}
