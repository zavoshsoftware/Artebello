namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V14 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Texts", "Body", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Texts", "Body", c => c.String());
        }
    }
}
