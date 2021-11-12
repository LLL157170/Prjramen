using Microsoft.AspNetCore.SignalR;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Hubs
{
    public class ChatHub : Hub
    {
        //private readonly IUserConnectionManager _userConnectionManager;
        //public ChatHub(IUserConnectionManager userConnectionManager)
        //{
        //    _userConnectionManager = userConnectionManager;
        //}
        public async Task SendToUser(string user,string message,string recivename)
        {
            
            RamenSupermarketContext db = new RamenSupermarketContext();
 
            //會員
            var memberI = ((from x in db.Members
                            where x.Name == user
                            select x).FirstOrDefault());
            var employeetype = ((from x in db.Employees
                                 where x.Name == recivename
                                 select x).FirstOrDefault());
            //員工
            var employeeI = ((from x in db.Employees
                              where x.Name == user
                              select x).FirstOrDefault());
            var membertype = ((from x in db.Members
                                where x.Name == recivename
                                select x).FirstOrDefault());

            if (memberI != null)
            {

                //if (employeetype.ConnectionId != null)
                //{
                //    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message, recivename);
                //    await Clients.Client(employeetype.ConnectionId).SendAsync("ReceiveMessage", user, message, recivename);
                //}
                //else
                //{
                    await Clients.All.SendAsync("ReceiveMessage", user, message, recivename);
                //}


            }
            else
            {

                if (membertype.ConnectionId != null)
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message, recivename);
                    await Clients.Client(membertype.ConnectionId).SendAsync("ReceiveMessage", user, message, recivename);
                }
                else
                {
                    await Clients.All.SendAsync("ReceiveMessage", user, message, recivename);
                }


            }

        }
        public string GetConnectionId() => Context.ConnectionId;

        public void save(string user)
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            var memberI = ((from x in db.Members
                            where x.Name == user
                            select x).FirstOrDefault());
            var employeeI = ((from x in db.Employees
                              where x.Name == user
                              select x).FirstOrDefault());

            if (memberI != null)
            {
                memberI.ConnectionId = Context.ConnectionId;
                db.SaveChanges();
            }
            else
            {
                employeeI.ConnectionId = Context.ConnectionId;
                db.SaveChanges();
            }

        }
 
        //public string GetConnectionId()
        //{
        //    var httpContext = this.Context.GetHttpContext();
        //    var userId = httpContext.Request.Query["userId"];
        //    _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

        //    return Context.ConnectionId;
        //}
        //public override Task OnConnectedAsync(CChatViewModel a)
        //{
        //    RamenSupermarketContext db = new RamenSupermarketContext();
            
            
        //    var name = Context.ConnectionId;
        //    var memberI = ((from x in db.Members
        //                      where a.UserName == x.Name
        //                      select x).FirstOrDefault());
        //    var employeeI = ((from x in db.Employees
        //                      where a.UserName == x.Name
        //                      select x).FirstOrDefault());
        //    if (a.UserName == memberI.Name)
        //    {
        //        Member member = new Member()
        //        {   
        //            MemberIdPk = memberI.MemberIdPk,
        //            Name = memberI.Name,
        //            Phone = memberI.Phone,
        //            DistrictIdFk = memberI.DistrictIdFk,
        //            Address = memberI.Address,
        //            EMail = memberI.EMail,
        //            Password = memberI.Password,
        //            Birthday = memberI.Birthday,
        //            ConnectionId = Context.ConnectionId
        //        };
        //    db.Members.Update(member);
        //    db.SaveChanges();
        //    }
        //    else
        //    {
        //        if (name == employeeI.Name)
        //        {
        //            Employee employee = new Employee()
        //        {
        //            EmployeeIdPk = employeeI.EmployeeIdPk,
        //            Name = employeeI.Name,
        //            Password = employeeI.Password,
        //            Birthday = employeeI.Birthday,
        //            ConnectionId = Context.ConnectionId
        //        };
        //            db.Employees.Update(employee);
        //            db.SaveChanges();
        //        }
        //    }
           
        //    return base.OnConnectedAsync();
            
        //}
 
    }
}
