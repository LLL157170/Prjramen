using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class CustReport
    {
        public int CustReportIdPk { get; set; }
        public int? MemberIdFk { get; set; }
        public string ReportContent { get; set; }
        public DateTime? CustTime { get; set; }

        public virtual Member MemberIdFkNavigation { get; set; }
    }
}
