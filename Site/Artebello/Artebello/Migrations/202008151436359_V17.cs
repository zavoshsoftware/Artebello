namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sellers", "ResumeUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sellers", "ResumeUrl");
        }
    }
}
