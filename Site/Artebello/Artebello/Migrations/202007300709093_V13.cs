namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TextTypes",
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
            
            AddColumn("dbo.Texts", "Summery", c => c.String());
            AddColumn("dbo.Texts", "ImageUrl", c => c.String());
            AddColumn("dbo.Texts", "TextTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Texts", "TextTypeId");
            AddForeignKey("dbo.Texts", "TextTypeId", "dbo.TextTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Texts", "TextTypeId", "dbo.TextTypes");
            DropIndex("dbo.Texts", new[] { "TextTypeId" });
            DropColumn("dbo.Texts", "TextTypeId");
            DropColumn("dbo.Texts", "ImageUrl");
            DropColumn("dbo.Texts", "Summery");
            DropTable("dbo.TextTypes");
        }
    }
}
