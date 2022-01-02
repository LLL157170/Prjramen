using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrjTest
{
    class TeamInformation1
    {
        public partial class TeamInformation
        {
            public int TeamID { get; set; }
            public string TramName { get; set; }
            public string City { get; set; }
            public Nullable<int> AreaID { get; set; }
            public string CreatYear { get; set; }
            public byte[] Picture { get; set; }
        }
    }
}
