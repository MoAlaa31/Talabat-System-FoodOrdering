using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Calender;

namespace Talabat.Core.Services.Contract
{
    public interface IScheduleService
    {
        public Task<bool> IsScheduleOverlappingAsync(int doctorId, DayOfWeek day, TimeOnly startTime, TimeOnly endTime);

    }
}
