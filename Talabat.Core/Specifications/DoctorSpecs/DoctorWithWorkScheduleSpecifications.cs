using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Calender;

namespace Talabat.Core.Specifications.DoctorSpecs
{
    public class DoctorWithWorkScheduleSpecifications: BaseSpecifications<Doctor>
    {
        public DoctorWithWorkScheduleSpecifications(int id)
            :base(d =>
                d.Id == id
            )
        {
            Includes.Add(d => d.WorkSchedules);
        }
    }
}
