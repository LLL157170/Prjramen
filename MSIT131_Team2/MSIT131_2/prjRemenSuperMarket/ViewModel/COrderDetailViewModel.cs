using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class COrderDetailViewModel
    {       
        public int OrderDetailsIdPk { get; set; }
        [DisplayName("訂單編號")]
        public int? OrderIdFk { get; set; }
        [DisplayName("商品名稱")]
        public string SalesInfo { get; set; }
        [DisplayName("商品圖片")]
        public string PictureBase64 { get; set; }
        [DisplayName("商品數量")]
        public int? Counts { get; set; }
        [DisplayName("總計")]
        public decimal? Subtotal { get; set; }
    }
}
