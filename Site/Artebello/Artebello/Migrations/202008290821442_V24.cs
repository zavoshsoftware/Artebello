namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sellers", "HeaderUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sellers", "HeaderUrl");
        }
    }
}
