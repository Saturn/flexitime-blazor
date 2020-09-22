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
        public string WorkedTimeDayString => $"{WorkedTimeDay.Hours}:{WorkedTimeDay.Minutes}";

        public List<string> Validate()
        {
            List<string> errors = new List<string>();
            if (StartTime > EndTime)
            {
                errors.Add("End time cannot be before Start time");
            }

            TimeSpan workedTime = EndTime - StartTime;
            if (workedTime.TotalSeconds < BreakTime.TimeOfDay.TotalSeconds)
            {
                errors.Add("Break time cannot be longer than total work time");
            }

            if (!errors.Any())
            {
                var x = BreakTime.TimeOfDay;
                WorkedTimeDay = EndTime - StartTime - BreakTime.TimeOfDay;
            }
            else
            {
                WorkedTimeDay = TimeSpan.Zero;
            }

            return errors;
        }
    }
}
