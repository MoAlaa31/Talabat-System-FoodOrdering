using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Calender;

namespace Talabat.Core.Specifications.DoctorPolicySpecs
{
    public class DoctorPolicySpecifications: BaseSpecifications<DoctorPolicy>
    {
        public DoctorPolicySpecifications(int doctorId)
            :base(dp =>
                dp.DoctorId == doctorId
            )
        {
            
        }
    }
}
