using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class ProductStatus
    {
        public ProductStatus()
        {
            Goods = new HashSet<Good>();
        }

        public int ProductStatusIdPk { get; set; }
        public string ProductStatus1 { get; set; }

        public virtual ICollection<Good> Goods { get; set; }
    }
}
