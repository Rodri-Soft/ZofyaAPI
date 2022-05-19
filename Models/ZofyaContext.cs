using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ZofyaApi.Models
{
    public partial class ZofyaContext : DbContext
    {
        public ZofyaContext()
        {
        }

        public ZofyaContext(DbContextOptions<ZofyaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Customer_Address> Customer_Addresses { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<ItemShoppingCart> ItemShoppingCarts { get; set; } = null!;
        public virtual DbSet<Item_Color> Item_Colors { get; set; } = null!;
        public virtual DbSet<Item_Size> Item_Sizes { get; set; } = null!;
        public virtual DbSet<Item_WishList> Item_WishLists { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<WishList> WishLists { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-G6HSTSE\\SQLEXPRESS;Database=Zofya;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.IDAddress);

                entity.ToTable("Address");

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Colony)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.InsideNumber)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.OutSideNumber)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.StreetName)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.IDUser);

                entity.ToTable("Customer");

                entity.Property(e => e.DateOfBith).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.IDShoppingCartNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.IDShoppingCart)
                    .HasConstraintName("FK_Customer_ShoppingCart");
            });

            modelBuilder.Entity<Customer_Address>(entity =>
            {
                entity.HasKey(e => e.IDCustomerAddress);

                entity.ToTable("Customer_Address");

                entity.HasOne(d => d.IDAddressNavigation)
                    .WithMany(p => p.Customer_Addresses)
                    .HasForeignKey(d => d.IDAddress)
                    .HasConstraintName("FK_Customer_Address_Address");

                entity.HasOne(d => d.IDUserNavigation)
                    .WithMany(p => p.Customer_Addresses)
                    .HasForeignKey(d => d.IDUser)
                    .HasConstraintName("FK_Customer_Address_Customer");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.SKU);

                entity.ToTable("Item");

                entity.Property(e => e.SKU)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Category)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ItemShoppingCart>(entity =>
            {
                entity.HasKey(e => e.IDItemShoppingCart);

                entity.ToTable("ItemShoppingCart");

                entity.Property(e => e.SKU)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.IDShoppingCartNavigation)
                    .WithMany(p => p.ItemShoppingCarts)
                    .HasForeignKey(d => d.IDShoppingCart)
                    .HasConstraintName("FK_ItemShoppingCart_ShoppingCart");

                entity.HasOne(d => d.SKUNavigation)
                    .WithMany(p => p.ItemShoppingCarts)
                    .HasForeignKey(d => d.SKU)
                    .HasConstraintName("FK_ItemShoppingCart_Item");
            });

            modelBuilder.Entity<Item_Color>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Item_Color");

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.SKU)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.SKUNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.SKU)
                    .HasConstraintName("FK_Item_Color_Item");
            });

            modelBuilder.Entity<Item_Size>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.SKU)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Size)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.SKUNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.SKU)
                    .HasConstraintName("FK_Item_Sizes_Item");
            });

            modelBuilder.Entity<Item_WishList>(entity =>
            {
                entity.HasKey(e => e.IDItemWishList);

                entity.ToTable("Item_WishList");

                entity.Property(e => e.IDItemWishList).ValueGeneratedNever();

                entity.Property(e => e.SKU)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.IDWishListNavigation)
                    .WithMany(p => p.Item_WishLists)
                    .HasForeignKey(d => d.IDWishList)
                    .HasConstraintName("FK_Item_WishList_WishList");

                entity.HasOne(d => d.SKUNavigation)
                    .WithMany(p => p.Item_WishLists)
                    .HasForeignKey(d => d.SKU)
                    .HasConstraintName("FK_Item_WishList_Item");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IDOrder);

                entity.ToTable("Order");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.OrderNumber)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.IDUserNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IDUser)
                    .HasConstraintName("FK_Order_Customer");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.IDShoppingCart);

                entity.ToTable("ShoppingCart");
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.HasKey(e => e.IDWishList);

                entity.ToTable("WishList");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.IDUserNavigation)
                    .WithMany(p => p.WishLists)
                    .HasForeignKey(d => d.IDUser)
                    .HasConstraintName("FK_WishList_Customer");
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.HasKey(e => e.RFC);

                entity.ToTable("Staff");

                entity.Property(e => e.RFC)
                    .HasMaxLength(13)
                    .IsFixedLength();

                entity.Property(e => e.CURP)
                    .HasMaxLength(18)
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Rol)
                    .HasMaxLength(20)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
