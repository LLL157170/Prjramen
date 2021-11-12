using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using prjRemenSuperMarket.Hubs;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Controllers
{
    public class ChatController : Controller
    {
        //private readonly IHubContext<ChatHub> _ChatHub;
        //private readonly IUserConnectionManager _userConnectionManager;
        //public ChatController(IHubContext<ChatHub> ChatHub, IUserConnectionManager userConnectionManager)
        //{
        //    _ChatHub = ChatHub;
        //    _userConnectionManager = userConnectionManager;
        //}


        //public IActionResult ChatTest()
        //{
        //    if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
        //    {
        //        string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
        //        Member member = JsonSerializer.Deserialize<Member>(json);
        //        ViewBag.txtId = member.MemberIdPk;
        //        ViewBag.txtName = member.Name;

        //        ViewBag.recivename = "小黑";
        //    }
        //    else
        //    {
        //        if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
        //        {
        //            string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_EMPLOYEE);
        //            Employee employee = JsonSerializer.Deserialize<Employee>(json);
        //            ViewBag.txtId = employee.EmployeeIdPk;
        //            ViewBag.txtName = employee.Name;

        //            //var list = new RamenSupermarketContext().Members.Select(row => new { Id = row.MemberIdPk, Name = row.Name });
        //            //return Json(list);
        //        }
        //    }
        //    return View();
        //}

        

        public IActionResult Load_member_List()
        {
            var list = new RamenSupermarketContext().Members.Select(row => new { Name = row.Name,Id= row.MemberIdPk });

            return Json(list);
        }
        
        
        public IActionResult ChatTestlogin()
        {
            string namex = "";
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER);
                Member member = JsonSerializer.Deserialize<Member>(json);
                ViewBag.txtId = member.MemberIdPk;
                namex = member.Name;


            }
            else
            {
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
                {
                    string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_EMPLOYEE);
                    Employee employee = JsonSerializer.Deserialize<Employee>(json);
                    ViewBag.txtId = employee.EmployeeIdPk;
                    namex = employee.Name;

                    //var list = new RamenSupermarketContext().Members.Select(row => new { Id = row.MemberIdPk, Name = row.Name });
                    //return Json(list);
                }
            }
            return Json(new { name = namex});
        }
    }
}
