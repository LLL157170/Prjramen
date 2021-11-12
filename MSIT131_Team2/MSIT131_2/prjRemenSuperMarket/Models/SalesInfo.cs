using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class SalesInfo
    {
        public SalesInfo()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int SalesInfoIdPk { get; set; }
        public int? ProductIdFk { get; set; }
        public int? GoodsIdFk { get; set; }
        public int? PriceFactorFk { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? Counts { get; set; }
        public int? DiscountIdFk { get; set; }
        public int? SalesStatesIdFk { get; set; }

        public virtual Discount DiscountIdFkNavigation { get; set; }
        public virtual Good GoodsIdFkNavigation { get; set; }
        public virtual PriceFactor PriceFactorFkNavigation { get; set; }
        public virtual ProductDetail ProductIdFkNavigation { get; set; }
        public virtual SalesState SalesStatesIdFkNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
