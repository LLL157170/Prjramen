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
    public class ShoppingCartController : Controller
    {
        public IActionResult List()
        {
            return View(new CSalesInfo().Get_List());
        }

        public IActionResult ViewCart()
        {
            Member member;

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                member = JsonSerializer.Deserialize<Member>(json);

                ViewBag.txtId = member.MemberIdPk;
                ViewBag.txtName = member.Name;
                ViewBag.txtPhone = member.Phone;
            }
            else
            {
                return RedirectToAction("Login", "MMLogin", new { lastAction = Request.Path.ToString() });
            }



            List<CShoppingCart> cart = new List<CShoppingCart>();
            if (HttpContext.Session.Keys.Contains(
                CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART))
            {
                string json = HttpContext.Session.GetString(
                    CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART);
                cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);

                RamenSupermarketContext db = new RamenSupermarketContext();
                foreach (var item in cart)
                {
                    CSalesInfo z = new CSalesInfo();
                    SalesInfo a = db.SalesInfos.FirstOrDefault(row => row.SalesInfoIdPk == item.productId);
                    z.QueryProduct = db.ProductDetails.FirstOrDefault(row => row.ProductIdPk == a.ProductIdFk);
                    item.PictureBase64 = z.PictureBase64;
                }
            }

            cart.Add_ProductState_To_All_ProductNames(member.Check_Member_Is_VIP());

            return View(cart);
        }

        [HttpPost]
        public IActionResult ViewCart(CAddToCartViewModel[] models, string hrefString)
        {
            string json;
            List<CShoppingCart> cart = new List<CShoppingCart>();

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART))
            {
                json = HttpContext.Session.GetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART);
                cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);
            }

            foreach (CShoppingCart item in cart)
            {
                item.count = models.FirstOrDefault(row => row.txtFid == item.productId).txtCount;
            }

            cart.RemoveAll(item => item.count == 0);

            json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(
                CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART, json);

            //if (hrefString != null)
            //{
            if (hrefString.Count(t => t == '/') >= 2)
            {
                string Controller = hrefString.Split('/')[1];
                string Action = hrefString.Split('/')[2];

                return RedirectToAction(Action, Controller);
            }

            else
                return RedirectToAction("Index", "Ramen");

            //}

            //return RedirectToAction("PaymentList", "Pay");
        }

        [HttpPost]
        public IActionResult AddtoCart(int? id, int cn)
        {
            string json = "";
            RamenSupermarketContext db = new RamenSupermarketContext();
            SalesInfo x = db.SalesInfos.FirstOrDefault(p => p.SalesInfoIdPk == id);
            CSalesInfo z = new CSalesInfo();
            z.SalesInfo = x;

            if (x != null)
            {
                List<CShoppingCart> cart = new List<CShoppingCart>(); ;
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART))
                {
                    json = HttpContext.Session.GetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART);
                    cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);

                }
                if (cart.FirstOrDefault(p => p.productId == x.SalesInfoIdPk) != null)
                {
                    CShoppingCart item = cart.FirstOrDefault(p => p.productId == x.SalesInfoIdPk);
                    item.count += cn;
                }
                else
                {
                    CShoppingCart items = new CShoppingCart()
                    {
                        count = cn,
                        product = x,
                        productName = z.GoodName,
                        price = (double)x.UnitPrice,
                        discount = (double)z.Discount,
                        productId = x.SalesInfoIdPk
                    };
                    cart.Add(items);
                }

                json = JsonSerializer.Serialize(cart);
                HttpContext.Session.SetString(
                    CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART, json);
            }
            return RedirectToAction("ViewCart");
        }

        public IActionResult Demo_All_In()
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            SalesInfo normal = db.SalesInfos.FirstOrDefault(r => r.ProductIdFk == 206 && r.SalesStatesIdFk == 1);
            SalesInfo spot = db.SalesInfos.FirstOrDefault(r => r.ProductIdFk == 206 && r.SalesStatesIdFk == 3);
            SalesInfo special = db.SalesInfos.FirstOrDefault(r => r.ProductIdFk == 206 && r.SalesStatesIdFk == 2);

            string json;
            List<CShoppingCart> cart = new List<CShoppingCart>();

            json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART, json);
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART))
            {
                json = HttpContext.Session.GetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART);
                cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);

                if (normal != null)
                    cart.Add(Demo(new CSalesInfo { SalesInfo = normal }));
                if (spot != null)
                    cart.Add(Demo(new CSalesInfo { SalesInfo = spot }));
                if (special != null)
                    cart.Add(Demo(new CSalesInfo { SalesInfo = special }));

                json = JsonSerializer.Serialize(cart);
                HttpContext.Session.SetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART, json);
            }

            return RedirectToAction("ViewCart");
        }

        private CShoppingCart Demo(CSalesInfo s)
        {
            return new CShoppingCart
            {
                product = s.SalesInfo,
                productId = s.SalesInfoIdPk,
                productName = s.GoodName,
                price = (double)s.UnitPrice,
                count = (int)s.Counts,
                discount = (double)s.DiscountIdFk,
            };
        }

        public IActionResult ShoppingCart_List_Count()
        {
            string json = "";
            List<CShoppingCart> cart = new List<CShoppingCart>();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART))
            {
                json = HttpContext.Session.GetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART);
                cart = JsonSerializer.Deserialize<List<CShoppingCart>>(json);
            }

            return Json(new { count = cart.Count });
        }
    }
}
