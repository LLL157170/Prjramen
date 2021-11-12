using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Controllers
{
    public class ProductDetailController : Controller
    {
        public IActionResult List()
        {
            if(HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
                return View();

            return RedirectToAction("Login", "EMLogin", new { lastAction = Request.Path.ToString() });
        }

        public IActionResult _ProductList(int? CategoryID,string KeyWord)
        {
           return PartialView(new CProductDetail().Get_List(KeyWord:KeyWord,CategoryID: CategoryID));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CProductDetail p)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            p.Product.Views = 0;

            if (p.Photo != null)
                p.Product.Picture = p.Photo.TransToBytes();

            db.ProductDetails.Add(p.Product);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("List");

            ProductDetail product = new RamenSupermarketContext().ProductDetails.FirstOrDefault(row => row.ProductIdPk == id);

            if (product == null) return RedirectToAction("List");

            CProductDetail cProduct = new CProductDetail();
            cProduct.Product = product;

            return View(cProduct);
        }

        [HttpPost]
        public IActionResult Edit(CProductDetail p)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            ProductDetail product = db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == p.ProductIdPk);

            if (product == null) return RedirectToAction("List");

            if (p.Photo != null)
                product.Picture = p.Photo.TransToBytes();

            product.CategoryDetailIdFk = p.CategoryDetailIdFk;
            product.ProductName = p.ProductName;
            product.Specification = p.Specification;
            product.Weight = p.Weight;
            product.Origin = p.Origin;
            product.StorageIdFk = p.StorageIdFk;
            product.Storagedays = p.Storagedays;
            product.ProductDescription = p.ProductDescription;

            db.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult Delete(int? id)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();

            ProductDetail product = db.ProductDetails.FirstOrDefault(p => p.ProductIdPk == id);

            if (product != null)
            {
                db.ProductDetails.Remove(product);
                db.SaveChanges();
            }

            return RedirectToAction("List");
        }

        public IActionResult Category_List()
        {
            var list = new RamenSupermarketContext().Categorys.Select(row => new { Id = row.CategoryIdPk, Name = row.CategoryName });

            List<object> List = new List<object>();

            List.Add(new { Id = (int?)null, Name = "全部" });

            foreach (var item in list)
            {
                List.Add(item);
            }

            return Json(List);
        }

        public IActionResult CategoryDetail_List(int CategoryID)
        {
            var list = new RamenSupermarketContext().CategoryDetails.Where(row => row.CategoryIdFk == CategoryID)
                       .Select(row => new { Id = row.CategoryDetailIdPk, Name = row.CategoryDetailName });

            return Json(list);
        }

        public IActionResult Storage_List()
        {
            var list = new RamenSupermarketContext().StorageMethods.Select(row => new { Id = row.StorageIdPk, Name = row.StorageName });

            return Json(list);
        }

        public IActionResult ProductList_By_Category_Filter(int CategoryID)
        {
            return Json(new CProductDetail().Get_List(CategoryID:CategoryID));
        }
    }

    public static class ImageTransByte
    {
        public static byte[] TransToBytes(this IFormFile file)
        {
            byte[] Bytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                Bytes = ms.ToArray();
            }

            return Bytes;
        }
    }
}
