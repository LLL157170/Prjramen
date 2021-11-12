using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CGoods
    {
        public CGoods()
        {
            Good = new Good();
            db = new RamenSupermarketContext();
        }

        /// <summary> 取得列表清單 </summary>
        public List<CGoods> Get_List(int? ProductID = null, int? CDID = null, int? StateID = null, string KeyWord = null)
        {
            IEnumerable<Good> goods = db.Goods;

            List<CGoods> list = new List<CGoods>();

            foreach (Good good in goods)
            {
                CGoods cGoods = new CGoods
                {
                    Good = good,
                    QueryProduct = db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == good.ProductIdFk),

                };

                if (Filter_List_Detail(cGoods, ProductID, CDID, StateID))
                    list.Add(cGoods);
            }

            if (KeyWord != null)
                list = list.Where(row => row.QueryProduct.ProductName.Contains(KeyWord)).ToList();

            return list;
        }

        private bool Filter_List_Detail(CGoods cGoods, params int?[] filterID)
        {
            int index = 0;

            //檢查所有篩選條件
            foreach (int? id in filterID)
            {
                index++;

                //False時結束迴圈並回傳(表示該筆資料不加入列表)
                if (!Detail_Filter_Conditions(cGoods, id, index))
                    return false;
            }

            return true;
        }

        private bool Detail_Filter_Conditions(CGoods cGoods, int? id, int index)
        {
            //篩選編號(id為null時指搜尋全部)
            //1.指定ProductID
            //2.指定CategoryDetailID
            //3.指定ProductStatusID

            return index switch
            {
                1 => id == null || cGoods.ProductIdFk == id,
                2 => id == null || cGoods.QueryProduct.CategoryDetailIdFk == id,
                3 => id == null || cGoods.Good.ProductStatusIdFk == id,
                _ => false //default
            };

        }

        /// <summary> </summary>
        public List<CGoods> Get_Not_Expired_List(int ProductID, int? GoodsID, int? SalesStateID)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            int? StateID = null;

            if (SalesStateID == 1) StateID = 1;
            if (SalesStateID == 3) StateID = 2;

            IEnumerable<Good> goods;
            //取得符合搜尋條件ProductID且未過期且數量大於0貨物，並按照日期升冪排序
            goods = db.Goods.Where(row => row.ProductIdFk == ProductID && row.ProductStatusIdFk != 3 && row.Counts > 0)
                            .OrderBy(row => row.ShelfLife);

            List<CGoods> list = new List<CGoods>();

            foreach (Good good in goods)
            {
                CGoods cGoods = new CGoods();

                //如果篩選條件GoodsID有值(指定不排除的上架貨物(編輯販售資訊時))
                if (good.GoodsIdPk != GoodsID)
                {
                    //如果該貨物已被上架，不加入到列表中
                    if (db.SalesInfos.FirstOrDefault(row => row.GoodsIdFk == good.GoodsIdPk && row.SalesStatesIdFk != 5) != null) continue;
                }

                //如果狀態不同，不加入到列表中
                if (StateID != null && good.ProductStatusIdFk != StateID) continue;


                cGoods.Good = good;
                list.Add(cGoods);
            }

            return list;
        }

        public ProductDetail QueryProduct { get; set; }

        public Good Good { get; set; }

        private RamenSupermarketContext db { get; set; }

        public int GoodsIdPk
        {
            get => Good.GoodsIdPk;
            set => Good.GoodsIdPk = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("進貨產品")]
        [Required(ErrorMessage = "未輸入")]
        public int? ProductIdFk
        {
            get => Good.ProductIdFk;
            set => Good.ProductIdFk = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("進貨廠商")]
        public int? MerchantIdFk
        {
            get => Good.MerchantIdFk;
            set => Good.MerchantIdFk = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("數量")]
        [Required(ErrorMessage = "未輸入")]
        [Range(1, 1000, ErrorMessage = "請輸入1~1000以內的數量")]
        public int? Counts
        {
            get => Good.Counts;
            set => Good.Counts = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("進貨價格")]
        [Required(ErrorMessage = "未輸入")]
        [Range(1, 10000, ErrorMessage = "請輸入正整數")]
        public decimal? PurchaseCost
        {
            get => Good.PurchaseCost;
            set => Good.PurchaseCost = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("進貨日期")]
        [Required(ErrorMessage = "未輸入")]
        public DateTime? PurchaseDate
        {
            get => Good.PurchaseDate;
            set => Good.PurchaseDate = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("保存日期")]
        [Required(ErrorMessage = "未輸入")]
        public DateTime? ShelfLife
        {
            get => Good.ShelfLife;
            set => Good.ShelfLife = value;
        }

        /// <summary> 新增、修改使用 </summary>
        [DisplayName("上架日期")]
        [Required(ErrorMessage = "未輸入")]
        public DateTime? LaunchDate
        {
            get => Good.LaunchDate;
            set => Good.LaunchDate = value;
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("產品名稱")]
        public string ProductName
        {
            get
            {
                if (Good.ProductIdFk != null)
                    return db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == Good.ProductIdFk).ProductName;

                return "";
            }
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("員工名稱")]
        public string EmployeeName
        {
            get
            {
                if (Good.EmployeeIdFk != null)
                    return db.Employees.FirstOrDefault(row => row.EmployeeIdPk == Good.EmployeeIdFk).Name;

                return "";
            }
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("供貨廠商")]
        public string MerchantName
        {
            get
            {
                if (Good.MerchantIdFk != null)
                    return db.Merchants.FirstOrDefault(row => row.MerchantIdPk == Good.MerchantIdFk).MerchantName;

                return "";
            }
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("進貨日期")]
        public string Text_PurchaseDate
        {
            get => ((DateTime)Good.PurchaseDate).ToString("yyyy-MM-dd");
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("保存日期")]
        public string Text_ShelfLife
        {
            get => ((DateTime)Good.ShelfLife).ToString("yyyy-MM-dd");
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("上架日期")]
        public string Text_LaunchDate
        {
            get
            {
                if (Good.LaunchDate != null)
                    return ((DateTime)Good.LaunchDate).ToString("yyyy-MM-dd");

                return "-";
            }
        }

        /// <summary> 列表使用 </summary>
        [DisplayName("庫存狀態")]
        public string ProductStatus
        {
            get
            {
                if (Good.ProductStatusIdFk != null)
                    return db.ProductStatuses.FirstOrDefault(row => row.ProductStatusIdPk == Good.ProductStatusIdFk).ProductStatus1;

                return "";
            }
        }
    }
}
