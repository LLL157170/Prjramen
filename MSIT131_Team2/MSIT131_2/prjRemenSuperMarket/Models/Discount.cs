using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Discount
    {
        public Discount()
        {
            SalesInfos = new HashSet<SalesInfo>();
        }

        public int DiscountIdPk { get; set; }
        public string DiscountName { get; set; }
        public double? DiscountFactor { get; set; }

        public virtual ICollection<SalesInfo> SalesInfos { get; set; }
    }
}
