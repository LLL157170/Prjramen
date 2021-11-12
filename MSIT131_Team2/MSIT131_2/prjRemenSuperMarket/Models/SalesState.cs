using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class SalesState
    {
        public SalesState()
        {
            SalesInfos = new HashSet<SalesInfo>();
        }

        public int SalesStatesIdPk { get; set; }
        public string SalesStates { get; set; }

        public virtual ICollection<SalesInfo> SalesInfos { get; set; }
    }
}
