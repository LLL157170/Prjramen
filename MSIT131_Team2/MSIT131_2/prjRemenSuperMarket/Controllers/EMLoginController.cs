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

namespace prjRemenSuperMarket.Controllers
{
    public class EMLoginController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly RamenSupermarketContext _RSContext;
        public EMLoginController(IWebHostEnvironment hostEnv, RamenSupermarketContext RSContext)
        {
            _hostingEnv = hostEnv;
            _RSContext = RSContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(CEmployeeLoginViewModel a)
        {
            //RamenSupermarketContext db = new RamenSupermarketContext();
            Employee employee = (from m in _RSContext.Employees
                                 where m.Name == a.txName && m.Password == a.txPassword
                                 select m).FirstOrDefault();
            if (employee != null)
            {
                if (employee.Password.Equals(a.txPassword))
                {
                    string json = JsonSerializer.Serialize(employee);
                    HttpContext.Session.SetString(CDictionary.SK_LOGIN_EMPLOYEE, json);

                    if(Request.Query["lastAction"].ToString() != "")
                    {
                        string Controller = Request.Query["lastAction"].ToString().Split('/')[1];
                        string Action = Request.Query["lastAction"].ToString().Split('/')[2];
                        return RedirectToAction(Action, Controller);
                    }

                    return RedirectToAction("List", "ProductDetail");
                }

            }
            else
            {
                ViewBag.loginerror = "帳號密碼錯誤";
            }
            return View();
        }
    }
}
