using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Schema;

namespace Flexitime.Blazor
{
    public class FlexitimeDayData
    {
        public Day Day { get; set; }
        public DateTime StartTime { get; set; } = new DateTime();
        public DateTime EndTime { get; set; } = new DateTime();
        public DateTime BreakTime { get; set; } = new DateTime();
        public TimeSpan WorkedTimeDay { get; set; }
        public string WorkedTimeDayString => WorkedTimeDay.ToString(@"hh\:mm");

        public List<string> Validate()
        {
            TimeSpan startTimeSpan = StartTime.TimeOfDay;
            TimeSpan endTimeSpan = EndTime.TimeOfDay;
            TimeSpan breakTimeSpan = BreakTime.TimeOfDay;

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
