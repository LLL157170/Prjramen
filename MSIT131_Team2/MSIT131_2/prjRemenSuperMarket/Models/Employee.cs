using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Employee
    {
        public Employee()
        {
            AdminReplies = new HashSet<AdminReply>();
            Goods = new HashSet<Good>();
        }

        public string TextBirthday
        {
            get => ((DateTime)Birthday).ToShortDateString();
        }

        public int EmployeeIdPk { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime? Birthday { get; set; }
        public string ConnectionId { get; set; }

        public virtual ICollection<AdminReply> AdminReplies { get; set; }
        public virtual ICollection<Good> Goods { get; set; }
    }
}
