using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjRemenSuperMarket.ViewModel;
using prjRemenSuperMarket.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace prjRemenSuperMarket.Controllers
{
    public class SalesInfoController : Controller
    {
        public IActionResult List()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            {
                SystemFunction.Update_All_SalesInfos();
                return View(new CSalesInfo().Get_List());
            }
            else
            {
                return RedirectToAction("Login", "EMLogin", new { lastAction = Request.Path.ToString() });
            }
        }

        public IActionResult _Current_SalesInfosList_View(int? ProductID, int? CategoryID, int? StateID, string KeyWord)
        {
            return PartialView(new CSalesInfo().Get_List(ProductID: ProductID,
                                                  CategoryID: CategoryID,
                                                  StateID: StateID,
                                                  KeyWord: KeyWord,
                                                  state: CSalesInfo.ListStates.Current));
        }

        public IActionResult _Expired_SalesInfosList_View(int? ProductID, int? CategoryID, int? StateID, string KeyWord)
        {
            return PartialView(new CSalesInfo().Get_List(ProductID: ProductID,
                                                  CategoryID: CategoryID,
                                                  StateID: StateID,
                                                  KeyWord: KeyWord,
                                                  state: CSalesInfo.ListStates.Expired));
        }

        public IActionResult _Create_SalesInfo()
        {
            return PartialView();
        }

        public IActionResult _Edit_SalesInfo(int? id)
        {
            SalesInfo salesInfo = new RamenSupermarketContext().SalesInfos.FirstOrDefault(row => row.SalesInfoIdPk == id);

            CSalesInfo cSalesInfo = new CSalesInfo();
            cSalesInfo.SalesInfo = salesInfo;

            return PartialView(cSalesInfo);
        }

        public IActionResult _GoodsList_View(int ProductID, int? GoodsID, int? StateID)
        {
            SystemFunction.Update_All_Goods();

            return PartialView(new CGoods().Get_Not_Expired_List(ProductID, GoodsID, StateID));
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class APISalesInfoController : ControllerBase
    {
        private readonly RamenSupermarketContext db;

        public APISalesInfoController(RamenSupermarketContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<ActionResult<SalesInfo>> Create(SalesInfo salesInfo)
        {
            db.SalesInfos.Add(salesInfo);

            //更新這個販售資訊的貨物的上架日期
            salesInfo.Update_SalesInfo_Goods_LaunchDate();

            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, SalesInfo salesInfo)
        {
            if (id != salesInfo.SalesInfoIdPk)
            {
                return BadRequest();
            }

            //修改販售資訊
            if (salesInfo.SalesStatesIdFk != 5)
            {
                //找出相同ID的販售資訊(保存紀錄用)
                SalesInfo record = await db.SalesInfos.FirstOrDefaultAsync(row => row.SalesInfoIdPk == id);

                //將修改的資訊內容指派給新的販售資訊
                SalesInfo newone = new SalesInfo
                {
                    ProductIdFk = salesInfo.ProductIdFk,
                    GoodsIdFk = salesInfo.GoodsIdFk,
                    PriceFactorFk = salesInfo.PriceFactorFk,
                    UnitPrice = salesInfo.UnitPrice,
                    Counts = salesInfo.Counts,
                    DiscountIdFk = salesInfo.DiscountIdFk,
                    SalesStatesIdFk = salesInfo.SalesStatesIdFk,
                };

                //更新這個販售資訊的貨物的上架日期
                newone.Update_SalesInfo_Goods_LaunchDate();

                //將販售資訊紀錄的狀態設為停止出售
                record.SalesStatesIdFk = 5;

                //加入新的販售資訊
                db.SalesInfos.Add(newone);
            }

            //下架販售資訊
            else
                db.Entry(salesInfo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool SalesInfoExists(int id)
        {
            return db.SalesInfos.Any(e => e.SalesInfoIdPk == id);
        }
    }
}
