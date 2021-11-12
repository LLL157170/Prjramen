using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class StorageMethod
    {
        public StorageMethod()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public int StorageIdPk { get; set; }
        public string StorageName { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
