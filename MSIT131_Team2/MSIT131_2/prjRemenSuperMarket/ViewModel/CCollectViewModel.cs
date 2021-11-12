using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CCollectViewModel
    {
        public CCollectViewModel collectViewModel { get; set; }
        public int CollectIdPk { get; set; }
        public int ProductInfoIFk { get; set; }
        [DisplayName("商品名稱")]
        public string Product { get; set; }
        [DisplayName("商品圖片")]
        public string PictureBase64{ get; set; }
        [DisplayName("收藏類別")]
        public string CollectType { get; set; }
    }
}
