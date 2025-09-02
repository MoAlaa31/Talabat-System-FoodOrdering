using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Calender
{
    public class Doctor: BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Speciality { get; set; } = string.Empty;

        public string? SubSpeciality { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<DoctorPolicy> Policies { get; set; }  // Navigation property for the relationship

        // Nullable - Doctor may not have an active policy initially
        public int? ActivePolicyId { get; set; }
        public DoctorPolicy ActivePolicy { get; set; } // Navigation property

        public int SlotDurationMinutes { get; set; } = 20;     // default is 20 min

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<WorkSchedule> WorkSchedules { get; set; }
        public ICollection<ScheduleException> ScheduleExceptions { get; set; }
    }
}
