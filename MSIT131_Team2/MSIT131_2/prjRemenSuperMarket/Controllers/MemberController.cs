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

namespace FPdemo03.Controllers
{
    public class MemberController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly RamenSupermarketContext _RSContext;
        public MemberController(IWebHostEnvironment hostEnv, RamenSupermarketContext RSContext)
        {
            _hostingEnv = hostEnv;
            _RSContext = RSContext;
        }
        public IActionResult City()
        {
            var cities = from p in _RSContext.Citys
                         select p;

            //var cities = _RSContext.Citys.Select(row => new { Id = row.CityIdPk, Name = row.CityName });

            return Json(cities);
        }
        public IActionResult District(int id)
        {
            var Districts = from p in _RSContext.Districts
                            where p.CityIdFk == id
                            select p;

            //var Districts = _RSContext.Districts.Where(row=>row.CityIdFk == cityID).Select(row => new { Id = row.DistrictIdPk, Name = row.DistrictName });

            return Json(Districts);

        }

        public IActionResult OrderDetail(int? orderId)
        {
            if (orderId != null)
            {

                var orders = from p in _RSContext.OrderDetails
                             where p.OrderIdFk == orderId
                             select p;
                List<COrderDetailViewModel> orderlist = new List<COrderDetailViewModel>();

                foreach (OrderDetail order in orders)
                {
                    COrderDetailViewModel ODView = new COrderDetailViewModel();
                    ODView.OrderDetailsIdPk = order.OrderDetailsIdPk;
                    ODView.OrderIdFk = order.OrderIdFk;
                    int proinfoid = (int)(from s in _RSContext.SalesInfos
                                          where s.SalesInfoIdPk == order.SalesInfoIdFk
                                          select s.ProductIdFk).FirstOrDefault();
                    ODView.SalesInfo = (from p in _RSContext.ProductDetails
                                        where p.ProductIdPk == proinfoid
                                        select p.ProductName).FirstOrDefault();
                    ODView.Counts = order.Counts;
                    ODView.Subtotal = order.Subtotal;
                    ODView.PictureBase64 = Convert.ToBase64String((from p in _RSContext.ProductDetails
                                            where p.ProductIdPk == proinfoid
                                            select p.Picture).FirstOrDefault());
                    orderlist.Add(ODView);
                }
                return View(orderlist);

            }
            return RedirectToAction("Login", "MMLogin");

        }
        public IActionResult AddressEdit(int? id)
        {
            if (id != null)
            {
                return View();
            }
            return RedirectToAction("Login", "MMLogin");
        }
        [HttpPost]
        public IActionResult AddressEdit(CAddressViewModel cAddress)
        {

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member cust = JsonSerializer.Deserialize<Member>(json);
                var mem = (from m in _RSContext.Members
                           where m.MemberIdPk == cust.MemberIdPk
                           select m).FirstOrDefault();
                var city = (from c in _RSContext.Citys
                            where c.CityName == cAddress.CityName
                            select c.CityIdPk).FirstOrDefault();
                var dist = (from d in _RSContext.Districts
                            where d.CityIdFk == city && d.DistrictName == cAddress.DistrictName
                            select d.DistrictIdPk).FirstOrDefault();
                mem.DistrictIdFk = dist;
                mem.Address = cAddress.Address;
                _RSContext.SaveChanges();
                return RedirectToAction("MemberMain");
            }


