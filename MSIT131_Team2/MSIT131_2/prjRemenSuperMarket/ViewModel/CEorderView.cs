using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CEorderView
    {
        public int OidPk { get; set; }
        public DateTime? shipdate { get; set; }
        public int dis { get; set; }
        public string address { get; set; }
    }
}
