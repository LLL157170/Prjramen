using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            Orders = new HashSet<Order>();
        }

        public int PaymentIdPk { get; set; }
        public string Payment { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
