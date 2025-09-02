using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Calender
{
    public class WorkSchedule: BaseEntity
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DayOfWeek Day { get; set; } // e.g., Monday, Tuesday...

        public TimeOnly StartTime { get; set; } // e.g., 09:00 AM
        public TimeOnly EndTime { get; set; } // e.g., 05:00 PM

    }

}
