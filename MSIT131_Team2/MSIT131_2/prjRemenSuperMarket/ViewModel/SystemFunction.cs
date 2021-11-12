using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using prjRemenSuperMarket.Models;

namespace prjRemenSuperMarket.ViewModel
{
    public static class SystemFunction
    {
        /// <summary> 使販售商品下架 </summary>
        public static void Off_Shelf(this int SalesInfoID)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            db.SalesInfos.FirstOrDefault(row => row.SalesInfoIdPk == SalesInfoID).SalesStatesIdFk = 5;
            db.SaveChanges();
        }

        /// <summary> 檢查該販售商品有無訂單，如果有則下架該商品(保留記錄) </summary>
        public static bool Check_Has_Order_Then_Off_Shelf(this int SalesInfoID)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            if (db.OrderDetails.FirstOrDefault(row => row.SalesInfoIdFk == SalesInfoID) != null)
            {
                SalesInfoID.Off_Shelf();
                return true;
            }

            return false;
        }

        /// <summary>  檢查顧客有無建立拉麵店，如果有則視為VIP </summary>
        public static bool Check_Member_Is_VIP(this Member member)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            if (db.RamenStores.FirstOrDefault(row => row.MemberId == member.MemberIdPk) != null)
                return true;

            return false;
        }

        /// <summary>  依訂單扣除販售資訊及貨物的數量 </summary>
        public static void Decrease_Goods_Count_By_Orders(this Order order)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            foreach (OrderDetail item in db.OrderDetails.Where(row => row.OrderIdFk == order.OrderIdPk))
            {
                SalesInfo salesInfo = db.SalesInfos.FirstOrDefault(row => row.SalesInfoIdPk == item.SalesInfoIdFk);
                Good goods = db.Goods.FirstOrDefault(row => row.GoodsIdPk == salesInfo.GoodsIdFk);

                salesInfo.Counts -= item.Counts;
                goods.Counts -= item.Counts;
            }

            db.SaveChanges();
        }

        /// <summary> 設置所有販售資訊商品的價格區間 </summary>
        public static void Set_All_SalesInfos_Price_Interval(this IEnumerable<CSalesInfo> cSalesInfos)
        {
            foreach (CSalesInfo item in cSalesInfos)
            {
                item.Set_SalesInfo_Price_Interval();
            }
        }

        /// <summary> 設置產品價格區間 </summary>
        private static void Set_SalesInfo_Price_Interval(this CSalesInfo cSalesInfo)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            IEnumerable<SalesInfo> All_OnShelf = db.SalesInfos.Where(row => row.ProductIdFk == cSalesInfo.ProductIdFk && row.SalesStatesIdFk != 5 && row.Counts > 0)
                                                              .OrderByDescending(row => row.UnitPrice);
            int? Max = null;
            int? Min = null;

            if(All_OnShelf.Count() > 0)
            {
                Max = (int?)All_OnShelf.First().UnitPrice;
                Min = (int?)All_OnShelf.Last().UnitPrice;
            }

            //if (All_OnShelf.First().Counts == 0 || All_OnShelf.First().Counts == null)
            //    Max = null;

            //if (All_OnShelf.Last().Counts == 0 || All_OnShelf.Last().Counts == null)
            //    Min = null;

            switch (Max)
            {
                case null: cSalesInfo.PriceInterval = "缺貨中"; break;
                case int i when i == Min: cSalesInfo.PriceInterval = "$" + Max; break;
                case int i when i == Max && Min == null: cSalesInfo.PriceInterval = "$" + Max; break;
                default: cSalesInfo.PriceInterval = $"${Max} -> ${Min}"; break;
            }
        }

        /// <summary> 設置產品各個狀態價錢、數量、ID </summary>
        public static void Set_Product_Info_ByStates(this CProductDetail cProduct)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            IEnumerable<SalesInfo> All_OnShelf = db.SalesInfos.Where(row => row.ProductIdFk == cProduct.ProductIdPk && row.SalesStatesIdFk != 5)
                                                              .OrderByDescending(row => row.UnitPrice);

            cProduct.NormalPrice = (int?)All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 1)?.UnitPrice;
            cProduct.SpotPrice = (int?)All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 3)?.UnitPrice;
            cProduct.SpecialOfferPrice = (int?)All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 2)?.UnitPrice;

            cProduct.NormalCount = (All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 1)?.Counts);
            cProduct.SpotCount = (All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 3)?.Counts);
            cProduct.SpecialOfferCount = (All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 2)?.Counts);

            cProduct.NormalSalesInfoId = (All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 1)?.SalesInfoIdPk);
            cProduct.SpotSalesInfoId = (All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 3)?.SalesInfoIdPk);
            cProduct.SpecialOfferSalesInfoId = (All_OnShelf.FirstOrDefault(row => row.SalesStatesIdFk == 2)?.SalesInfoIdPk);
        }

        /// <summary> 將購物車中所有非正常販售或VIP正常價的產品名稱加上狀態註記 </summary>
        public static void Add_ProductState_To_All_ProductNames(this IEnumerable<CShoppingCart> cShoppingCart,bool IsVIP)
        {
            foreach (CShoppingCart item in cShoppingCart)
            {
                item.Add_ProductState_To_ProductName(IsVIP);
            }
        }

        /// <summary> 將購物車中非正常販售或VIP正常價的產品名稱加上狀態註記 </summary>
        private static void Add_ProductState_To_ProductName(this CShoppingCart cShoppingCart, bool IsVIP)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            int? SalesState = db.SalesInfos.FirstOrDefault(row => row.SalesInfoIdPk == cShoppingCart.product.SalesInfoIdPk).SalesStatesIdFk;

            switch (SalesState)
            {
                case 1:
                    if (IsVIP)
                    {
                        cShoppingCart.productName += " (VIP優惠)";
                        cShoppingCart.price = Math.Round((float)(cShoppingCart.price * 0.9)); 
                    }
                    break;

                case 2: cShoppingCart.productName += " (特價品)"; break;
                case 3: cShoppingCart.productName += " (即期品)"; break;
                default: break;
            }
        }

        /// <summary> 使貨物資訊至最新狀態 </summary>
        public static void Update_All_Goods()
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            //取得過期以外貨物
            IEnumerable<Good> goodsList = db.Goods.Where(row => row.ProductStatusIdFk != 3);

            foreach (Good goods in goodsList)
            {
                //檢查貨物狀態
                goods.Update_Goods_State();
            }

            db.SaveChanges();
        }

        /// <summary> 依保存期限更新貨物狀態 </summary>
        public static void Update_Goods_State(this Good goods)
        {
            DateTime Today = DateTime.Today;

            //有效日期天數差距
            var DiffDay = new TimeSpan(goods.ShelfLife.Value.Ticks - DateTime.Now.Ticks).TotalDays;

            switch ((DateTime)goods.ShelfLife)
            {
                case DateTime d when d >= Today && DiffDay > 7: goods.ProductStatusIdFk = 1; break;
                case DateTime d when d >= Today && DiffDay <= 7: goods.ProductStatusIdFk = 2; break;
                default: goods.ProductStatusIdFk = 3; break;
            }
        }

        /// <summary> 檢查該貨物是否會被缺貨的販售資訊自動補貨 </summary>
        public static bool Check_Has_OutOfStock_SalesInfo(this Good goods)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            //找出缺貨的販售資訊
            IEnumerable<SalesInfo> salesInfos = db.SalesInfos.Where(row => row.ProductIdFk == goods.ProductIdFk && row.GoodsIdFk == null);

            foreach (SalesInfo item in salesInfos)
            {
                //如果貨物狀態符合販售狀態，表示這筆貨物將被自動補貨
                if (item.SalesInfo_State_Compare_Goods_State(goods.GoodsIdPk))
                    return true;
            }

            return false;
        }

        /// <summary> 檢查修改的貨物是否已上架，如果有則新增一筆販售資訊 </summary>
        public static void Create_OnShelf_Goods_SalesInfo(this Good goods)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            SalesInfo salesInfo = db.SalesInfos.FirstOrDefault(row => row.GoodsIdFk == goods.GoodsIdPk && row.SalesStatesIdFk != 5);

            if (salesInfo != null)
            {
                SalesInfo newone = salesInfo.Create_New_SalesInfo(goods);
                db.SalesInfos.Add(newone);
            }

            db.SaveChanges();
        }

        /// <summary> 使販售資訊至最新狀態 </summary>
        public static void Update_All_SalesInfos()
        {
            //先使貨物資訊至最新狀態
            Update_All_Goods();

            RamenSupermarketContext db = new RamenSupermarketContext();

            List<SalesInfo> newSalesInfos = new();

            //找出正常、即期品販售資訊(升冪排序確保當正常販售的貨物即期時，可以將這個貨物給到"待補貨(或過期)的即期品販售"(沒有可替補的即期貨物情況下))
            foreach (SalesInfo item in db.SalesInfos.Where(row => row.SalesStatesIdFk == 1 || row.SalesStatesIdFk == 3).OrderBy(row => row.SalesStatesIdFk))
            {
                //檢查上架貨物的有效日期是否符合狀態或是數量為0
                if (!item.SalesInfo_State_Compare_Goods_State(item.GoodsIdFk))
                {
                    //不符合狀態則找出一個符合狀態的貨物
                    Good replace = Get_Replace_Goods(item.SalesStatesIdFk, item.ProductIdFk);

                    //本無貨物且找不到可替補貨物，不產生新販售資訊
                    if (item.GoodsIdFk == null && replace == null) continue;

                    //將不符合狀態的貨物停止販售(用以保存記錄)，並上架一個符合狀態的貨物(使用replace符合狀態的貨物)
                    SalesInfo newone = item.Create_New_SalesInfo(replace);
                    newSalesInfos.Add(newone);

                    ////如果販售資訊本有貨物，但因貨物狀態不符，而找到可替補貨物時
                    //if (item.GoodsIdFk != null && replace != null)
                    //{
                    //    //將不符合狀態的貨物停止販售(用以保存記錄)，並上架一個符合狀態的貨物(使用replace符合狀態的貨物)
                    //    SalesInfo newone = item.Create_New_SalesInfo(replace);
                    //    newSalesInfos.Add(newone);
                    //}

                    ////如果販售資訊本有貨物，但因貨物狀態不符，找不到可替補貨物時
                    //if (item.GoodsIdFk != null && replace == null)
                    //{
                    //    SalesInfo newone = item.Create_New_SalesInfo(replace);
                    //    newSalesInfos.Add(newone);
                    //}

                    ////如果販售資訊本無貨物，但找到可替補貨物時
                    //if (item.GoodsIdFk == null && replace != null)
                    //{
                    //    SalesInfo newone = item.Create_New_SalesInfo(replace);
                    //    newSalesInfos.Add(newone);
                    //}

                    ////如果販售資訊本無貨物，且找不到可替補貨物時
                    //if (item.GoodsIdFk == null && replace == null)
                    //{
                    //    continue;
                    //}

                }
            }

            //將檢查產生的新貨物資訊加入
            foreach (SalesInfo item in newSalesInfos)
            {
                db.SalesInfos.Add(item);
            }

            db.SaveChanges();
        }

        /// <summary> 檢查貨物狀態是否符合販售資訊銷售狀態或是數量為0 </summary>
        private static bool SalesInfo_State_Compare_Goods_State(this SalesInfo salesInfo, int? GoodsID)
        {
            //如果沒有貨物ID(待補貨)，返回false
            if (GoodsID == null) return false;

            RamenSupermarketContext db = new RamenSupermarketContext();

            //找出貨物
            Good goods = db.Goods.FirstOrDefault(row => row.GoodsIdPk == GoodsID);

            if (goods == null) return false;

            //如果貨物的數量為0，返回false
            if (goods.Counts <= 0) return false;

            //檢查這筆貨物的狀態後與販售狀態比對
            int? goodsState = goods.ProductStatusIdFk;

            switch (salesInfo.SalesStatesIdFk)
            {
                case int s when s == 1 && goodsState == 1: return true;
                case int s when s == 1 && goodsState != 1: return false;
                case int s when s == 3 && goodsState == 2: return true;
                case int s when s == 3 && goodsState != 2: return false;
                default: return false;
            }
        }

        /// <summary> 取得一個新的符合狀態的貨物 </summary>
        private static Good Get_Replace_Goods(int? SalesStateID, int? ProductID)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            //取得不過期且符合產品ID的貨物，並依照日期升冪排序
            List<Good> list = db.Goods.Where(row => row.ProductIdFk == ProductID && row.ProductStatusIdFk != 3).OrderBy(row => row.ShelfLife).ToList();

            //取得第一個符合狀態的貨物
            foreach (Good goods in list)
            {
                //如果貨物數量為0，跳過這個貨物
                if (goods.Counts <= 0) continue;

                //如果貨物已被上架，跳過這個貨物
                if (db.SalesInfos.Where(row => row.SalesStatesIdFk != 5).FirstOrDefault(row => row.GoodsIdFk == goods.GoodsIdPk) != null)
                    continue;

                //販售狀態比對貨物狀態，符合狀態則回傳這個貨物
                int? goodsState = goods.ProductStatusIdFk;

                switch (SalesStateID)
                {
                    case int s when s == 1 && goodsState == 1: return goods;
                    case int s when s == 3 && goodsState == 2: return goods;
                    default: continue;
                }
            }

            //如果沒有符合狀態的貨物，傳回null，表示待補貨
            return null;
        }

        /// <summary> 產生一個新的符合狀態的販售資訊 </summary>
        private static SalesInfo Create_New_SalesInfo(this SalesInfo record, Good replace)
        {
            SalesInfo newone = new SalesInfo
            {
                ProductIdFk = record.ProductIdFk,
                PriceFactorFk = record.PriceFactorFk,
                DiscountIdFk = record.DiscountIdFk,
                SalesStatesIdFk = record.SalesStatesIdFk,
                GoodsIdFk = replace != null ? replace.GoodsIdPk : null,
                Counts = replace != null ? replace.Counts : 0,
            };

            //計算販售單價，如果沒有符合狀態的貨物則設為null
            newone.UnitPrice = replace != null ? newone.Calculate_SalesInfo_UnitPrice() : record.UnitPrice;

            //更新這個販售資訊的貨物的上架日期
            newone.Update_SalesInfo_Goods_LaunchDate();

            //將不符合狀態的貨物資訊停止販售
            record.SalesStatesIdFk = 5;

            return newone;
        }

        /// <summary> 計算販售單價 </summary>
        private static decimal? Calculate_SalesInfo_UnitPrice(this SalesInfo salesInfo)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            double priceFactor = (double)db.PriceFactors.FirstOrDefault(row => row.PriceFactorIdPk == salesInfo.PriceFactorFk).PriceFactor1;
            double discount = (double)db.Discounts.FirstOrDefault(row => row.DiscountIdPk == salesInfo.DiscountIdFk).DiscountFactor;
            decimal cost = (decimal)db.Goods.FirstOrDefault(row => row.GoodsIdPk == salesInfo.GoodsIdFk).PurchaseCost;

            int unitPrice = (int)Math.Round(priceFactor * discount * (int)cost);

            return unitPrice;
        }

        /// <summary> 販售狀態上架或替補貨物時，更新貨物的上架日期 </summary>
        public static void Update_SalesInfo_Goods_LaunchDate(this SalesInfo salesInfo)
        {
            if (salesInfo == null || salesInfo.GoodsIdFk == null) return;

            RamenSupermarketContext db = new RamenSupermarketContext();

            db.Goods.FirstOrDefault(row => row.GoodsIdPk == salesInfo.GoodsIdFk).LaunchDate = DateTime.Today;

            db.SaveChanges();
        }
    }
}
