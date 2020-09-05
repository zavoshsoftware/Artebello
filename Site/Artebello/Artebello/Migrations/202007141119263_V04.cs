namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V04 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductColors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        HexCode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "ProductColorId", c => c.Guid());
            CreateIndex("dbo.Products", "ProductColorId");
            AddForeignKey("dbo.Products", "ProductColorId", "dbo.ProductColors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProductColorId", "dbo.ProductColors");
            DropIndex("dbo.Products", new[] { "ProductColorId" });
            DropColumn("dbo.Products", "ProductColorId");
            DropTable("dbo.ProductColors");
        }
    }
}
