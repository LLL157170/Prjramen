using Microsoft.AspNetCore.Mvc;
using prjRemenSuperMarket.Models;
using prjRemenSuperMarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Controllers
{
    public class RamenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Reservation()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            RamenSupermarketContext db = new RamenSupermarketContext();
            IEnumerable<RamenStore> rs = db.RamenStores;
            List<CRamenStore> list = new List<CRamenStore>();
            foreach(var item in rs)
            {
                CRamenStore ra = new CRamenStore();
                ra.ramenStore = item;
                list.Add(ra);
            }
            return View(list);
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Blog_Detail()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
