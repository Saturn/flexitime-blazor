using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Flexitime.Blazor
{
    public class FlexitimeService
    {
        private readonly ILocalStorageService _localStorageService;

        public readonly IEnumerable<Day> Days = Enum.GetValues(typeof(Day)).Cast<Day>();

        public FlexitimeWeekData Data { get; set; } = new FlexitimeWeekData();

        public FlexitimeService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public Dictionary<Day, List<string>> Validate()
        {
            return Data.Validate();
        }

        public async Task LoadData()
        {
            foreach (Day day in Days)
            {
                string key = $"data-{day}";
                if (!await _localStorageService.ContainKeyAsync(key))
                    continue;
                FlexitimeDayData dayData = JsonSerializer.Deserialize<FlexitimeDayData>(await _localStorageService.GetItemAsStringAsync(key));
                Data[day] = dayData;
            }
        }

        public async Task SaveDay(FlexitimeDayData dayData)
        {
            await _localStorageService.SetItemAsync($"data-{dayData.Day}", JsonSerializer.Serialize(Data[dayData.Day]));
        }

        public async Task Save()
        {
            foreach (FlexitimeDayData dayData in Data)
            {
                await SaveDay(dayData);
            }
        }
    }
}
