using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class DeliveryStatus
    {
        public DeliveryStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int DeliveryStatusIdPk { get; set; }
        public string DeliveryStatus1 { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
