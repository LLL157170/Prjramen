using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjRamenProject.Controllers
{
    public class PayController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly RamenSupermarketContext _context;
        public PayController(IWebHostEnvironment hostEnv, RamenSupermarketContext Context)
        {
            _hostingEnv = hostEnv;
            _context = Context;
        }

        public IActionResult PaymentList()
        {
            Member member;
            List<CShoppingCart> cart = null;
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
                return RedirectToAction("Login", "Member");
            }

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART))
            {
                string cartJson = HttpContext.Session.GetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART);
                cart = JsonSerializer.Deserialize<List<CShoppingCart>>(cartJson);

                foreach(var item in cart)
                {
                    //var y = (from a in _context.SalesInfos
                    //         from o in _context.ProductDetails
                    //         where a.SalesInfoIdPk == item.productId && a.ProductIdFk == o.ProductIdPk
                    //         select o).FirstOrDefault();


                    CSalesInfo z = new CSalesInfo();
                    SalesInfo a = _context.SalesInfos.FirstOrDefault(row => row.SalesInfoIdPk == item.productId);
                    z.QueryProduct = _context.ProductDetails.FirstOrDefault(row => row.ProductIdPk == a.ProductIdFk);
                    item.PictureBase64 = z.PictureBase64;
                }
            }
            else
            {
                return RedirectToAction("Page", "Home");
            }

            cart.Add_ProductState_To_All_ProductNames(member.Check_Member_Is_VIP());

            return View(cart);
        }

        [HttpPost]
        public IActionResult PaymentList(CPayOrderViewModel p)
        {
            Member member = null;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                member = JsonSerializer.Deserialize<Member>(json);
            }

            if (member == null) 
                return RedirectToAction("Page", "Home");

            var payMentId = ((from x in _context.PaymentTypes
                              where x.Payment == p.OCheck
                              select x.PaymentIdPk).FirstOrDefault());

            Order order = new Order()
            {
                
                MemberIdFk = p.OId,
                PaymentFk = payMentId,
                OrderDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                ShipAddress = p.OAddress,
                DeliveryStatusIdFk = 1,
                TotalPrice = ((decimal)(p.OTotalPrice)),
                DistrictIdFk = p.ODistrictId
                

            };
            _context.Orders.Add(order);
            _context.SaveChanges();


            List<CShoppingCart> cart = null;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART))
            {
                string cartJson = HttpContext.Session.GetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART);
                cart = JsonSerializer.Deserialize<List<CShoppingCart>>(cartJson);
                cart.Add_ProductState_To_All_ProductNames(member.Check_Member_Is_VIP());

                for (int x =0; x < cart.Count ; x++)
                {
                    
                    var SalesInfoID = (from pd in _context.SalesInfos
                                 where cart[x].productId == pd.SalesInfoIdPk
                                 select pd.SalesInfoIdPk).FirstOrDefault();

                    OrderDetail orderDetail = new OrderDetail()
                    {
                    
                    SalesInfoIdFk = SalesInfoID,
                    OrderIdFk = order.OrderIdPk,
                    Counts = cart[x].count,
                    Subtotal = cart[x].小計
                    
                    };
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
                
                }
                if (cart.Count > 0)
                {
                    cart.RemoveAll(it => true);

                    string json = JsonSerializer.Serialize(cart);
                    HttpContext.Session.SetString(CDictionary.SK_PURCHASED_PRODUCTS_IN_SHOPPINGCART, json);
                }

                //依訂單扣除販售資訊及貨物的數量
                order.Decrease_Goods_Count_By_Orders();

            }

            return RedirectToAction("Page", "Home");
        }
        
    }
}
