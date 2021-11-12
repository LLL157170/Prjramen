using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CShoppingCart
    {
        public SalesInfo product { get; set; }
        public string productName { get; set; }
        public string PictureBase64 { get; set; }
        public int productId { get; set; }        
        public int count { get; set; }
        public double price { get; set; }
        public double discount { get; set; }
        public decimal 小計 { get { return Convert.ToDecimal(this.price) * this.count; } }
    }
}
