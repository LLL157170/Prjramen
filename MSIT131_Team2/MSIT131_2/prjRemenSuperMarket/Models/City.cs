using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
            RamenStores = new HashSet<RamenStore>();
        }

        public int CityIdPk { get; set; }
        public string CityName { get; set; }

        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<RamenStore> RamenStores { get; set; }
    }
}
