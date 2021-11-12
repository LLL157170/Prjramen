using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Controllers
{
    public class EMListController : Controller
    {
        public IActionResult EList()
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

        public IActionResult MList()
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

        public IActionResult Eorder(CEorderQueryViewModel x)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            {
                RamenSupermarketContext db = new RamenSupermarketContext();

                List<COrderViewModel> orderlist = new List<COrderViewModel>();
                if (string.IsNullOrEmpty(x.txtQuery))
                {
                    var orders = from p in db.Orders
                                 select p;
                    
                    foreach (Order order in orders)
                    {
                        COrderViewModel cOrderView = new COrderViewModel();
                        cOrderView.District = (from d in db.Districts
                                               where d.DistrictIdPk == order.DistrictIdFk
                                               select d.DistrictName).FirstOrDefault();

                        cOrderView.Payment = (from p in db.PaymentTypes
                                              where p.PaymentIdPk == order.PaymentFk
                                              select p.Payment).FirstOrDefault();

                        cOrderView.DeliveryStatus = (from d in db.DeliveryStatuses
                                                     where d.DeliveryStatusIdPk == order.DeliveryStatusIdFk
                                                     select d.DeliveryStatus1).FirstOrDefault();
                        var cityid = (from c in db.Districts
                                      where c.DistrictIdPk == order.DistrictIdFk
                                      select c.CityIdFk).FirstOrDefault();
                        cOrderView.City = (from c in db.Citys
                                           where c.CityIdPk == cityid
                                           select c.CityName).FirstOrDefault();
                        cOrderView.OrderIdPk = order.OrderIdPk;
                        cOrderView.OrderDate = order.OrderDate;
                        cOrderView.ShipDate = order.ShipDate;
                        cOrderView.ShipAddress = order.ShipAddress;
                        cOrderView.TotalPrice = order.TotalPrice;
                        cOrderView.membername = (from d in db.Members
                                                 where d.MemberIdPk == order.MemberIdFk
                                                 select d.Name).FirstOrDefault();
                        orderlist.Add(cOrderView);

                    }
                    return View(orderlist);
                }
                else
                {
                    var h = (from p in db.Members
                             where p.Name.Contains(x.txtQuery)
                             select p.MemberIdPk);

                    foreach (var id in h)
                    {
                        var orders = from p in db.Orders
                                     where p.MemberIdFk == id
                                     select p;
                        foreach (Order order in orders)
                        {
                            COrderViewModel cOrderView = new COrderViewModel();
                            cOrderView.District = (from d in db.Districts
                                                   where d.DistrictIdPk == order.DistrictIdFk
                                                   select d.DistrictName).FirstOrDefault();

                            cOrderView.Payment = (from p in db.PaymentTypes
                                                  where p.PaymentIdPk == order.PaymentFk
                                                  select p.Payment).FirstOrDefault();

                            cOrderView.DeliveryStatus = (from d in db.DeliveryStatuses
                                                         where d.DeliveryStatusIdPk == order.DeliveryStatusIdFk
                                                         select d.DeliveryStatus1).FirstOrDefault();
                            var cityid = (from c in db.Districts
                                          where c.DistrictIdPk == order.DistrictIdFk
                                          select c.CityIdFk).FirstOrDefault();
                            cOrderView.City = (from c in db.Citys
                                               where c.CityIdPk == cityid
                                               select c.CityName).FirstOrDefault();
                            cOrderView.OrderIdPk = order.OrderIdPk;
                            cOrderView.OrderDate = order.OrderDate;
                            cOrderView.ShipDate = order.ShipDate;
                            cOrderView.ShipAddress = order.ShipAddress;
                            cOrderView.TotalPrice = order.TotalPrice;
                            cOrderView.membername = (from d in db.Members
                                                     where d.MemberIdPk == order.MemberIdFk
                                                     select d.Name).FirstOrDefault();
                            orderlist.Add(cOrderView);

                        }
                    }
                        return View(orderlist);
                } 

            }
            return RedirectToAction("Login", "EMLogin");
        }

        public IActionResult Editorder(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Eorder");
            }
            RamenSupermarketContext db = new RamenSupermarketContext();
            var x = db.Orders.FirstOrDefault(p => p.OrderIdPk == id);
            if (x == null)
            {
                return RedirectToAction("Eorder");
            }
            return View(x);
        }

        [HttpPost]
        public IActionResult Editorder(CEorderView p)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            Order order = db.Orders.FirstOrDefault(row => row.OrderIdPk == p.OidPk);

            if (order == null) return RedirectToAction("EList");

            order.ShipDate = p.shipdate;
            order.ShipAddress = p.address;
            order.DistrictIdFk = p.dis;

            db.SaveChanges();

            return RedirectToAction("Eorder");
        }
        public IActionResult Deleteorder(int? id)
        {
            if (id != null)
            {
                RamenSupermarketContext db = new RamenSupermarketContext();
                var x = db.Orders.FirstOrDefault(p => p.OrderIdPk== id);

                var y = from n in db.OrderDetails
                        where x.OrderIdPk == n.OrderIdFk
                        select n;
                if (x != null)
                {
                    foreach(var f in y)
                    {
                        db.OrderDetails.Remove(f);
                    }

                    db.Orders.Remove(x);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Eorder");
        }
        public IActionResult EOrderDetail(int? orderId)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            if (orderId != null)
            {

                var orders = from p in db.OrderDetails
                             where p.OrderIdFk == orderId
                             select p;
                List<COrderDetailViewModel> orderlist = new List<COrderDetailViewModel>();

                foreach (OrderDetail order in orders)
                {
                    COrderDetailViewModel ODView = new COrderDetailViewModel();
                    ODView.OrderDetailsIdPk = order.OrderDetailsIdPk;
                    ODView.OrderIdFk = order.OrderIdFk;
                    int proinfoid = (int)(from s in db.SalesInfos
                                          where s.SalesInfoIdPk == order.SalesInfoIdFk
                                          select s.ProductIdFk).FirstOrDefault();
                    ODView.SalesInfo = (from p in db.ProductDetails
                                        where p.ProductIdPk == proinfoid
                                        select p.ProductName).FirstOrDefault();
                    ODView.Counts = order.Counts;
                    ODView.Subtotal = order.Subtotal;
                    ODView.PictureBase64 = Convert.ToBase64String((from p in db.ProductDetails
                                                                   where p.ProductIdPk == proinfoid
                                                                   select p.Picture).FirstOrDefault());
                    orderlist.Add(ODView);
                }
                return View(orderlist);

            }
            return RedirectToAction("Login", "EMLogin");

        }

        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Login(CEmployeeLoginViewModel a)
        //{
        //    RamenSupermarketContext db = new RamenSupermarketContext();
        //    Employee employee = (from m in db.Employees
        //                         where m.Name == a.txName && m.Password == a.txPassword
        //                         select m).FirstOrDefault();
        //    if (employee != null)
        //    {
        //        if (employee.Password.Equals(a.txPassword))
        //        {
        //            string json = JsonSerializer.Serialize(employee);
        //            HttpContext.Session.SetString(CDictionary.SK_LOGIN_EMPLOYEE, json);
        //            return RedirectToAction("List", "ProductDetail");
        //        }

        //    }
        //    else
        //    {
        //        ViewBag.loginerror = "帳號密碼錯誤";
        //    }
        //    return View();
        //}
        public IActionResult LogOff()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            {
                HttpContext.Session.Remove(CDictionary.SK_LOGIN_EMPLOYEE);
            }
            //else
            //{
            //    return RedirectToAction("Login","EMLogin");
            //}
            return RedirectToAction("Login", "EMLogin");
        }


        //public IActionResult Alist(string a)
        //{
        //    RamenSupermarketContext db = new RamenSupermarketContext();
        //    IEnumerable<Employee> datas = null;
        //    if (string.IsNullOrEmpty(a))
        //    {
        //        datas = from p in db.Employees
        //                select p;
        //    }
        //    else
        //    {
        //        datas = db.Employees.Where(p => p.Name.Contains(a));
        //        ViewBag.keyword = a;
        //    }
        //    return Json(datas);
        //}
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(Employee p)
        //{
        //    RamenSupermarketContext db = new RamenSupermarketContext();
        //    db.Employees.Add(p);
        //    db.SaveChanges();
        //    return RedirectToAction("List");
        //}

        //public IActionResult Edit(int? id)
        //{
        //    if (id != null)
        //    {
        //        RamenSupermarketContext db = new RamenSupermarketContext();
        //        Employee x = db.Employees.FirstOrDefault(x => x.EmployeeIdPk == id);
        //        return View(x);
        //    }
        //    return RedirectToAction("List");
        //}
        //[HttpPost]
        //public IActionResult Edit(Employee e)
        //{
        //    RamenSupermarketContext db = new RamenSupermarketContext();
        //    Employee x = db.Employees.FirstOrDefault(p => p.EmployeeIdPk == e.EmployeeIdPk);
        //    x.Name = e.Name;
        //    x.Password = e.Password;
        //    x.Birthday = e.Birthday;
        //    db.SaveChanges();
        //    return RedirectToAction("List");
        //}

        //public IActionResult Delete(int? id)
        //{
        //    RamenSupermarketContext db = new RamenSupermarketContext();
        //    Employee x = db.Employees.FirstOrDefault(p => p.EmployeeIdPk == id);
        //    db.Employees.Remove(x);
        //    db.SaveChanges();
        //    return RedirectToAction("List");
        //}

        public IActionResult Get_Employee_Name()
        {
            Employee employee = new Employee();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_EMPLOYEE);
                employee = JsonSerializer.Deserialize<Employee>(json);

            }
            return Json(new { name = employee.Name });
        }

        public IActionResult Ranking()
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
    }
}
