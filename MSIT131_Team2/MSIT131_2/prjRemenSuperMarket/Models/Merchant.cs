using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Merchant
    {
        public Merchant()
        {
            Goods = new HashSet<Good>();
        }

        public int MerchantIdPk { get; set; }
        public string MerchantName { get; set; }
        public string MerchantPhone { get; set; }
        public string MerchantAddress { get; set; }
        public string BankAccount { get; set; }

        public virtual ICollection<Good> Goods { get; set; }
    }
}
