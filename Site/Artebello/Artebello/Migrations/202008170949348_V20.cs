namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Gender", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Gender", c => c.Byte(nullable: false));
        }
    }
}
