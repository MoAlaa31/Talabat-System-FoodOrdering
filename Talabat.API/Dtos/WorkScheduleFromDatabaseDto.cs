using System.ComponentModel.DataAnnotations;
using Talabat.API.Attributes;
using Talabat.Core.Entities.Calender;

namespace Talabat.API.Dtos
{
    public class WorkScheduleFromDatabaseDto
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; } // e.g., Monday, Tuesday...
        public TimeOnly StartTime { get; set; } // e.g., 09:00 AM
        public TimeOnly EndTime { get; set; } // e.g., 05:00 PM
    }
}
