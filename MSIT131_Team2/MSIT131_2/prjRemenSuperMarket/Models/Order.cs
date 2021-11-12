using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderIdPk { get; set; }
        public int? MemberIdFk { get; set; }
        public int? PaymentFk { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public string ShipAddress { get; set; }
        public int? DistrictIdFk { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? DeliveryStatusIdFk { get; set; }

        public virtual DeliveryStatus DeliveryStatusIdFkNavigation { get; set; }
        public virtual District DistrictIdFkNavigation { get; set; }
        public virtual Member MemberIdFkNavigation { get; set; }
        public virtual PaymentType PaymentFkNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
