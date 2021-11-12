using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            Collects = new HashSet<Collect>();
            Evaluations = new HashSet<Evaluation>();
            Goods = new HashSet<Good>();
            SalesInfos = new HashSet<SalesInfo>();
        }

        public int ProductIdPk { get; set; }
        public int? CategoryDetailIdFk { get; set; }
        public string ProductName { get; set; }
        public byte[] Picture { get; set; }
        public string Specification { get; set; }
        public string Weight { get; set; }
        public string Origin { get; set; }
        public int? StorageIdFk { get; set; }
        public int? Storagedays { get; set; }
        public string ProductDescription { get; set; }
        public int? Views { get; set; }

        public virtual CategoryDetail CategoryDetailIdFkNavigation { get; set; }
        public virtual StorageMethod StorageIdFkNavigation { get; set; }
        public virtual ICollection<Collect> Collects { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
        public virtual ICollection<SalesInfo> SalesInfos { get; set; }
    }
}
