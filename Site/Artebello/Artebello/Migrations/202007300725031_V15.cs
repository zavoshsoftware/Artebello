namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TextTypes", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TextTypes", "Name");
        }
    }
}
