using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryDetails = new HashSet<CategoryDetail>();
        }

        public int CategoryIdPk { get; set; }
        public string CategoryName { get; set; }
        public string CatDescription { get; set; }
        public byte[] Picture { get; set; }

        public virtual ICollection<CategoryDetail> CategoryDetails { get; set; }
    }
}
