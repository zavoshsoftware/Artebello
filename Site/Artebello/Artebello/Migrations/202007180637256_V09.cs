namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V09 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int());
            AddColumn("dbo.Sellers", "ImageUrl", c => c.String());
            AddColumn("dbo.Sellers", "Body", c => c.String(storeType: "ntext"));
            AddColumn("dbo.Sellers", "StartDate", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sellers", "StartDate");
            DropColumn("dbo.Sellers", "Body");
            DropColumn("dbo.Sellers", "ImageUrl");
            DropColumn("dbo.Products", "Quantity");
        }
    }
}
