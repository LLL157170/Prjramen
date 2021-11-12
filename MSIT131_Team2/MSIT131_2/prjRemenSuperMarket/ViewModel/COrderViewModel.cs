using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class COrderViewModel
    {
        public COrderViewModel corderViewModel { get; set; }
        public int OrderIdPk { get; set; }
        [DisplayName("付款方式")]
        public string Payment { get; set; }//fk
        [DisplayName("訂單日期")]
        public DateTime? OrderDate { get; set; }
        [DisplayName("到貨日期")]
        public DateTime? ShipDate { get; set; }
        [DisplayName("地址")]
        public string ShipAddress { get; set; }
        [DisplayName("區")]
        public string District { get; set; }//fk
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("總價")]
        public decimal? TotalPrice { get; set; }
        [DisplayName("貨物狀態")]
        public string DeliveryStatus { get; set; }//fk
        [DisplayName("會員名稱")]
        public string membername { get; set; }
    }
}
