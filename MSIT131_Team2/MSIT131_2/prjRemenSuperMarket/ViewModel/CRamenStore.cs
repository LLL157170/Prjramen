using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CRamenStore
    {
        public RamenStore ramenStore { get; set; }
        
        public CRamenStore()
        {
            RamenProductInfos = new HashSet<RamenProductInfo>();
            ramenStore = new RamenStore();
            
        }
        RamenSupermarketContext db = new RamenSupermarketContext();
        public string DistrictName { get { return db.Districts.Find(DistrictId).DistrictName; } }
        public string CityName { get { return db.Citys.Find(CityId).CityName; } }
        public int RamenStoreId { get { return ramenStore.RamenStoreId; } set { ramenStore.RamenStoreId = value; } }
        public string StoreName { get { return ramenStore.StoreName; } set { ramenStore.StoreName = value; } }
        public int? MemberId { get { return ramenStore.MemberId; } set { ramenStore.MemberId = value; } }
        public string Introduce { get { return ramenStore.Introduce; } set { ramenStore.Introduce = value; } }
        public byte[] Pictrue { get { return ramenStore.Pictrue; } set { ramenStore.Pictrue = value; } }
        public byte[] Logo { get { return ramenStore.Logo; } set { ramenStore.Logo = value; } }
        public int? CityId { get { return ramenStore.CityId; } set { ramenStore.CityId = value; } }
        public int? DistrictId { get { return ramenStore.DistrictId; } set { ramenStore.DistrictId = value; } }
        public string Address { get { return ramenStore.Address; } set { ramenStore.Address = value; } }
        public string Phone { get { return ramenStore.Phone; } set { ramenStore.Phone = value; } }

        public string LogoBase64
        {
            get
            {
                if (Logo != null)
                    return Convert.ToBase64String(Logo);

                return "";
            }
        }

        public string PictrueBase64
        {
            get
            {
                if (Pictrue != null)
                    return Convert.ToBase64String(Pictrue);
                return "";
            }
        }
        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<RamenProductInfo> RamenProductInfos { get; set; }

        
    }
}

