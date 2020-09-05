namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "HeaderUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "HeaderUrl");
        }
    }
}
