namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V08 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivationCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        IsUsed = c.Boolean(nullable: false),
                        UsingDate = c.DateTime(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivationCodes", "UserId", "dbo.Users");
            DropIndex("dbo.ActivationCodes", new[] { "UserId" });
            DropTable("dbo.ActivationCodes");
        }
    }
}
