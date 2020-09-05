namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "HeaderImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "HeaderImageUrl");
        }
    }
}
