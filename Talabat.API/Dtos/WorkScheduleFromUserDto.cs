using System.ComponentModel.DataAnnotations;
using Talabat.API.Attributes;
using Talabat.Core.Entities.Calender;

namespace Talabat.API.Dtos
{
    public class WorkScheduleFromUserDto
    {
        [ExistingId<Doctor>]
        public int doctorId { get; set; }

        [ValidEnumValue<DayOfWeek>(ErrorMessage = "Invalid value for DayOfWeek.")]
        public DayOfWeek Day { get; set; } // e.g., Monday, Tuesday...

        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; } // e.g., 09:00 AM

        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; } // e.g., 05:00 PM
    }
}
