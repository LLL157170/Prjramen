using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace prjRemenSuperMarket.Models
{
    public partial class RamenSupermarketContext : DbContext
    {
        public RamenSupermarketContext()
        {
        }

        public RamenSupermarketContext(DbContextOptions<RamenSupermarketContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminReply> AdminReplies { get; set; }
        public virtual DbSet<Category> Categorys { get; set; }
        public virtual DbSet<CategoryDetail> CategoryDetails { get; set; }
        public virtual DbSet<City> Citys { get; set; }
        public virtual DbSet<Collect> Collects { get; set; }
        public virtual DbSet<CollectType> CollectTypes { get; set; }
        public virtual DbSet<CustReport> CustReports { get; set; }
        public virtual DbSet<DeliveryStatus> DeliveryStatuses { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Evaluation> Evaluations { get; set; }
        public virtual DbSet<Good> Goods { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<PriceFactor> PriceFactors { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<ProductStatus> ProductStatuses { get; set; }
        public virtual DbSet<RamenProductInfo> RamenProductInfos { get; set; }
        public virtual DbSet<RamenStore> RamenStores { get; set; }
        public virtual DbSet<RamenStoreCollect> RamenStoreCollects { get; set; }
        public virtual DbSet<SalesInfo> SalesInfos { get; set; }
        public virtual DbSet<SalesState> SalesStates { get; set; }
        public virtual DbSet<StorageMethod> StorageMethods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=RamenSupermarket;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<AdminReply>(entity =>
            {
                entity.HasKey(e => e.AdminReplyIdPk);

                entity.ToTable("AdminReply");

                entity.Property(e => e.AdminReplyIdPk).HasColumnName("AdminReplyID(PK)");

                entity.Property(e => e.AdminTime).HasColumnType("date");

                entity.Property(e => e.EmployeeIdFk).HasColumnName("EmployeeID(FK)");

                entity.Property(e => e.MemberIdFk).HasColumnName("MemberID(FK)");

                entity.Property(e => e.ReplyContent).HasMaxLength(50);

                entity.HasOne(d => d.EmployeeIdFkNavigation)
                    .WithMany(p => p.AdminReplies)
                    .HasForeignKey(d => d.EmployeeIdFk)
                    .HasConstraintName("FK_AdminReply_Employees");

                entity.HasOne(d => d.MemberIdFkNavigation)
                    .WithMany(p => p.AdminReplies)
                    .HasForeignKey(d => d.MemberIdFk)
                    .HasConstraintName("FK_AdminReply_Members");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryIdPk);

                entity.Property(e => e.CategoryIdPk).HasColumnName("CategoryID(PK)");

                entity.Property(e => e.CatDescription).HasMaxLength(50);

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.Picture).HasColumnType("image");
            });

            modelBuilder.Entity<CategoryDetail>(entity =>
            {
                entity.HasKey(e => e.CategoryDetailIdPk);

                entity.Property(e => e.CategoryDetailIdPk).HasColumnName("CategoryDetailID(PK)");

                entity.Property(e => e.CatDetailDescription).HasMaxLength(50);

                entity.Property(e => e.CategoryDetailName).HasMaxLength(50);

                entity.Property(e => e.CategoryIdFk).HasColumnName("CategoryID(FK)");

                entity.Property(e => e.Picture).HasColumnType("image");

                entity.HasOne(d => d.CategoryIdFkNavigation)
                    .WithMany(p => p.CategoryDetails)
                    .HasForeignKey(d => d.CategoryIdFk)
                    .HasConstraintName("FK_CategoryDetails_Categorys");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityIdPk);

                entity.Property(e => e.CityIdPk).HasColumnName("CityID(PK)");

                entity.Property(e => e.CityName).HasMaxLength(50);
            });

            modelBuilder.Entity<Collect>(entity =>
            {
                entity.HasKey(e => e.CollectIdPk);

                entity.ToTable("Collect");

                entity.Property(e => e.CollectIdPk).HasColumnName("CollectID(PK)");

                entity.Property(e => e.CollectTypeIdFk).HasColumnName("CollectTypeID(FK)");

                entity.Property(e => e.MemberIdFk).HasColumnName("MemberID(FK)");

                entity.Property(e => e.ProductIdFk).HasColumnName("ProductID(FK)");

                entity.HasOne(d => d.CollectTypeIdFkNavigation)
                    .WithMany(p => p.Collects)
                    .HasForeignKey(d => d.CollectTypeIdFk)
                    .HasConstraintName("FK_Collect_CollectType");

                entity.HasOne(d => d.MemberIdFkNavigation)
                    .WithMany(p => p.Collects)
                    .HasForeignKey(d => d.MemberIdFk)
                    .HasConstraintName("FK_Collect_Members");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.Collects)
                    .HasForeignKey(d => d.ProductIdFk)
                    .HasConstraintName("FK_Collect_ProductDetails");
            });

            modelBuilder.Entity<CollectType>(entity =>
            {
                entity.HasKey(e => e.CollectTypeIdPk);

                entity.ToTable("CollectType");

                entity.Property(e => e.CollectTypeIdPk).HasColumnName("CollectTypeID(PK)");

                entity.Property(e => e.CollectType1)
                    .HasMaxLength(50)
                    .HasColumnName("CollectType");
            });

            modelBuilder.Entity<CustReport>(entity =>
            {
                entity.HasKey(e => e.CustReportIdPk);

                entity.ToTable("CustReport");

                entity.Property(e => e.CustReportIdPk).HasColumnName("CustReportID(PK)");

                entity.Property(e => e.CustTime).HasColumnType("date");

                entity.Property(e => e.MemberIdFk).HasColumnName("MemberID(FK)");

                entity.Property(e => e.ReportContent).HasMaxLength(50);

                entity.HasOne(d => d.MemberIdFkNavigation)
                    .WithMany(p => p.CustReports)
                    .HasForeignKey(d => d.MemberIdFk)
                    .HasConstraintName("FK_CustReport_Members");
            });

            modelBuilder.Entity<DeliveryStatus>(entity =>
            {
                entity.HasKey(e => e.DeliveryStatusIdPk);

                entity.ToTable("DeliveryStatus");

                entity.Property(e => e.DeliveryStatusIdPk).HasColumnName("DeliveryStatusID(PK)");

                entity.Property(e => e.DeliveryStatus1)
                    .HasMaxLength(50)
                    .HasColumnName("DeliveryStatus");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(e => e.DiscountIdPk);

                entity.ToTable("Discount");

                entity.Property(e => e.DiscountIdPk).HasColumnName("DiscountID(PK)");

                entity.Property(e => e.DiscountName).HasMaxLength(50);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.DistrictIdPk);

                entity.ToTable("District");

                entity.Property(e => e.DistrictIdPk).HasColumnName("DistrictID(PK)");

                entity.Property(e => e.CityIdFk).HasColumnName("CityID(FK)");

                entity.Property(e => e.DistrictName).HasMaxLength(50);

                entity.HasOne(d => d.CityIdFkNavigation)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityIdFk)
                    .HasConstraintName("FK_District_Citys");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeIdPk);

                entity.Property(e => e.EmployeeIdPk).HasColumnName("EmployeeID(PK)");

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(16)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.HasKey(e => e.EvaluationIdPk);

                entity.ToTable("Evaluation");

                entity.Property(e => e.EvaluationIdPk).HasColumnName("EvaluationID(PK)");

                entity.Property(e => e.Content).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Evaluation1).HasColumnName("Evaluation");

                entity.Property(e => e.MemberIdFk).HasColumnName("MemberID(FK)");

                entity.Property(e => e.ProductIdFk).HasColumnName("ProductID(FK)");

                entity.HasOne(d => d.MemberIdFkNavigation)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.MemberIdFk)
                    .HasConstraintName("FK_Evaluation_Members");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.ProductIdFk)
                    .HasConstraintName("FK_Evaluation_ProductDetails");
            });

            modelBuilder.Entity<Good>(entity =>
            {
                entity.HasKey(e => e.GoodsIdPk);

                entity.Property(e => e.GoodsIdPk).HasColumnName("GoodsID(PK)");

                entity.Property(e => e.EmployeeIdFk).HasColumnName("EmployeeID(FK)");

                entity.Property(e => e.LaunchDate).HasColumnType("date");

                entity.Property(e => e.MerchantIdFk).HasColumnName("MerchantID(FK)");

                entity.Property(e => e.ProductIdFk).HasColumnName("ProductID(FK)");

                entity.Property(e => e.ProductStatusIdFk).HasColumnName("ProductStatusID(FK)");

                entity.Property(e => e.PurchaseCost).HasColumnType("money");

                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.Property(e => e.ShelfLife).HasColumnType("date");

                entity.HasOne(d => d.EmployeeIdFkNavigation)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.EmployeeIdFk)
                    .HasConstraintName("FK_Goods_Employees");

                entity.HasOne(d => d.MerchantIdFkNavigation)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.MerchantIdFk)
                    .HasConstraintName("FK_Goods_Merchants");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.ProductIdFk)
                    .HasConstraintName("FK_Goods_ProductDetails");

                entity.HasOne(d => d.ProductStatusIdFkNavigation)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.ProductStatusIdFk)
                    .HasConstraintName("FK_Goods_ProductStatus");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.MemberIdPk);

                entity.Property(e => e.MemberIdPk).HasColumnName("MemberID(PK)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.DistrictIdFk).HasColumnName("DistrictID(FK)");

                entity.Property(e => e.EMail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("E-mail");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.DistrictIdFkNavigation)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.DistrictIdFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Members_District");
            });

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.HasKey(e => e.MerchantIdPk);

                entity.Property(e => e.MerchantIdPk).HasColumnName("MerchantID(PK)");

                entity.Property(e => e.BankAccount).HasMaxLength(50);

                entity.Property(e => e.MerchantAddress).HasMaxLength(50);

                entity.Property(e => e.MerchantName).HasMaxLength(50);

                entity.Property(e => e.MerchantPhone).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderIdPk);

                entity.Property(e => e.OrderIdPk).HasColumnName("OrderID(PK)");

                entity.Property(e => e.DeliveryStatusIdFk).HasColumnName("DeliveryStatusID(FK)");

                entity.Property(e => e.DistrictIdFk).HasColumnName("DistrictID(FK)");

                entity.Property(e => e.MemberIdFk).HasColumnName("MemberID(FK)");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentFk).HasColumnName("Payment(FK)");

                entity.Property(e => e.ShipAddress).HasMaxLength(50);

                entity.Property(e => e.ShipDate).HasColumnType("date");

                entity.Property(e => e.TotalPrice).HasColumnType("money");

                entity.HasOne(d => d.DeliveryStatusIdFkNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliveryStatusIdFk)
                    .HasConstraintName("FK_Orders_DeliveryStatus");

                entity.HasOne(d => d.DistrictIdFkNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DistrictIdFk)
                    .HasConstraintName("FK_Orders_District");

                entity.HasOne(d => d.MemberIdFkNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberIdFk)
                    .HasConstraintName("FK_Orders_Members");

                entity.HasOne(d => d.PaymentFkNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentFk)
                    .HasConstraintName("FK_Orders_PaymentType");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailsIdPk);

                entity.Property(e => e.OrderDetailsIdPk).HasColumnName("OrderDetailsID(PK)");

                entity.Property(e => e.OrderIdFk).HasColumnName("OrderID(FK)");

                entity.Property(e => e.SalesInfoIdFk).HasColumnName("SalesInfoID(FK)");

                entity.Property(e => e.Subtotal).HasColumnType("money");

                entity.HasOne(d => d.OrderIdFkNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderIdFk)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.SalesInfoIdFkNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.SalesInfoIdFk)
                    .HasConstraintName("FK_OrderDetails_SalesInfo");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasKey(e => e.PaymentIdPk);

                entity.ToTable("PaymentType");

                entity.Property(e => e.PaymentIdPk).HasColumnName("PaymentID(PK)");

                entity.Property(e => e.Payment).HasMaxLength(50);
            });

            modelBuilder.Entity<PriceFactor>(entity =>
            {
                entity.HasKey(e => e.PriceFactorIdPk);

                entity.ToTable("PriceFactor");

                entity.Property(e => e.PriceFactorIdPk).HasColumnName("PriceFactorID(PK)");

                entity.Property(e => e.PriceFactor1).HasColumnName("PriceFactor");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => e.ProductIdPk);

                entity.Property(e => e.ProductIdPk).HasColumnName("ProductID(PK)");

                entity.Property(e => e.CategoryDetailIdFk).HasColumnName("CategoryDetailID(FK)");

                entity.Property(e => e.Origin).HasMaxLength(50);

                entity.Property(e => e.Picture).HasColumnType("image");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.Specification).HasMaxLength(50);

                entity.Property(e => e.StorageIdFk).HasColumnName("StorageID(FK)");

                entity.Property(e => e.Weight)
                    .HasMaxLength(50)
                    .HasColumnName("weight");

                entity.HasOne(d => d.CategoryDetailIdFkNavigation)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.CategoryDetailIdFk)
                    .HasConstraintName("FK_ProductDetails_CategoryDetails");

                entity.HasOne(d => d.StorageIdFkNavigation)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.StorageIdFk)
                    .HasConstraintName("FK_ProductDetails_StorageMethod");
            });

            modelBuilder.Entity<ProductStatus>(entity =>
            {
                entity.HasKey(e => e.ProductStatusIdPk);

                entity.ToTable("ProductStatus");

                entity.Property(e => e.ProductStatusIdPk).HasColumnName("ProductStatusID(PK)");

                entity.Property(e => e.ProductStatus1)
                    .HasMaxLength(50)
                    .HasColumnName("ProductStatus");
            });

            modelBuilder.Entity<RamenProductInfo>(entity =>
            {
                entity.HasKey(e => e.RamenProductId);

                entity.ToTable("RamenProductInfo");

                entity.Property(e => e.RamenProductId).HasColumnName("RamenProductID");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ProductPicture).HasColumnType("image");

                entity.Property(e => e.RamenStoreId).HasColumnName("RamenStoreID");

                entity.HasOne(d => d.RamenStore)
                    .WithMany(p => p.RamenProductInfos)
                    .HasForeignKey(d => d.RamenStoreId)
                    .HasConstraintName("FK_RamenProductInfo_RamenStore");
            });

            modelBuilder.Entity<RamenStore>(entity =>
            {
                entity.ToTable("RamenStore");

                entity.Property(e => e.RamenStoreId).HasColumnName("RamenStoreID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.Property(e => e.Logo).HasColumnType("image");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Pictrue).HasColumnType("image");

                entity.Property(e => e.StoreName).HasMaxLength(50);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.RamenStores)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_RamenStore_Citys");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.RamenStores)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_RamenStore_District");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.RamenStores)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_RamenStore_Members");
            });

            modelBuilder.Entity<RamenStoreCollect>(entity =>
            {
                entity.HasKey(e => e.CollectId);

                entity.ToTable("RamenStoreCollect");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.RamenStoreCollects)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_RamenStoreCollect_Members");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.RamenStoreCollects)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_RamenStoreCollect_RamenStore");
            });

            modelBuilder.Entity<SalesInfo>(entity =>
            {
                entity.HasKey(e => e.SalesInfoIdPk);

                entity.ToTable("SalesInfo");

                entity.Property(e => e.SalesInfoIdPk).HasColumnName("SalesInfoID(PK)");

                entity.Property(e => e.DiscountIdFk).HasColumnName("DiscountID(FK)");

                entity.Property(e => e.GoodsIdFk).HasColumnName("GoodsID(FK)");

                entity.Property(e => e.PriceFactorFk).HasColumnName("PriceFactor(FK)");

                entity.Property(e => e.ProductIdFk).HasColumnName("ProductID(FK)");

                entity.Property(e => e.SalesStatesIdFk).HasColumnName("SalesStatesID(FK)");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.DiscountIdFkNavigation)
                    .WithMany(p => p.SalesInfos)
                    .HasForeignKey(d => d.DiscountIdFk)
                    .HasConstraintName("FK_SalesInfo_Discount");

                entity.HasOne(d => d.GoodsIdFkNavigation)
                    .WithMany(p => p.SalesInfos)
                    .HasForeignKey(d => d.GoodsIdFk)
                    .HasConstraintName("FK_SalesInfo_Goods");

                entity.HasOne(d => d.PriceFactorFkNavigation)
                    .WithMany(p => p.SalesInfos)
                    .HasForeignKey(d => d.PriceFactorFk)
                    .HasConstraintName("FK_SalesInfo_PriceFactor");

                entity.HasOne(d => d.ProductIdFkNavigation)
                    .WithMany(p => p.SalesInfos)
                    .HasForeignKey(d => d.ProductIdFk)
                    .HasConstraintName("FK_SalesInfo_ProductDetails");

                entity.HasOne(d => d.SalesStatesIdFkNavigation)
                    .WithMany(p => p.SalesInfos)
                    .HasForeignKey(d => d.SalesStatesIdFk)
                    .HasConstraintName("FK_SalesInfo_SalesStates");
            });

            modelBuilder.Entity<SalesState>(entity =>
            {
                entity.HasKey(e => e.SalesStatesIdPk);

                entity.Property(e => e.SalesStatesIdPk).HasColumnName("SalesStatesID(PK)");

                entity.Property(e => e.SalesStates).HasMaxLength(50);
            });

            modelBuilder.Entity<StorageMethod>(entity =>
            {
                entity.HasKey(e => e.StorageIdPk);

                entity.ToTable("StorageMethod");

                entity.Property(e => e.StorageIdPk).HasColumnName("StorageID(PK)");

                entity.Property(e => e.StorageName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
