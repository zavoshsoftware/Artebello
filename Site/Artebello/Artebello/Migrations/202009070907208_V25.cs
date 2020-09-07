namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsAvailable", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Products", "Weight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Weight", c => c.Int());
            DropColumn("dbo.Products", "IsAvailable");
        }
    }
}
