using System;
using System.Collections.Generic;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class Evaluation
    {
        public int EvaluationIdPk { get; set; }
        public int? MemberIdFk { get; set; }
        public int? ProductIdFk { get; set; }
        public int? Evaluation1 { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }

        public virtual Member MemberIdFkNavigation { get; set; }
        public virtual ProductDetail ProductIdFkNavigation { get; set; }
    }
}
