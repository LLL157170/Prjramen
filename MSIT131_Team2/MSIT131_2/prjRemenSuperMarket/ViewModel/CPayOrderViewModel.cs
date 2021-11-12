using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CPayOrderViewModel
    {
        public int OId { get; set; }
        public string OPhone { get; set; }
        public string OCheck { get; set; }
        public string OAddress { get; set; }
        public double OTotalPrice { get; set; }
        public int ODistrictId { get; set; }
    }
}
