using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class PriceFactor
    {
        public PriceFactor()
        {
            SalesInfos = new HashSet<SalesInfo>();
        }

        public int PriceFactorIdPk { get; set; }
        public double? PriceFactor1 { get; set; }

        public virtual ICollection<SalesInfo> SalesInfos { get; set; }
    }
}
