using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Member
    {
        public Member()
        {
            AdminReplies = new HashSet<AdminReply>();
            Collects = new HashSet<Collect>();
            CustReports = new HashSet<CustReport>();
            Evaluations = new HashSet<Evaluation>();
            Orders = new HashSet<Order>();
            RamenStoreCollects = new HashSet<RamenStoreCollect>();
            RamenStores = new HashSet<RamenStore>();
        }

        public string TextBirthday
        {
            get => ((DateTime)Birthday).ToShortDateString();
        }

        public int MemberIdPk { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int DistrictIdFk { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public string ConnectionId { get; set; }

        public virtual District DistrictIdFkNavigation { get; set; }
        public virtual ICollection<AdminReply> AdminReplies { get; set; }
        public virtual ICollection<Collect> Collects { get; set; }
        public virtual ICollection<CustReport> CustReports { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<RamenStoreCollect> RamenStoreCollects { get; set; }
        public virtual ICollection<RamenStore> RamenStores { get; set; }
    }
}
