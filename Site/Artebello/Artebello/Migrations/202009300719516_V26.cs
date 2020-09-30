namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V26 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsLetters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
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
            DropTable("dbo.NewsLetters");
        }
    }
}
