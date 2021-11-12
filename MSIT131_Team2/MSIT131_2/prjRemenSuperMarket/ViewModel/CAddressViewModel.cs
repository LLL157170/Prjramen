using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CAddressViewModel
    {
        [DisplayName]
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string Address { get; set; }
    }
}