            return RedirectToAction("Loign", "MMLogin");

        }
        public IActionResult PasswordEditCheck()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                return View();
            }
            return RedirectToAction("Login", "MMLogin");
        }
        [HttpPost]
        public IActionResult PasswordEditCheck(string password)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member cust = JsonSerializer.Deserialize<Member>(json);
                string mempw = cust.Password;
                if (password == mempw)
                {
                    return RedirectToAction("PasswordEdit", new { id = cust.MemberIdPk });
                }
                else
                {

                    return View();
                }
            }
            return RedirectToAction("Loign", "MMLogin");
        }
        public IActionResult PasswordEdit(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                return View();
            }
            return RedirectToAction("Login", "MMLogin");
        }
        [HttpPost]
        public IActionResult PasswordEdit(string newpassword)
        {

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member cust = JsonSerializer.Deserialize<Member>(json);
                var mem = (from m in _RSContext.Members
                           where m.MemberIdPk == cust.MemberIdPk
                           select m).FirstOrDefault();
                mem.Password = newpassword;
                _RSContext.SaveChanges();
                return RedirectToAction("MemberMain");
            }

            return RedirectToAction("Loign", "MMLogin");
        }
        public IActionResult order(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member member = JsonSerializer.Deserialize<Member>(json);
                var orders = from p in _RSContext.Orders
                             where p.MemberIdFk == member.MemberIdPk
                             select p;
                List<COrderViewModel> orderlist = new List<COrderViewModel>();
                foreach (Order order in orders)
                {
                    COrderViewModel cOrderView = new COrderViewModel();
                    cOrderView.District = (from d in _RSContext.Districts
                                           where d.DistrictIdPk == order.DistrictIdFk
                                           select d.DistrictName).FirstOrDefault();

                    cOrderView.Payment = (from p in _RSContext.PaymentTypes
                                          where p.PaymentIdPk == order.PaymentFk
                                          select p.Payment).FirstOrDefault();

                    cOrderView.DeliveryStatus = (from d in _RSContext.DeliveryStatuses
                                                 where d.DeliveryStatusIdPk == order.DeliveryStatusIdFk
                                                 select d.DeliveryStatus1).FirstOrDefault();
                    var cityid = (from c in _RSContext.Districts
                                  where c.DistrictIdPk == order.DistrictIdFk
                                  select c.CityIdFk).FirstOrDefault();
                    cOrderView.City = (from c in _RSContext.Citys
                                       where c.CityIdPk == cityid
                                       select c.CityName).FirstOrDefault();
                    cOrderView.OrderIdPk = order.OrderIdPk;
                    cOrderView.OrderDate = order.OrderDate;
                    cOrderView.ShipDate = order.ShipDate;
                    cOrderView.ShipAddress = order.ShipAddress;
                    cOrderView.TotalPrice = order.TotalPrice;
                    orderlist.Add(cOrderView);

                }
                return View(orderlist);

            }
            return RedirectToAction("Login", "MMLogin");

        }
        public IActionResult collect()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member member = JsonSerializer.Deserialize<Member>(json);
                var collects = from c in _RSContext.Collects
                               where c.MemberIdFk == member.MemberIdPk
                               select c;
                List<CCollectViewModel> collectList = new List<CCollectViewModel>();
               
                foreach (Collect collect in collects)
                {
                    CCollectViewModel cCollect = new CCollectViewModel();
                    cCollect.CollectIdPk = (from p in _RSContext.Collects
                                            where p.CollectIdPk == collect.CollectIdPk
                                            select p.CollectIdPk).FirstOrDefault();
                    cCollect.ProductInfoIFk = (from p in _RSContext.ProductDetails
                                               where p.ProductIdPk == collect.ProductIdFk
                                               select p.ProductIdPk).FirstOrDefault();
                    cCollect.Product = (from p in _RSContext.ProductDetails
                                        where p.ProductIdPk == collect.ProductIdFk
                                        select p.ProductName).FirstOrDefault();
                    cCollect.CollectType = (from c in _RSContext.CollectTypes
                                            where c.CollectTypeIdPk == collect.CollectTypeIdFk
                                            select c.CollectType1).FirstOrDefault();
                    cCollect.PictureBase64 = Convert.ToBase64String((from p in _RSContext.ProductDetails
                                              where p.ProductIdPk == collect.ProductIdFk
                                              select p.Picture).FirstOrDefault());
                    collectList.Add(cCollect);

                }
                return View(collectList);

            }
            return RedirectToAction("Login", "MMLogin");

        }
        public IActionResult DeleteCollect(int? id)
        {
            if (id != null)
            {
                RamenSupermarketContext db = new RamenSupermarketContext();
                var x = db.Collects.FirstOrDefault(p => p.CollectIdPk == id);
                if (x != null)
                {
                    db.Collects.Remove(x);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("collect");
        }
        public IActionResult storecollect()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member member = JsonSerializer.Deserialize<Member>(json);
                var collects = from c in _RSContext.RamenStoreCollects
                               where c.MemberId == member.MemberIdPk
                               select c;
                List<CRamenStoreCollect> collectList = new List<CRamenStoreCollect>();

                foreach (RamenStoreCollect collect in collects)
                {
                    CRamenStoreCollect cstoreCollect = new CRamenStoreCollect();
                    cstoreCollect.StoreCollectId = (from p in _RSContext.RamenStoreCollects
                                            where p.CollectId == collect.CollectId
                                            select p.CollectId).FirstOrDefault();
                    cstoreCollect.StoreId = (from p in _RSContext.RamenStores
                                               where p.RamenStoreId == collect.StoreId
                                               select p.RamenStoreId).FirstOrDefault();
                    cstoreCollect.StoreName = (from p in _RSContext.RamenStores
                                          where p.RamenStoreId == collect.StoreId
                                          select p.StoreName).FirstOrDefault();
                    cstoreCollect.PictureBase64 = Convert.ToBase64String((from p in _RSContext.RamenStores
                                                   where p.RamenStoreId == collect.StoreId
                                                   select p.Logo).FirstOrDefault());
                    cstoreCollect.Phone = (from p in _RSContext.RamenStores
                                          where p.RamenStoreId == collect.StoreId
                                          select p.Phone).FirstOrDefault();
                    cstoreCollect.Address = (from p in _RSContext.RamenStores
                                           where p.RamenStoreId == collect.StoreId
                                           select p.Address).FirstOrDefault();

                    RamenStore storeModel = _RSContext.RamenStores.FirstOrDefault(p => p.RamenStoreId == collect.StoreId);
                    City cityModel = _RSContext.Citys.FirstOrDefault(p => p.CityIdPk == storeModel.CityId);
                    cstoreCollect.City = cityModel.CityName;

                    District districtModel = _RSContext.Districts.FirstOrDefault(p => p.DistrictIdPk == storeModel.DistrictId);
                    cstoreCollect.District = districtModel.DistrictName;

                    collectList.Add(cstoreCollect);

                }
                return View(collectList);

            }
            return RedirectToAction("Login", "MMLogin");

        }
        public IActionResult DeleteStoreCollect(int? id)
        {
            if (id != null)
            {
                RamenSupermarketContext db = new RamenSupermarketContext();
                var x = db.RamenStoreCollects.FirstOrDefault(p => p.CollectId == id);
                if (x != null)
                {
                    db.RamenStoreCollects.Remove(x);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("storecollect");

        }
        public IActionResult MemberMain()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member cust = JsonSerializer.Deserialize<Member>(json);
                ViewBag.kk = cust.MemberIdPk;
                ViewBag.userName = cust.Name;
                return View();
            }
            return RedirectToAction("Login", "MMLogin");
        }
        
        public IActionResult MemberEdit()
        {           

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member cust = JsonSerializer.Deserialize<Member>(json);
                var cityid = (from c in _RSContext.Districts
                              where c.DistrictIdPk == cust.DistrictIdFk
                              select c.CityIdFk).FirstOrDefault();
                var cityname= (from c in _RSContext.Citys
                               where c.CityIdPk == cityid
                               select c.CityName).FirstOrDefault();
                var distname = (from c in _RSContext.Districts
                                where c.DistrictIdPk == cust.DistrictIdFk
                                select c.DistrictName).FirstOrDefault();
                ViewBag.longaddress = cityname + distname + cust.Address;
                return View(cust);
                
            }
            return RedirectToAction("Login", "MMLogin");

        }
        [HttpPost]
        public IActionResult MemberEdit(CRegister member)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member cust = JsonSerializer.Deserialize<Member>(json);
                Member x = _RSContext.Members.FirstOrDefault(p => p.MemberIdPk == cust.MemberIdPk);
                x.Name = member.Name;
                x.EMail = member.EMail;
                x.Phone = member.Phone;
                var CityFk = ((from p in _RSContext.Citys
                               where p.CityName == member.CityName
                               select p.CityIdPk).ToList())[0];
                int DistrictIdFk = ((from p in _RSContext.Districts
                                     where p.DistrictName == member.DistrictName && p.CityIdFk == CityFk
                                     select p.DistrictIdPk).ToList())[0];
                if (member.Address != null)
                {
                    x.DistrictIdFk = DistrictIdFk;
                    x.Address = member.Address;
                }
                x.Birthday = member.Birthday;
                _RSContext.SaveChanges();
                string jsonedit = JsonSerializer.Serialize(x);
                HttpContext.Session.SetString(CDictionary.SK_LOGIN_USER, jsonedit);


                return RedirectToAction("MemberEdit", "Member");
            }


            return RedirectToAction("Login", "MMLogin");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(CRegister cRegister)
        {
            Member newmem = new Member();
            newmem.Name = cRegister.Name;
            newmem.EMail = cRegister.EMail;
            newmem.Phone = cRegister.Phone;
            var CityFk = ((from p in _RSContext.Citys
                           where p.CityName == cRegister.CityName
                           select p.CityIdPk).ToList())[0];
            int DistrictIdFk = ((from p in _RSContext.Districts
                                 where p.DistrictName == cRegister.DistrictName && p.CityIdFk == CityFk
                                 select p.DistrictIdPk).ToList())[0];
            newmem.DistrictIdFk = DistrictIdFk;
            newmem.Address = cRegister.Address;
            newmem.Password = cRegister.Password;
            newmem.Birthday = cRegister.Birthday;
            _RSContext.Members.Add(newmem);
            _RSContext.SaveChanges();


            return RedirectToAction("Login", "MMLogin");
        }
        public IActionResult List(CQueryViewModel Query)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            if (string.IsNullOrEmpty(Query.txtQuery))
            {
                var datas = from p in db.Members
                            select p;
                return View(datas);
            }
            else
            {
                var datas = db.Members.Where(p => p.Name.Contains(Query.txtQuery) || p.Phone.Contains(Query.txtQuery) || p.Address.Contains(Query.txtQuery) || p.EMail.Contains(Query.txtQuery));
                ViewBag.keyword = Query.txtQuery;
                return View(datas);
            }
        }

        public IActionResult Create()
        {
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("List");
            }
            RamenSupermarketContext db = new RamenSupermarketContext();
            var x = db.Members.FirstOrDefault(p => p.MemberIdPk == id);
            if (x == null)
            {
                return RedirectToAction("List");
            }
            return View(x);

        }
        [HttpPost]
        public IActionResult Edit(Member e)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var x = db.Members.FirstOrDefault(p => p.MemberIdPk == e.MemberIdPk);
            if (x != null)
            {
                x.Name = e.Name;
                x.Phone = e.Phone;
                x.DistrictIdFk = e.MemberIdPk;
                x.Address = e.Address;
                x.EMail = e.EMail;
                x.Password = e.Password;
                x.Birthday = e.Birthday;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                RamenSupermarketContext db = new RamenSupermarketContext();
                var x = db.Members.FirstOrDefault(p => p.MemberIdPk == id);
                if (x != null)
                {
                    db.Members.Remove(x);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        public IActionResult Get_Login_User()
        {
            Member cust = new Member();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                cust = JsonSerializer.Deserialize<Member>(json);
                
            }
            return Json(new { name = cust.Name });
        }
        public IActionResult LogOff()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                HttpContext.Session.Remove(CDictionary.SK_LOGIN_USER);
            }
            else
            {
                return RedirectToAction("Login", "MMLogin");
            }
            return RedirectToAction("page","Home" );
        }
        public IActionResult Load_member_List()
        {
            var list = new RamenSupermarketContext().Members.Select(row => new { Name = row.Name, Id = row.MemberIdPk });

            return Json(list);
        }


        
    }
}
