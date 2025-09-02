using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Calender
{
    public class ScheduleException: BaseEntity
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly? StartTime { get; set; } // e.g., 09:00 AM
        public TimeOnly? EndTime { get; set; } // e.g., 05:00 PM
        public bool IsAvailable { get; set; } // False means the doctor is unavailable
    }
}
