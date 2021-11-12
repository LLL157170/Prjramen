using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.Controllers
{
    public class PushDemoController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly RamenSupermarketContext _RSContext;
        public PushDemoController(IWebHostEnvironment hostEnv, RamenSupermarketContext RSContext)
        {
            _hostingEnv = hostEnv;
            _RSContext = RSContext;
        }
        public IActionResult Index()
        {

            List<RamenProductInfo> pushDemos = new List<RamenProductInfo>();
            pushDemos.Add(new RamenProductInfo
            {
                ProductPicture = System.IO.File.ReadAllBytes(_hostingEnv.WebRootPath + "/images/201.jpg"),//照片，string to byte
                RamenStoreId = 33,
                ProductName = "豚骨拉麵",
                Price = 180
            });

            pushDemos.Add(new RamenProductInfo
            {
                ProductPicture = System.IO.File.ReadAllBytes(_hostingEnv.WebRootPath + "/images/202.jpg"),
                RamenStoreId = 33,
                ProductName = "地獄拉麵",
                Price = 190
            });

            pushDemos.Add(new RamenProductInfo
            {
                ProductPicture = System.IO.File.ReadAllBytes(_hostingEnv.WebRootPath + "/images/203.jpg"),
                RamenStoreId = 33,
                ProductName = "蔬菜拉麵",
                Price = 180
            });

            pushDemos.Add(new RamenProductInfo
            {
                ProductPicture = System.IO.File.ReadAllBytes(_hostingEnv.WebRootPath + "/images/204.jpg"),
                RamenStoreId = 33,
                ProductName = "叉燒拉麵",
                Price = 180
            });

            pushDemos.Add(new RamenProductInfo
            {
                ProductPicture = System.IO.File.ReadAllBytes(_hostingEnv.WebRootPath + "/images/205.jpg"),
                RamenStoreId = 33,
                ProductName = "鮮蝦拉麵",
                Price = 180
            });

            foreach (var item in pushDemos)
            {
                _RSContext.RamenProductInfos.Add(item);
            }
            _RSContext.SaveChanges();

            return NoContent();
        }
    }
}
