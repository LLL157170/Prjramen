using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Collect
    {
        public int CollectIdPk { get; set; }
        public int? MemberIdFk { get; set; }
        public int? ProductIdFk { get; set; }
        public int? CollectTypeIdFk { get; set; }

        public virtual CollectType CollectTypeIdFkNavigation { get; set; }
        public virtual Member MemberIdFkNavigation { get; set; }
        public virtual ProductDetail ProductIdFkNavigation { get; set; }
    }
}
