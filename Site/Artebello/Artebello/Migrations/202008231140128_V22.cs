﻿namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductGroups", "HeaderUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductGroups", "HeaderUrl");
        }
    }
}
