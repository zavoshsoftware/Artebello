namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sellers", "Summery", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sellers", "Summery");
        }
    }
}
