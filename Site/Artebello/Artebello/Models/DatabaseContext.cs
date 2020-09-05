using System;
using System.Collections.Generic;
using System.Data.Entity;
namespace Models
{
   public class DatabaseContext:DbContext
    {
        static DatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ZarinpallAuthority> ZarinpallAuthorities { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<OrderDiscount> OrderDiscounts { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductMedium> ProductMediums { get; set; }
        public DbSet<ProductOrientation> ProductOrientations { get; set; }
        public DbSet<ProductTheme> ProductThemes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ActivationCode> ActivationCodes { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<TextType> TextType { get; set; }



    }
}
