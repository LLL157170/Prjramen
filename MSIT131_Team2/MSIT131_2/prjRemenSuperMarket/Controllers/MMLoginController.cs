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
    public class MMLoginController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly RamenSupermarketContext _RSContext;
        public MMLoginController(IWebHostEnvironment hostEnv, RamenSupermarketContext RSContext)
        {
            _hostingEnv = hostEnv;
            _RSContext = RSContext;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(CLoginViewModel model)
        {
            Member member = (from m in _RSContext.Members
                             where m.EMail == model.txtAccount && m.Password == model.txtPassword
                             select m).FirstOrDefault();
            if (member != null)
            {
                if (member.Password.Equals(model.txtPassword))
                {
                    string json = JsonSerializer.Serialize(member);
                    HttpContext.Session.SetString(CDictionary.SK_LOGIN_USER, json);

                    if (Request.Query["lastAction"].ToString() != "")
                    {
                        string Controller = Request.Query["lastAction"].ToString().Split('/')[1];
                        string Action = Request.Query["lastAction"].ToString().Split('/')[2];
                        return RedirectToAction(Action, Controller);
                    }

                    return RedirectToAction("Page", "Home");
                }
                else
                {
                    ViewBag.loginerror = "帳號密碼錯誤";
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
