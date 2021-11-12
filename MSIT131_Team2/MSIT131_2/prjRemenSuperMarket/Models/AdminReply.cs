using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class AdminReply
    {
        public int AdminReplyIdPk { get; set; }
        public int? MemberIdFk { get; set; }
        public int? EmployeeIdFk { get; set; }
        public string ReplyContent { get; set; }
        public DateTime? AdminTime { get; set; }

        public virtual Employee EmployeeIdFkNavigation { get; set; }
        public virtual Member MemberIdFkNavigation { get; set; }
    }
}
