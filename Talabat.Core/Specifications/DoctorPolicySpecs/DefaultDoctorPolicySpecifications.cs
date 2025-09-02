using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Calender;

namespace Talabat.Core.Specifications.DoctorPolicySpecs
{
    public class DefaultDoctorPolicySpecifications: BaseSpecifications<DoctorPolicy>
    {
        public DefaultDoctorPolicySpecifications()
            : base(dp =>
                 dp.IsDefault
            )
        {

        }
    }
}
