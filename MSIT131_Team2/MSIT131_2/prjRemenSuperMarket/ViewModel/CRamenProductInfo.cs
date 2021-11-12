using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Views.RamenStore
{
    public class CRamenProductInfo
    {
        public RamenProductInfo ramenproductinfo { get; set; }
        public CRamenProductInfo()
        {
            ramenproductinfo = new RamenProductInfo();
        }
        public int RamenProductId { get { return ramenproductinfo.RamenProductId; } set { ramenproductinfo.RamenProductId = value; } }
        public int? RamenStoreId { get { return ramenproductinfo.RamenStoreId; } set { ramenproductinfo.RamenStoreId = value; } }
        public string ProductName { get { return ramenproductinfo.ProductName; } set { ramenproductinfo.ProductName = value; } }
        public byte[] ProductPicture { get { return ramenproductinfo.ProductPicture; } set { ramenproductinfo.ProductPicture = value; } }
        public string ProductPictureBase64
        {
            get
            {
                if (ProductPicture != null)
                    return Convert.ToBase64String(ProductPicture);

                return "";
            }
        }
        public int? Price { get { return ramenproductinfo.Price; } set { ramenproductinfo.Price = value; } }
    }
}
