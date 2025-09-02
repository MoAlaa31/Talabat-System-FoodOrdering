using System.ComponentModel.DataAnnotations;
using Talabat.API.Attributes;
using Talabat.Core.Entities.Calender;

namespace Talabat.API.Dtos
{
    public class BookAppointmentDto
    {
        [ExistingId<Doctor>]
        public int doctorId { get; set; }
        [ExistingId<Patient>]
        public int patientId { get; set; }

        [DataType(DataType.Date)]
        public DateOnly date { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly time { get; set; }
    }

}
