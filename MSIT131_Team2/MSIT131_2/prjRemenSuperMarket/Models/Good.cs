using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Good
    {
        public Good()
        {
            SalesInfos = new HashSet<SalesInfo>();
        }

        public int GoodsIdPk { get; set; }
        public int? ProductIdFk { get; set; }
        public int? EmployeeIdFk { get; set; }
        public int? MerchantIdFk { get; set; }
        public int? Counts { get; set; }
        public decimal? PurchaseCost { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ShelfLife { get; set; }
        public DateTime? LaunchDate { get; set; }
        public int? ProductStatusIdFk { get; set; }

        public virtual Employee EmployeeIdFkNavigation { get; set; }
        public virtual Merchant MerchantIdFkNavigation { get; set; }
        public virtual ProductDetail ProductIdFkNavigation { get; set; }
        public virtual ProductStatus ProductStatusIdFkNavigation { get; set; }
        public virtual ICollection<SalesInfo> SalesInfos { get; set; }
    }
}
