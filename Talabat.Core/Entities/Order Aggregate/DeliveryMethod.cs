using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {
            
        }
        public DeliveryMethod(string shortName, string discription, decimal cost, string deliveryTime)
        {
            ShortName = shortName;
            Discription = discription;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }

        public string ShortName { get; set; }
        public string Discription { get; set; }
        public Decimal Cost { get; set; }
        public string DeliveryTime { get; set; }
    }
}
