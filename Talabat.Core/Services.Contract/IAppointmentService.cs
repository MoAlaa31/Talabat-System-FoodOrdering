using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Common;
using Talabat.Core.Entities.Calender;

namespace Talabat.Core.Services.Contract
{
    public interface IAppointmentService
    {
        //public List<TimeOnly> GenerateTimeSlots(WorkSchedule schedule, int slotDurationMinutes);
        //public Task<Dictionary<DayOfWeek, List<TimeSpan>>> GetAvailableSlots(int doctorId, int slotDurationMinutes, DateTime selectedWeek);
        public Task<ServiceResult<Dictionary<DateOnly, List<TimeOnly>>>> GetAvailableSlotsAsync(Doctor doctor);
    }
}
