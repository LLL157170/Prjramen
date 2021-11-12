using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjRemenSuperMarket.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace prjRemenSuperMarket.Controllers
{
    public class SelectorListController : Controller
    {

        /// <summary> JSON:所有折扣名稱列表 </summary>
        public IActionResult DiscountName()
        {
            return Json(SelectorListByJson.DiscountName());
        }

        /// <summary> JSON:所有售價係數列表 </summary>
        public IActionResult PriceFactor()
        {
            return Json(SelectorListByJson.PriceFactor());
        }

        /// <summary> JSON:所有販售狀態列表(包含"全部"選項) </summary>
        public IActionResult All_SalesStates()
        {
            return Json(SelectorListByJson.All_SalesStates());
        }

        /// <summary> JSON:所有商品名稱列表(包含"全部"選項) </summary>
        public IActionResult All_ProductName()
        {
            return Json(SelectorListByJson.All_ProductName());
        }

        public IActionResult ProductName()
        {
            return Json(SelectorListByJson.ProductName());
        }

        /// <summary> JSON:未上架販售狀態列表(不包含"停止販售"選項) </summary>
        public IActionResult Exclude_SalesStates(int ProductID)
        {
            return Json(SelectorListByJson.Exclude_SalesStates(ProductID));
        }

        /// <summary> JSON:未上架販售狀態(不包含"停止販售")的商品名稱列表 </summary>
        public IActionResult Exclude_ProductName()
        {
            return Json(SelectorListByJson.Exclude_ProductName());
        }

        public IActionResult Category()
        {
            return Json(SelectorListByJson.Category());
        }

        public IActionResult CategoryDetails(int? CategoryID)
        {
            return Json(SelectorListByJson.CategoryDetails(CategoryID));
        }

        public IActionResult All_Category()
        {
            return Json(SelectorListByJson.All_Category());
        }

        public IActionResult All_CategoryDetails()
        {
            return Json(SelectorListByJson.All_CategoryDetails());
        }

        public IActionResult Storage()
        {
            return Json(SelectorListByJson.Storage());
        }

        public IActionResult All_ProductStatus()
        {
            return Json(SelectorListByJson.All_ProductStatus());
        }

        public IActionResult Filter_All_ProductName(int CategoryDetailID)
        {
            return Json(SelectorListByJson.Filter_All_ProductName(CategoryDetailID));
        }

        public IActionResult MerchantName()
        {
            return Json(SelectorListByJson.MerchantName());
        }

        public IActionResult City()
        {
            return Json(SelectorListByJson.City());
        }

        public IActionResult District(int? CityID)
        {
            return Json(SelectorListByJson.District(CityID));
        }

        public IActionResult RamenStore()
        {
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
            Member cust = JsonSerializer.Deserialize<Member>(json);

            return Json(SelectorListByJson.RamenStore(cust.MemberIdPk));
        }
    }

    public static class SelectorListByJson
    {
        public static IEnumerable<object> PriceFactor()
        {
            var list = new RamenSupermarketContext().PriceFactors.Select(row => new { Id = row.PriceFactorIdPk, Name = row.PriceFactor1 });

            return list;
        }

        public static IEnumerable<object> All_SalesStates()
        {
            List<object> List = new List<object>();
            var list = new RamenSupermarketContext().SalesStates.Select(row => new { Id = row.SalesStatesIdPk, Name = row.SalesStates });

            List.Add(new { Id = (int?)null, Name = "全部" });

            foreach (var item in list)
                List.Add(item);

            return List;
        }

        public static IEnumerable<object> Filter_All_ProductName(int CategoryDetailID)
        {
            List<object> List = new List<object>();

            var list = new RamenSupermarketContext().ProductDetails.Where(row => row.CategoryDetailIdFk == CategoryDetailID)
                .Select(row => new { Id = row.ProductIdPk, Name = row.ProductName });

            List.Add(new { Id = (int?)null, Name = "全部" });

            foreach (var item in list)
                List.Add(item);

            return List;
        }

        public static IEnumerable<object> All_ProductName()
        {
            List<object> List = new List<object>();
            var list = new RamenSupermarketContext().ProductDetails.Select(row => new { Id = row.ProductIdPk, Name = row.ProductName });

            List.Add(new { Id = (int?)null, Name = "全部" });

            foreach (var item in list)
                List.Add(item);

            return List;
        }

        public static IEnumerable<object> ProductName()
        {
            return new RamenSupermarketContext().ProductDetails.Select(row => new { Id = row.ProductIdPk, Name = row.ProductName });
        }

        public static IEnumerable<object> All_CategoryDetails()
        {
            List<object> List = new List<object>();
            var list = new RamenSupermarketContext().CategoryDetails.Select(row => new { Id = row.CategoryDetailIdPk, Name = row.CategoryDetailName });

            List.Add(new { Id = (int?)null, Name = "全部" });

            foreach (var item in list)
                List.Add(item);

            return List;
        }

        public static IEnumerable<object> All_Category()
        {
            List<object> List = new List<object>();
            var list = new RamenSupermarketContext().Categorys.Select(row => new { Id = row.CategoryIdPk, Name = row.CategoryName });

            List.Add(new { Id = (int?)null, Name = "全部" });

            foreach (var item in list)
                List.Add(item);

            return List;
        }

        public static IEnumerable<object> All_ProductStatus()
        {
            List<object> List = new List<object>();
            var list = new RamenSupermarketContext().ProductStatuses.Select(row => new { Id = row.ProductStatusIdPk, Name = row.ProductStatus1 });

            List.Add(new { Id = (int?)null, Name = "全部" });

            foreach (var item in list)
                List.Add(item);

            return List;
        }
        public static IEnumerable<object> Category()
        {
            var list = new RamenSupermarketContext().Categorys.Select(row => new { Id = row.CategoryIdPk, Name = row.CategoryName });

            return list;
        }

        public static IEnumerable<object> MerchantName()
        {
            return new RamenSupermarketContext().Merchants.Select(row => new { Id = row.MerchantIdPk, Name = row.MerchantName });
        }

        public static IEnumerable<object> Storage()
        {
            var list = new RamenSupermarketContext().StorageMethods.Select(row => new { Id = row.StorageIdPk, Name = row.StorageName });

            return list;
        }

        public static IEnumerable<object> RamenStore(int? MemberID)
        {
            return new RamenSupermarketContext().RamenStores.Where(row => row.MemberId == MemberID)
                .Select(row => new { Id = row.RamenStoreId, Name = row.StoreName });
        }

        public static IEnumerable<object> DiscountName()
        {
            var list = new RamenSupermarketContext().Discounts.OrderByDescending(row => row.DiscountFactor)
                       .Select(row => new { Id = row.DiscountIdPk, Name = $"{row.DiscountName}(*{row.DiscountFactor})", Factor = row.DiscountFactor });

            return list;
        }

        public static IEnumerable<object> City()
        {
            return new RamenSupermarketContext().Citys.Select(row => new { Id = row.CityIdPk, Name = row.CityName });
        }

        public static IEnumerable<object> District(int? CityID)
        {
            return new RamenSupermarketContext().Districts.Where(row=>row.CityIdFk==CityID)
                .Select(row => new { Id = row.DistrictIdPk, Name = row.DistrictName });
        }

        public static IEnumerable<object> CategoryDetails(int? CategoryID)
        {
            if(CategoryID != null)
                return new RamenSupermarketContext().CategoryDetails.Where(row => row.CategoryIdFk == CategoryID)
                       .Select(row => new { Id = row.CategoryDetailIdPk, Name = row.CategoryDetailName });

            else
                return new RamenSupermarketContext().CategoryDetails
                       .Select(row => new { Id = row.CategoryDetailIdPk, Name = row.CategoryDetailName });
        }

        public static IEnumerable<object> Exclude_SalesStates(int ProductID)
        {
            var list = Get_Product_SaleStateList(ProductID).Select(row => new { Id = row.SalesStatesIdPk, Name = row.SalesStates });

            return list;
        }

        public static IEnumerable<object> Exclude_ProductName()
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            List<ProductDetail> productList = db.ProductDetails.ToList();

            foreach (int id in db.SalesInfos.Select(row => row.ProductIdFk))
            {
                //如果未上架的銷售列表數量為0(表示該商品的各銷售狀態皆已上架)，在商品列表中刪除該商品
                if (Get_Product_SaleStateList(id).Count == 0)
                {
                    ProductDetail product = db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == id);
                    productList.Remove(product);
                }
            }

            var list = productList.Select(row => new { Id = row.ProductIdPk, Name = row.ProductName });

            return list;
        }

        /// <summary> 取得指定商品未上架的銷售狀態列表</summary>
        private static List<SalesState> Get_Product_SaleStateList(int ProductID)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            List<SalesState> SaleStateList = db.SalesStates.Where(row => row.SalesStatesIdPk != 5).ToList();

            //在上架產品中搜尋該產品的所有銷售狀態
            IEnumerable<int?> existingSaleStatesID = db.SalesInfos.Where(row => row.ProductIdFk == ProductID && row.SalesStatesIdFk != 5)
                                                     .Select(row => row.SalesStatesIdFk);

            //刪除該產品已上架的銷售狀態
            foreach (int id in existingSaleStatesID)
            {
                SalesState removeSaleState = db.SalesStates.FirstOrDefault(row => row.SalesStatesIdPk == id);
                SaleStateList.Remove(removeSaleState);
            }

            return SaleStateList;
        }
    }
}
