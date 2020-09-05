namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V02 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductMediums",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductOrientations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductThemes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "Width", c => c.Int());
            AddColumn("dbo.Products", "Height", c => c.Int());
            AddColumn("dbo.Products", "Size", c => c.Int());
            AddColumn("dbo.Products", "CreateYear", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ProductThemeId", c => c.Guid());
            AddColumn("dbo.Products", "ProductTypeId", c => c.Guid());
            AddColumn("dbo.Products", "ProductMediumId", c => c.Guid());
            AddColumn("dbo.Products", "ProductOrientationId", c => c.Guid());
            CreateIndex("dbo.Products", "ProductThemeId");
            CreateIndex("dbo.Products", "ProductTypeId");
            CreateIndex("dbo.Products", "ProductMediumId");
            CreateIndex("dbo.Products", "ProductOrientationId");
            AddForeignKey("dbo.Products", "ProductMediumId", "dbo.ProductMediums", "Id");
            AddForeignKey("dbo.Products", "ProductOrientationId", "dbo.ProductOrientations", "Id");
            AddForeignKey("dbo.Products", "ProductThemeId", "dbo.ProductThemes", "Id");
            AddForeignKey("dbo.Products", "ProductTypeId", "dbo.ProductTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProductTypeId", "dbo.ProductTypes");
            DropForeignKey("dbo.Products", "ProductThemeId", "dbo.ProductThemes");
            DropForeignKey("dbo.Products", "ProductOrientationId", "dbo.ProductOrientations");
            DropForeignKey("dbo.Products", "ProductMediumId", "dbo.ProductMediums");
            DropIndex("dbo.Products", new[] { "ProductOrientationId" });
            DropIndex("dbo.Products", new[] { "ProductMediumId" });
            DropIndex("dbo.Products", new[] { "ProductTypeId" });
            DropIndex("dbo.Products", new[] { "ProductThemeId" });
            DropColumn("dbo.Products", "ProductOrientationId");
            DropColumn("dbo.Products", "ProductMediumId");
            DropColumn("dbo.Products", "ProductTypeId");
            DropColumn("dbo.Products", "ProductThemeId");
            DropColumn("dbo.Products", "CreateYear");
            DropColumn("dbo.Products", "Size");
            DropColumn("dbo.Products", "Height");
            DropColumn("dbo.Products", "Width");
            DropTable("dbo.ProductTypes");
            DropTable("dbo.ProductThemes");
            DropTable("dbo.ProductOrientations");
            DropTable("dbo.ProductMediums");
        }
    }
}
