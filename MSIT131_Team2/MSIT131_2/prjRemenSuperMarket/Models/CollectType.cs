using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class CollectType
    {
        public CollectType()
        {
            Collects = new HashSet<Collect>();
        }

        public int CollectTypeIdPk { get; set; }
        public string CollectType1 { get; set; }

        public virtual ICollection<Collect> Collects { get; set; }
    }
}
