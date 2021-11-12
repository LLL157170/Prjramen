using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CRamenStoreCollect
    {
        public CRamenStoreCollect storeCollectViewModel { get; set; }
        public int StoreCollectId { get; set; }
        public int StoreId { get; set; }
        [DisplayName("店家名稱")]
        public string StoreName { get; set; }
        [DisplayName("店家圖片")]
        public string PictureBase64 { get; set; }
        [DisplayName("電話")]
        public string Phone { get; set; }        
        public string City { get; set; }
        public string District { get; set; }
        [DisplayName("地址")]
        public string Address { get; set; }        
    }
}
