using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class District
    {
        public District()
        {
            Members = new HashSet<Member>();
            Orders = new HashSet<Order>();
            RamenStores = new HashSet<RamenStore>();
        }

        public int DistrictIdPk { get; set; }
        public int? CityIdFk { get; set; }
        public string DistrictName { get; set; }

        public virtual City CityIdFkNavigation { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<RamenStore> RamenStores { get; set; }
    }
}
