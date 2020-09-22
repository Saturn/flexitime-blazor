
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Flexitime.Blazor
{
    public class FlexitimeWeekData : IEnumerable<FlexitimeDayData>
    {
        private Dictionary<Day, FlexitimeDayData> _data = new Dictionary<Day, FlexitimeDayData>();

        public TimeSpan WorkedTime { get; set; } = new TimeSpan();
        public string WorkedTimeWeekString => $"{(int)WorkedTime.TotalHours} hours {WorkedTime.Minutes} minutes.";

        public FlexitimeWeekData()
        {
            foreach (Day day in Enum.GetValues(typeof(Day)))
            {
                _data[day] = new FlexitimeDayData();
            }
        }

        public FlexitimeDayData this[Day day]
        {
            get => _data[day];
            set => _data[day] = value;
        }

        public IEnumerator<FlexitimeDayData> GetEnumerator()
        {
            return _data.Keys.Select(day => _data[day]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Dictionary<Day, List<string>> Validate()
        {
            Dictionary<Day, List<string>> errors = new Dictionary<Day, List<string>>();
            foreach ((Day day, FlexitimeDayData dayData) in _data)
            {
                List<string> dayErrors = dayData.Validate();
                if (dayErrors.Any())
                {
                    errors[day] = dayErrors;
                }
            }
            return errors;
        }
    }

}
