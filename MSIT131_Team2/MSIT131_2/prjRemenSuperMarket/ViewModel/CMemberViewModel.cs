using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CMemberViewModel
    {
        public Member member { get; set; }
        public CMemberViewModel()
        {
            member = new Member();
        }
        public string TextBirthday
        {
            get => ((DateTime)Birthday).ToShortDateString();
        }
        RamenSupermarketContext db = new RamenSupermarketContext();
        public string District { get { return db.Districts.Find(DistrictIdFk).DistrictName; } }
        public int MemberIdPk { get { return member.MemberIdPk; } set { member.MemberIdPk = value; } }
        public string Name { get { return member.Name; } set { member.Name = value; } }
        public string Phone { get { return member.Phone; } set { member.Phone = value; } }
        public int DistrictIdFk { get { return member.DistrictIdFk; } set { member.DistrictIdFk = value; } }
        public string Address { get { return member.Address; } set { member.Address = value; } }
        public string EMail { get { return member.EMail; } set { member.EMail = value; } }
        public string Password { get { return member.Password; } set { member.Password = value; } }
        public DateTime Birthday { get { return member.Birthday; } set { member.Birthday = value; } }
        public string ConnectionId { get { return member.ConnectionId; } set { member.ConnectionId = value; } }

        public virtual District DistrictIdFkNavigation { get; set; }
        public virtual ICollection<AdminReply> AdminReplies { get; set; }
        public virtual ICollection<Collect> Collects { get; set; }
        public virtual ICollection<CustReport> CustReports { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<RamenStore> RamenStores { get; set; }



    }
}
