using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using prjRemenSuperMarket.Models;

namespace prjRemenSuperMarket.ViewModel
{
    public class CProductDetail
    {
        public CProductDetail()
        {
            Product = new ProductDetail();
            db = new RamenSupermarketContext();
        }

        /// <summary> 取得列表清單 </summary>
        public List<CProductDetail> Get_List(string KeyWord = null, int? CategoryID = null)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            IEnumerable<ProductDetail> products = null;

            if (KeyWord != null)
                products = from p in db.ProductDetails where p.ProductName.Contains(KeyWord) select p;
            else
                products = from p in db.ProductDetails select p;

            List<CProductDetail> list = new List<CProductDetail>();

            foreach (ProductDetail product in products)
            {
                CProductDetail cProduct = new CProductDetail();
                cProduct.Product = product;

                if(CategoryID == null || cProduct.QueryCategoryDetail.CategoryIdFk == CategoryID)
                    list.Add(cProduct);
            }

            return list;
        }

        public ProductDetail Product { get; set; }

        private RamenSupermarketContext db { get; set; }

        // 產品價格
        public SalesInfo salesInfo 
        {
            get;
            set;
        }

        public int? NormalPrice { get; set; }
        public int? SpotPrice { get; set; }
        public int? SpecialOfferPrice { get; set; }

        public int? NormalCount { get; set; }
        public int? SpotCount { get; set; }
        public int? SpecialOfferCount { get; set; }

        public int? NormalSalesInfoId { get; set; }
        public int? SpotSalesInfoId { get; set; }
        public int? SpecialOfferSalesInfoId { get; set; }


        /// <summary> 查詢CategoryDetailIdFk的內容 </summary>
        private CategoryDetail QueryCategoryDetail
        {
            get
            {
                if (Product != null)
                    return db.CategoryDetails.FirstOrDefault(row => row.CategoryDetailIdPk == Product.CategoryDetailIdFk);

                return null;
            }
        }

        /// <summary> 新增、編輯時設置產品圖片(Picture)時使用</summary>
        public IFormFile Photo { get; set; }

        /// <summary> 編輯時調整細項類別(CategoryDetailIdFk)時使用</summary>
        public int? CategoryIdFk
        {
            get
            {
                if (QueryCategoryDetail != null)
                    return db.Categorys.FirstOrDefault(row => row.CategoryIdPk == QueryCategoryDetail.CategoryIdFk).CategoryIdPk;

                return null;
            }
        }

        /// <summary> 列表、編輯時顯示產品圖片時使用</summary>
        public string PictureBase64
        {
            get
            {
                if (Product.Picture != null)
                    return Convert.ToBase64String(Product.Picture);

                return "";
            }
        }

        public int ProductIdPk
        {
            get => Product.ProductIdPk;
            set => Product.ProductIdPk = value;
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("產品種類 / 細類")]
        public string TextCategoryDetail
        {
            get
            {
                if (QueryCategoryDetail != null)
                {
                    string CategoryName = db.Categorys.FirstOrDefault(row => row.CategoryIdPk == QueryCategoryDetail.CategoryIdFk).CategoryName;
                    string CategoryDetailName = db.CategoryDetails.FirstOrDefault(row => row.CategoryDetailIdPk == Product.CategoryDetailIdFk).CategoryDetailName;
                    return CategoryName + " / " + CategoryDetailName;
                }

                return "";
            }
        }


        [DisplayName("圖片")]
        public byte[] Picture
        {
            get => Product.Picture;
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("保存方式")]
        public string Text_Storage
        {
            get
            {
                if (Product.StorageIdFk != null)
                    return db.StorageMethods.FirstOrDefault(row => row.StorageIdPk == Product.StorageIdFk).StorageName;

                return "";
            }
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("瀏覽次數")]
        public int? Views
        {
            get => Product.Views;
            set => Product.Views = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("名稱")]
        [Required(ErrorMessage = "未輸入")]
        public string ProductName
        {
            get => Product.ProductName;
            set => Product.ProductName = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("原物料類別")]
        public int? CategoryDetailIdFk
        {
            get => Product.CategoryDetailIdFk;
            set => Product.CategoryDetailIdFk = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("保存方式")]
        public int? StorageIdFk
        {
            get => Product.StorageIdFk;
            set => Product.StorageIdFk = value;
        }

        [DisplayName("規格")]
        [Required(ErrorMessage = "未輸入")]
        public string Specification
        {
            get => Product.Specification;
            set => Product.Specification = value;
        }

        [DisplayName("重量")]
        [Required(ErrorMessage = "未輸入")]
        public string Weight
        {
            get => Product.Weight;
            set => Product.Weight = value;
        }

        [DisplayName("產地")]
        [Required(ErrorMessage = "未輸入")]
        public string Origin
        {
            get => Product.Origin;
            set => Product.Origin = value;
        }

        [DisplayName("保存天數")]
        [Required(ErrorMessage = "未輸入")]
        public int? Storagedays
        {
            get => Product.Storagedays;
            set => Product.Storagedays = value;
        }

        [DisplayName("產品介紹")]
        public string ProductDescription
        {
            get => Product.ProductDescription;
            set => Product.ProductDescription = value;
        }
    }
}
