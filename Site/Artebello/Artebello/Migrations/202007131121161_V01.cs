namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Order = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Order = c.Int(nullable: false),
                        Body = c.String(),
                        ImageUrl = c.String(),
                        BlogCategoryId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogCategories", t => t.BlogCategoryId, cascadeDelete: true)
                .Index(t => t.BlogCategoryId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProvinceId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsCenter = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provinces", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CityId = c.Guid(),
                        Address = c.String(),
                        PostalCode = c.String(),
                        Email = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        PaymentDate = c.DateTime(),
                        RefId = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RawAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 250),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        Body = c.String(),
                        ImageUrl = c.String(),
                        SellerId = c.Guid(),
                        ProductGroupId = c.Guid(),
                        DiscountAmount = c.Decimal(precision: 18, scale: 2),
                        IsInPromotion = c.Boolean(nullable: false),
                        IsInHome = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroupId)
                .ForeignKey("dbo.Sellers", t => t.SellerId)
                .Index(t => t.SellerId)
                .Index(t => t.ProductGroupId);
            
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        UrlParam = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sellers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        UserId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Password = c.String(maxLength: 150),
                        CellNum = c.String(nullable: false, maxLength: 20),
                        FullName = c.String(nullable: false, maxLength: 250),
                        Code = c.Int(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        Email = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ZarinpallAuthorities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Authority = c.String(),
                        OrderId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiscountCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        ExpireDate = c.DateTime(nullable: false),
                        IsPercent = c.Boolean(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsMultiUsing = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDiscounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        DiscountCodeId = c.Guid(nullable: false),
                        IsUse = c.Boolean(nullable: false),
                        UsingDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiscountCodes", t => t.DiscountCodeId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.DiscountCodeId);
            
            CreateTable(
                "dbo.Texts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Body = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDiscounts", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderDiscounts", "DiscountCodeId", "dbo.DiscountCodes");
            DropForeignKey("dbo.Cities", "ProvinceId", "dbo.Provinces");
            DropForeignKey("dbo.ZarinpallAuthorities", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Sellers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Products", "SellerId", "dbo.Sellers");
            DropForeignKey("dbo.Products", "ProductGroupId", "dbo.ProductGroups");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Blogs", "BlogCategoryId", "dbo.BlogCategories");
            DropIndex("dbo.OrderDiscounts", new[] { "DiscountCodeId" });
            DropIndex("dbo.OrderDiscounts", new[] { "OrderId" });
            DropIndex("dbo.ZarinpallAuthorities", new[] { "OrderId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Sellers", new[] { "UserId" });
            DropIndex("dbo.Products", new[] { "ProductGroupId" });
            DropIndex("dbo.Products", new[] { "SellerId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "CityId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Cities", new[] { "ProvinceId" });
            DropIndex("dbo.Blogs", new[] { "BlogCategoryId" });
            DropTable("dbo.Texts");
            DropTable("dbo.OrderDiscounts");
            DropTable("dbo.DiscountCodes");
            DropTable("dbo.Provinces");
            DropTable("dbo.ZarinpallAuthorities");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Sellers");
            DropTable("dbo.ProductGroups");
            DropTable("dbo.Products");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.Cities");
            DropTable("dbo.Blogs");
            DropTable("dbo.BlogCategories");
        }
    }
}
