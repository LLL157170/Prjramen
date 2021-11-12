using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailsIdPk { get; set; }
        public int? OrderIdFk { get; set; }
        public int? SalesInfoIdFk { get; set; }
        public int? Counts { get; set; }
        public decimal? Subtotal { get; set; }

        public virtual Order OrderIdFkNavigation { get; set; }
        public virtual SalesInfo SalesInfoIdFkNavigation { get; set; }
    }
}
