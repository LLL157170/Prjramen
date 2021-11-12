using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Controllers
{
    public class GoodsController : Controller
    {
        public IActionResult List()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "EMLogin", new { lastAction = Request.Path.ToString() });
            }
        }

        public IActionResult _Good_List_View(int? ProductID, int? CDID, int? StateID, string KeyWord)
        {
            SystemFunction.Update_All_SalesInfos();
            return PartialView(new CGoods().Get_List(ProductID: ProductID,
                                                     CDID: CDID,
                                                     StateID: StateID,
                                                     KeyWord: KeyWord));
        }

        public IActionResult _Create_Goods_View()
        {
            return PartialView();
        }

        public IActionResult _Edit_Goods_View(int? id)
        {
            Good good = new RamenSupermarketContext().Goods.FirstOrDefault(row => row.GoodsIdPk == id);
            CGoods cGoods = new CGoods();
            cGoods.Good = good;

            return PartialView(cGoods);
        }

        public IActionResult Demo1()
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            if (db.ProductDetails.FirstOrDefault(r => r.ProductIdPk == 206) == null)
                return NoContent();

            db.Goods.Add(DemoNormalGoods());
            db.Goods.Add(DemoSpotGoods());

            db.SaveChanges();

            return NoContent();
        }

        public IActionResult Demo2()
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            if (db.ProductDetails.FirstOrDefault(r => r.ProductIdPk == 206) == null)
                return NoContent();

            db.Goods.Add(DemoNormalGoods());
            db.Goods.Add(DemoNormalGoods());
            db.Goods.Add(DemoSpotGoods());
            db.Goods.Add(DemoNormalGoods());
            db.Goods.Add(DemoNormalGoods());
            db.Goods.Add(DemoSpotGoods());
            db.Goods.Add(DemoNormalGoods());
            db.Goods.Add(DemoNormalGoods());
            db.Goods.Add(DemoSpotGoods());

            db.SaveChanges();

            //if (DemoNormalGoods().Check_Has_OutOfStock_SalesInfo())
            //    return Content("因目前有一缺貨的販售資訊，該貨物已被系統自動上架。");
            //else
                return NoContent();
        }

        private Good DemoNormalGoods()
        {
            return new Good
            {
                ProductIdFk = 206,
                EmployeeIdFk = 1,
                MerchantIdFk = 1,
                Counts = 20,
                PurchaseCost = 20,
                PurchaseDate = DateTime.Today,
                ShelfLife = DateTime.Today.AddDays(14),
                ProductStatusIdFk = 1
            };
        }

        private Good DemoSpotGoods()
        {
            return new Good
            {
                ProductIdFk = 206,
                EmployeeIdFk = 1,
                MerchantIdFk = 3,
                Counts = 20,
                PurchaseCost = 20,
                PurchaseDate = DateTime.Today,
                ShelfLife = DateTime.Today.AddDays(6),
                ProductStatusIdFk = 2
            };
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class APIGoodsController : ControllerBase
    {
        private readonly RamenSupermarketContext db;

        public APIGoodsController(RamenSupermarketContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<ActionResult<Good>> Create(Good goods)
        {
            //設置新增貨物的員工ID
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_EMPLOYEE);
                Employee employee = JsonSerializer.Deserialize<Employee>(json);
                goods.EmployeeIdFk = employee.EmployeeIdPk;
            }

            //設置貨物狀態
            goods.Update_Goods_State();

            db.Goods.Add(goods);

            await db.SaveChangesAsync();

            if (goods.Check_Has_OutOfStock_SalesInfo())
                return Content("因目前有一缺貨的販售資訊，該貨物已被系統自動上架。");

            return Content("新增成功");
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Good goods)
        {
            //設置新增貨物的員工ID
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_EMPLOYEE);
                Employee employee = JsonSerializer.Deserialize<Employee>(json);
                goods.EmployeeIdFk = employee.EmployeeIdPk;
            }

            db.Entry(goods).State = EntityState.Modified;

            //設置貨物狀態
            goods.Update_Goods_State();

            await db.SaveChangesAsync();

            //檢查該貨物是否已上架(等待db儲存後再檢查確保資料正確，該方法會查找DB)
            goods.Create_OnShelf_Goods_SalesInfo();

            if (goods.Check_Has_OutOfStock_SalesInfo())
                return Content("因目前有一缺貨的販售資訊，該貨物已被系統自動上架。");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            SalesInfo salesInfo = await db.SalesInfos.FirstOrDefaultAsync(row => row.GoodsIdFk == id);

            if (salesInfo != null)
                return NotFound("無法刪除該貨物，因為在販售資訊已有該貨物記錄");

            Good goods = await db.Goods.FirstOrDefaultAsync(row => row.GoodsIdPk == id);

            if (goods == null)
                return NotFound("找不到該貨物，無法刪除");

            db.Goods.Remove(goods);

            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}
