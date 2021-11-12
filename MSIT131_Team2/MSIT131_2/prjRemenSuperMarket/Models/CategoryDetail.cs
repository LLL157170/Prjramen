using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class CategoryDetail
    {
        public CategoryDetail()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public int CategoryDetailIdPk { get; set; }
        public int? CategoryIdFk { get; set; }
        public string CategoryDetailName { get; set; }
        public string CatDetailDescription { get; set; }
        public byte[] Picture { get; set; }

        public virtual Category CategoryIdFkNavigation { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
