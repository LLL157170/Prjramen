//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrjTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlayerInformation
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> Weight { get; set; }
        public string Country { get; set; }
        public Nullable<int> TeamID { get; set; }
        public byte[] Picture { get; set; }
    
        public virtual TeamInformation TeamInformation { get; set; }
    }
}
