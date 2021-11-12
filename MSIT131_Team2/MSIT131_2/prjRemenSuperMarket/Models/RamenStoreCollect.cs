using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class RamenStoreCollect
    {
        public int CollectId { get; set; }
        public int? MemberId { get; set; }
        public int? StoreId { get; set; }

        public virtual Member Member { get; set; }
        public virtual RamenStore Store { get; set; }
    }
}
