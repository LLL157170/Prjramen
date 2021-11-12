using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjRemenSuperMarket.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace prjRemenSuperMarket.ViewModel
{
    //店家上傳產品照片
    public class CRamenAdd
    {
        public CRamenAdd()
        {
            RamenProductInfo = new RamenProductInfo();
            db = new RamenSupermarketContext();
        }

        public List<CRamenAdd> Get_List(int? storeID)
        {
            List<CRamenAdd> list = new List<CRamenAdd>();
            foreach (RamenProductInfo info in db.RamenProductInfos.Where(row=>row.RamenStoreId == storeID))
            {
                CRamenAdd cRamen = new CRamenAdd();
                cRamen.RamenProductInfo = info;
                list.Add(cRamen);
            }

            return list;
        }

        private RamenSupermarketContext db { get; set; }

        public RamenProductInfo RamenProductInfo { get; set; }

        public IFormFile ProductPicture { get; set; }

        public string ProductPictureBase64
        {
            get
            {
                if (ProductPictureByte != null)
                    return Convert.ToBase64String(ProductPictureByte);

                return "";
            }
        }

        public int RamenProductId
        {
            get => RamenProductInfo.RamenProductId;
            set => RamenProductInfo.RamenProductId = value;
        }

        [DisplayName("店家名稱")]
        [Required(ErrorMessage = ("未輸入"))]
        public int? RamenStoreId
        {
            get => RamenProductInfo.RamenStoreId;
            set => RamenProductInfo.RamenStoreId = value;
        }

        [DisplayName("商品名稱")]
        [Required(ErrorMessage =("未輸入"))]
        public string ProductName
        {
            get => RamenProductInfo.ProductName;
            set => RamenProductInfo.ProductName = value;
        }

        [DisplayName("價格")]
        [Required(ErrorMessage = ("未輸入"))]
        public int? Price
        {
            get => RamenProductInfo.Price;
            set => RamenProductInfo.Price = value;
        }

        [DisplayName("商品圖片")]
        [Required(ErrorMessage =("未輸入"))]
        public byte[] ProductPictureByte
        {
            get => RamenProductInfo.ProductPicture;
            set => RamenProductInfo.ProductPicture = value;
        }
    }
}
