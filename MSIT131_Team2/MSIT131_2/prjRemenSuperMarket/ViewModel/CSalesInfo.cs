using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using prjRemenSuperMarket.Models;

namespace prjRemenSuperMarket.ViewModel
{
    public class CSalesInfo
    {
        public CSalesInfo()
        {
            SalesInfo = new SalesInfo();
            db = new RamenSupermarketContext();
        }

        public enum ListStates
        {
            All = 0,
            Current = 1,
            Expired = 2,
            Current_Distinct = 3,
            spotitem = 4,
            spitem = 5,
            unnormalitem = 6
        }

        public List<CSalesInfo> Get_List_By_CategoryDetails_Binary(long? CDID_Binary, string KeyWord, int? State_Binary = 2, ListStates itemstate = ListStates.Current_Distinct)
        {
            IEnumerable<SalesInfo> salesInfos = Filter_List(itemstate);
            salesInfos = salesInfos.OrderBy(row => row.ProductIdFk);

            List<CSalesInfo> list = new List<CSalesInfo>();

            foreach (SalesInfo salesInfo in salesInfos)
            {
                CSalesInfo cSalesInfo = new CSalesInfo();
                cSalesInfo.SalesInfo = salesInfo;
                cSalesInfo.QueryGood = db.Goods.FirstOrDefault(row => row.GoodsIdPk == salesInfo.GoodsIdFk);
                cSalesInfo.QueryProduct = db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == salesInfo.ProductIdFk);
                cSalesInfo.QueryCategoryDetail = db.CategoryDetails.FirstOrDefault(row => row.CategoryDetailIdPk == cSalesInfo.QueryProduct.CategoryDetailIdFk);

                long CDID_Bin = ((long)Math.Pow(2 ,(double)cSalesInfo.QueryProduct.CategoryDetailIdFk));
                int State_Bin = (int)Math.Pow(2, (double)salesInfo.SalesStatesIdFk);

                if(State_Binary == 2 || ((State_Binary & State_Bin) == State_Bin && salesInfo.Counts > 0))
                    if (CDID_Binary == null || (CDID_Binary & CDID_Bin) == CDID_Bin)
                        list.Add(cSalesInfo);
            }

            if (KeyWord != null)
                list = list.Where(row => row.QueryProduct.ProductName.Contains(KeyWord)).ToList();

            return list;
        }

        public List<CSalesInfo> Get_List(int? ProductID = null, int? CategoryID = null, int? CategoryDetailID = null, int? StateID = null,
                                         string KeyWord = null, ListStates state = ListStates.All)
        {
            IEnumerable<SalesInfo> salesInfos = Filter_List(state);

            List<CSalesInfo> list = new List<CSalesInfo>();

            foreach (SalesInfo salesInfo in salesInfos)
            {
                CSalesInfo cSalesInfo = new CSalesInfo();
                cSalesInfo.SalesInfo = salesInfo;
                cSalesInfo.QueryGood = db.Goods.FirstOrDefault(row => row.GoodsIdPk == salesInfo.GoodsIdFk);
                cSalesInfo.QueryProduct = db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == salesInfo.ProductIdFk);
                cSalesInfo.QueryCategoryDetail = db.CategoryDetails.FirstOrDefault(row => row.CategoryDetailIdPk == cSalesInfo.QueryProduct.CategoryDetailIdFk);

                //ID篩選條件
                if (Filter_List_Detail(cSalesInfo, ProductID, CategoryID, StateID, CategoryDetailID))
                    list.Add(cSalesInfo);
            }

            if (KeyWord != null)
                list = list.Where(row => row.QueryProduct.ProductName.Contains(KeyWord)).ToList();

