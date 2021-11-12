using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Page()
        {
            return View();
        }

        public IActionResult Index(long? CDID_Binary, string KeyWord, int Skip, bool ShowCollect = false, bool spot = false, bool special = false)
        {
            SystemFunction.Update_All_SalesInfos();
            IEnumerable<CSalesInfo> list;
            if (spot == true && special == false)
                list = new CSalesInfo().Get_List_By_CategoryDetails_Binary(CDID_Binary, KeyWord, itemstate: CSalesInfo.ListStates.spotitem);
            else if (spot == false && special == true)
                list = new CSalesInfo().Get_List_By_CategoryDetails_Binary(CDID_Binary, KeyWord, itemstate: CSalesInfo.ListStates.spitem);
            else if (spot == true && special == true)
                list = new CSalesInfo().Get_List_By_CategoryDetails_Binary(CDID_Binary, KeyWord, itemstate: CSalesInfo.ListStates.unnormalitem);
            else
                list = new CSalesInfo().Get_List_By_CategoryDetails_Binary(CDID_Binary, KeyWord);

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
                ViewBag.IsLogin = true;

            if (ShowCollect)
            {
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
                {
                    string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                    int? memberID = JsonSerializer.Deserialize<Member>(json).MemberIdPk;

                    IEnumerable<Collect> collects = new RamenSupermarketContext().Collects.Where(row => row.MemberIdFk == memberID && row.CollectTypeIdFk == 1);

                    list = list.Where(row =>
                    {
                        foreach (Collect item in collects)
                        {
                            if (item.ProductIdFk == row.ProductIdFk)
                                return true;
                        }

                        return false;
                    });
                }
            }

            ViewBag.Product = list.ToList().Count;

            if (Skip > 0)
                list = list.Skip(Skip);

            list = list.Take(9);

            list.Set_All_SalesInfos_Price_Interval();

            return PartialView(list);
        }


        public IActionResult Use_KeyWord_Find_ProductName(string KeyWord)
        {
            var ProductNames = new RamenSupermarketContext().ProductDetails.Where(row => row.ProductName.Contains(KeyWord))
                               .Select(row => new { Id = row.ProductIdPk, Name = row.ProductName });

            return Json(ProductNames);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class APIHomeController : ControllerBase
    {
        private readonly RamenSupermarketContext db;

        public APIHomeController(RamenSupermarketContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collect>>> GetCollects()
        {
            int? memberID = null;

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                memberID = JsonSerializer.Deserialize<Member>(json).MemberIdPk;
            }

            //回傳登入會員的收藏產品資料
            return await db.Collects.Where(row => row.MemberIdFk == memberID).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add_Favorite([FromBody] int ProductID)
        {
            Collects_Action(ProductID, 1);
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Tracking([FromBody] int ProductID)
        {
            Collects_Action(ProductID, 2);
            await db.SaveChangesAsync();
            return NoContent();

        }

        public void Collects_Action(int ProductID, int CollectTypeID)
        {
            int? memberID = null;

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                memberID = JsonSerializer.Deserialize<Member>(json).MemberIdPk;

                //檢查登入會員有無收藏或追蹤這個產品
                Collect collect = db.Collects.FirstOrDefault(row => row.MemberIdFk == memberID &&
                                                                    row.ProductIdFk == ProductID &&
                                                                    row.CollectTypeIdFk == CollectTypeID);

                //沒有則加入這個商品的收藏或追蹤
                if (collect == null)
                {
                    db.Collects.Add(new Collect
                    {
                        MemberIdFk = memberID,
                        ProductIdFk = ProductID,
                        CollectTypeIdFk = CollectTypeID,
                    });
                }

                //有則移除這個收藏或追蹤
                else
                    db.Collects.Remove(collect);

            }

        }


    }
}
