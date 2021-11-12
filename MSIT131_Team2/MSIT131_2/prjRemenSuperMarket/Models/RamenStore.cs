using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class RamenStore
    {
        public RamenStore()
        {
            RamenProductInfos = new HashSet<RamenProductInfo>();
            RamenStoreCollects = new HashSet<RamenStoreCollect>();
        }

        public int RamenStoreId { get; set; }
        public string StoreName { get; set; }
        public int? MemberId { get; set; }
        public string Introduce { get; set; }
        public byte[] Pictrue { get; set; }
        public byte[] Logo { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<RamenProductInfo> RamenProductInfos { get; set; }
        public virtual ICollection<RamenStoreCollect> RamenStoreCollects { get; set; }
    }
}
