using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Calender;

namespace Talabat.Core.Specifications.AppointmentSpecs
{
    public class AppointmentSpecifications: BaseSpecifications<Appointment>
    {
        public AppointmentSpecifications(int doctorId, DateOnly startDate, int daysToInclude = 7)
            : base(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate >= startDate &&
                a.AppointmentDate < startDate.AddDays(daysToInclude)
            )
        {
            // Constructor logic remains the same, but it's more flexible now.
            // `startDate` specifies the start of the date range (e.g., for a specific week).
            // `daysToInclude` can now be customized (default is 7 days).
        }
    }
}
