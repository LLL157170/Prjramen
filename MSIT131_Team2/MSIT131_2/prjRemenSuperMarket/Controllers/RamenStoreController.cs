using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using prjRemenSuperMarket.Views.RamenStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace prjRemenSuperMarket.Controllers
{
    public class RamenStoreController : Controller
    {
        

        public IActionResult PushPage()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                return View();
            }

            return RedirectToAction("Login", "MMLogin", new { lastAction = Request.Path.ToString() });
        }

        public IActionResult _PushPage_RamenList(int? storeID)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                return PartialView(new CRamenAdd().Get_List(storeID));
            }

            return RedirectToAction("Login", "MMLogin", new { lastAction = Request.Path.ToString() });
        }

        public IActionResult _PushPage_Store_Add()
        {
            return PartialView();
        }


        public IActionResult _PushPage_Store_Edit(int? id)
        {
            RamenStore ramenStore = new RamenSupermarketContext().RamenStores.FirstOrDefault(row => row.RamenStoreId == id);
            CStoreAdd cRamenStore = new CStoreAdd();
            cRamenStore.RamenStore = ramenStore;

            return PartialView(cRamenStore);
        }

        public IActionResult _PushPage_Ramen_Add()
        {
            return PartialView();
        }

        public IActionResult _PushPage_Ramen_Edit(int? id)
        {
            RamenProductInfo productInfo = new RamenSupermarketContext().RamenProductInfos.FirstOrDefault(row => row.RamenProductId == id);
            CRamenAdd cRamenAdd = new CRamenAdd();
            cRamenAdd.RamenProductInfo = productInfo;

            return PartialView(cRamenAdd);
        }

        [HttpPost]
        public IActionResult StoreAdd(CStoreAdd cStoreAdd)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member cust = JsonSerializer.Deserialize<Member>(json);

                cStoreAdd.MemberId = cust.MemberIdPk;

                if (cStoreAdd.Logo != null)
                    cStoreAdd.LogoBytes = cStoreAdd.Logo.TransToBytes();

                if (cStoreAdd.Picture != null)
                    cStoreAdd.PictureBytes = cStoreAdd.Picture.TransToBytes();

                db.RamenStores.Add(cStoreAdd.RamenStore);
                db.SaveChanges();
            }

            return NoContent();
        }

        [HttpPost]
        public IActionResult StoreEdit(CStoreAdd cStoreAdd)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            RamenStore editStore = db.RamenStores.FirstOrDefault(p => p.RamenStoreId == cStoreAdd.RamenStoreId);

            //使editStore 不被DBContext 追蹤
            db.Entry(editStore).State = EntityState.Detached;

            if (cStoreAdd.Logo != null)
                cStoreAdd.LogoBytes = cStoreAdd.Logo.TransToBytes();
            else
                cStoreAdd.LogoBytes = editStore.Logo;

            if (cStoreAdd.Picture != null)
                cStoreAdd.PictureBytes = cStoreAdd.Picture.TransToBytes();
            else
                cStoreAdd.PictureBytes = editStore.Pictrue;

            editStore = cStoreAdd.RamenStore;

            db.RamenStores.Update(editStore);
            db.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public IActionResult StoreDelete(int? id)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            IEnumerable<RamenProductInfo> productInfos = db.RamenProductInfos.Where(row => row.RamenStoreId == id);

            foreach (RamenProductInfo item in productInfos)
                db.Entry(item).State = EntityState.Deleted;

            db.SaveChanges();

            RamenStore ramenStore = db.RamenStores.FirstOrDefault(row => row.RamenStoreId == id);

            db.Entry(ramenStore).State = EntityState.Deleted;

            db.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public IActionResult RamenAdd(CRamenAdd cramenAdd)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            if (cramenAdd.ProductPicture != null)
                cramenAdd.ProductPictureByte = cramenAdd.ProductPicture.TransToBytes();

            db.RamenProductInfos.Add(cramenAdd.RamenProductInfo);
            db.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public IActionResult RamenEdit(CRamenAdd cramenAdd)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            RamenProductInfo editRamen = db.RamenProductInfos.FirstOrDefault(row => row.RamenProductId == cramenAdd.RamenProductId);
            db.Entry(editRamen).State = EntityState.Detached;

            if (cramenAdd.ProductPicture != null)
                cramenAdd.ProductPictureByte = cramenAdd.ProductPicture.TransToBytes();
            else
                cramenAdd.ProductPictureByte = editRamen.ProductPicture;

            editRamen = cramenAdd.RamenProductInfo;

            db.RamenProductInfos.Update(editRamen);
            db.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public IActionResult RamenDelete(int? id)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            RamenProductInfo productInfos = db.RamenProductInfos.FirstOrDefault(row => row.RamenProductId == id);

            db.Entry(productInfos).State = EntityState.Deleted;

            db.SaveChanges();

            return NoContent();
        }

        public IActionResult StoreName_List()
        {

            string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
            Member cust = JsonSerializer.Deserialize<Member>(json);

            var list = new RamenSupermarketContext().RamenStores.Where(p => p.MemberId == cust.MemberIdPk).Select(p => new { id = p.RamenStoreId, name = p.StoreName });

            return Json(list);
        }

        public IActionResult storeDetail(int id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
                ViewBag.IsLogin = true;

            RamenSupermarketContext db = new RamenSupermarketContext();
            var q = db.RamenStores.FirstOrDefault(p => p.RamenStoreId == id);
            CRamenStore rs = new CRamenStore();
            rs.ramenStore = q;
            return View(rs);
        }

        public IActionResult RamenDetail(int id)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var q = db.RamenProductInfos.Where(p => p.RamenStoreId == id);
            List<CRamenProductInfo> list = new List<CRamenProductInfo>();
            foreach (RamenProductInfo item in q)
            {
                CRamenProductInfo c = new CRamenProductInfo();
                c.ramenproductinfo = item;
                list.Add(c);
            }
            return Json(list);
        }

        public IActionResult District()
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var q = db.Districts;
            return Json(q);
        }
    }
}
