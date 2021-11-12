using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class RamenProductInfo
    {
        public int RamenProductId { get; set; }
        public int? RamenStoreId { get; set; }
        public string ProductName { get; set; }
        public byte[] ProductPicture { get; set; }
        public int? Price { get; set; }

        public virtual RamenStore RamenStore { get; set; }
    }
}
