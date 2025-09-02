using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Calender;

namespace Talabat.Core.Specifications.WorkScheduleSpecs
{
    public class WorkSheduleSpecifications: BaseSpecifications<WorkSchedule>
    {
        public WorkSheduleSpecifications(int doctorId):base( ws => ws.DoctorId == doctorId)
        {
            
        }
    }
}
