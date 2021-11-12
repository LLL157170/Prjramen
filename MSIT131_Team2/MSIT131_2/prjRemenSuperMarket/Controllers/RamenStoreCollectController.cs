using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Controllers
{
    public class RStoreCollectController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RamenStoreCollectController : Controller
    {
        private readonly RamenSupermarketContext db;

        public RamenStoreCollectController(RamenSupermarketContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RamenStoreCollect>>> GetCollects()
        {
            int? memberID = null;

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                memberID = JsonSerializer.Deserialize<Member>(json).MemberIdPk;
            }

            //回傳登入會員的收藏產品資料
            return await db.RamenStoreCollects.Where(row => row.MemberId == memberID).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add_Favorite([FromBody] int StoreID)
        {
            int? memberID = null;

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                memberID = JsonSerializer.Deserialize<Member>(json).MemberIdPk;

                //檢查登入會員有無收藏或追蹤這個產品
                RamenStoreCollect collect = db.RamenStoreCollects.FirstOrDefault(row => row.MemberId == memberID &&
                                                                                        row.StoreId == StoreID);

                //沒有則加入這個商品的收藏或追蹤
                if (collect == null)
                {
                    db.RamenStoreCollects.Add(new RamenStoreCollect
                    {
                        MemberId = memberID,
                        StoreId = StoreID,
                    });
                }

                //有則移除這個收藏或追蹤
                else
                    db.RamenStoreCollects.Remove(collect);

            }

            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}
