
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Flexitime.Blazor
{
    public class FlexitimeWeekData : IEnumerable<FlexitimeDayData>
    {
        private Dictionary<Day, FlexitimeDayData> _data = new Dictionary<Day, FlexitimeDayData>();

        public TimeSpan WorkedTime { get; set; }
        public string WorkedTimeWeekString => $"{(int)Math.Floor(WorkedTime.TotalHours):00}:{WorkedTime.Minutes:00}";

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
            set
            {
                value.Day = day;
                _data[day] = value;
            }
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

            WorkedTime = CalculateAllWorkedTime(_data.Values);
            return errors;
        }

        private TimeSpan CalculateAllWorkedTime(IEnumerable<FlexitimeDayData> dayDatas)
        {
            double minutes = 0;
            foreach (FlexitimeDayData dayData in dayDatas)
            {
                Debug.WriteLine($"{dayData} hours is {dayData.WorkedTimeDay.Hours} and minutes is {dayData.WorkedTimeDay.Minutes}.");
                minutes += dayData.WorkedTimeDay.TotalMinutes;
            }
            Debug.WriteLine($"minutes = {minutes}");
            return TimeSpan.FromMinutes(minutes);
        }
    }

}
