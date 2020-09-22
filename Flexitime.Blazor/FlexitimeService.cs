    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flexitime.Blazor
{
    public class FlexitimeService
    {
        public readonly IEnumerable<Day> Days = Enum.GetValues(typeof(Day)).Cast<Day>();

        public FlexitimeWeekData Data = new FlexitimeWeekData();

        public Dictionary<Day, List<string>> Validate()
        {
            return Data.Validate();
        }
    }
}
