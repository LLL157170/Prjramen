﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BasketBallEntities1 : DbContext
    {
        public BasketBallEntities1()
            : base("name=BasketBallEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<PlayerInformation> PlayerInformations { get; set; }
        public virtual DbSet<TeamInformation> TeamInformations { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Iteminformation> Iteminformations { get; set; }
    }
}