            return list;
        }

        //篩選要顯示的列表狀態
        private IEnumerable<SalesInfo> Filter_List(ListStates state)
        {
            //篩選條件
            //All: 傳回所有列表
            //Current: 傳回上架的列表
            //Expired: 傳回下架的列表
            //Current_Distinct: 傳回上架但產品不重複的列表

            return state switch
            {
                ListStates.Current => db.SalesInfos.Where(row => row.SalesStatesIdFk != 5),
                ListStates.Expired => db.SalesInfos.Where(row => row.SalesStatesIdFk == 5),
                ListStates.Current_Distinct => db.SalesInfos.Where(row => row.SalesStatesIdFk != 5).DistinctBy(row => row.ProductIdFk),
                ListStates.spotitem => db.SalesInfos.Where(row => row.SalesStatesIdFk == 3).Where(row => row.Counts>0).DistinctBy(row => row.ProductIdFk),
                ListStates.spitem => db.SalesInfos.Where(row => row.SalesStatesIdFk == 2).Where(row => row.Counts > 0).DistinctBy(row => row.ProductIdFk),
                ListStates.unnormalitem => db.SalesInfos.Where(row => row.SalesStatesIdFk == 2 || row.SalesStatesIdFk == 3).Where(row => row.Counts > 0).DistinctBy(row => row.ProductIdFk),

                /*ListStates.All*/
                _ => db.SalesInfos
            };
        }

        private bool Filter_List_Detail(CSalesInfo cSalesInfo, params int?[] filterID)
        {
            int index = 0;

            //檢查所有篩選條件
            foreach (int? id in filterID)
            {
                index++;

                //False時結束迴圈並回傳(表示該筆資料不加入列表)
                if (!Detail_Filter_Conditions(cSalesInfo, id, index))
                    return false;
            }

            return true;
        }

        private bool Detail_Filter_Conditions(CSalesInfo cSalesInfo, int? id, int index)
        {
            //篩選編號(id為null時指搜尋全部)
            //1.指定ProductID
            //2.指定CategoryIdFk
            //3.指定SalesStatesIdFk

            return index switch
            {
                1 => id == null || cSalesInfo.QueryProduct.ProductIdPk == id,
                2 => id == null || cSalesInfo.QueryCategoryDetail.CategoryIdFk == id,
                3 => id == null || cSalesInfo.SalesInfo.SalesStatesIdFk == id,
                4 => id == null || cSalesInfo.QueryCategoryDetail.CategoryDetailIdPk == id,
                _ => true //default
            };

        }

        public ProductDetail QueryProduct { get; set; }

        public CategoryDetail QueryCategoryDetail { get; set; }

        public Good QueryGood { get; set; }

        public SalesInfo SalesInfo { get; set; }

        private RamenSupermarketContext db { get; set; }

        public string PriceInterval { get; set; }

        public int SalesInfoIdPk
        {
            get => SalesInfo.SalesInfoIdPk;
            set => SalesInfo.SalesInfoIdPk = value;
        }

        [DisplayName("上架產品")]
        public int? ProductIdFk
        {
            get => SalesInfo.ProductIdFk;
            set => SalesInfo.ProductIdFk = value;
        }

        public int? GoodsIdFk
        {
            get => SalesInfo.GoodsIdFk;
            set => SalesInfo.GoodsIdFk = value;
        }

        [DisplayName("照片")]
        public string PictureBase64
        {
            get
            {
                if (QueryProduct.Picture != null)
                {
                    return Convert.ToBase64String(QueryProduct.Picture);
                }
                return "";
            }

        }

        [DisplayName("售價係數")]
        public int? PriceFactorFk
        {
            get => SalesInfo.PriceFactorFk;
            set => SalesInfo.PriceFactorFk = value;
        }

        [DisplayName("單價")]
        public decimal? UnitPrice
        {
            get => SalesInfo.UnitPrice;
            set => SalesInfo.UnitPrice = value;
        }

        [DisplayName("數量")]
        public int? Counts
        {
            get => SalesInfo.Counts;
            set => SalesInfo.Counts = value;
        }

        [DisplayName("折扣")]
        public int? DiscountIdFk
        {
            get => SalesInfo.DiscountIdFk;
            set => SalesInfo.DiscountIdFk = value;
        }

        [DisplayName("販售狀態")]
        public int? SalesStatesIdFk
        {
            get => SalesInfo.SalesStatesIdFk;
            set => SalesInfo.SalesStatesIdFk = value;
        }

        [DisplayName("貨物名稱")]
        public string GoodName
        {
            get
            {
                if (SalesInfo.ProductIdFk != null)
                    return db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == SalesInfo.ProductIdFk).ProductName;
                return "-";
            }
        }

        [DisplayName("售價係數")]
        public string PriceFactor
        {
            get
            {
                if (SalesInfo.PriceFactorFk != null)
                    return db.PriceFactors.FirstOrDefault(row => row.PriceFactorIdPk == SalesInfo.PriceFactorFk).PriceFactor1.ToString();

                return "-";
            }
        }

        [DisplayName("折扣")]
        public double? Discount
        {
            get
            {
                if (SalesInfo.DiscountIdFk != null)
                    return db.Discounts.FirstOrDefault(row => row.DiscountIdPk == SalesInfo.DiscountIdFk).DiscountFactor;

                return null;
            }
        }

        [DisplayName("販售狀態")]
        public string SalesStates
        {
            get
            {
                if (SalesInfo.SalesStatesIdFk != null)
                    return db.SalesStates.FirstOrDefault(row => row.SalesStatesIdPk == SalesInfo.SalesStatesIdFk).SalesStates;
                return "-";
            }
        }



    }
}
