using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.Calender;
using Talabat.Core.Services.Contract;

namespace Talabat.Services
{
    public class ScheduleService: IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsScheduleOverlappingAsync(int doctorId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
        {
            return await _unitOfWork.Repository<WorkSchedule>()
                .AnyAsync(ws => ws.DoctorId == doctorId && ws.Day == day &&
                                ((startTime >= ws.StartTime && startTime < ws.EndTime) ||
                                 (endTime > ws.StartTime && endTime <= ws.EndTime) ||
                                 (startTime <= ws.StartTime && endTime >= ws.EndTime)));
        }
    }
}
