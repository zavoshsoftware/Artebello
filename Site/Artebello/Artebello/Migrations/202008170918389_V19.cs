namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BirthDate", c => c.DateTime());
            AddColumn("dbo.Users", "BirthLocation", c => c.String());
            AddColumn("dbo.Users", "Gender", c => c.Byte(nullable: false));
            AddColumn("dbo.Users", "Phone", c => c.String());
            AddColumn("dbo.Users", "ZipCode", c => c.String());
            AddColumn("dbo.Users", "CurrentProvince", c => c.String());
            AddColumn("dbo.Users", "Address", c => c.String());
            AddColumn("dbo.Products", "Dimensions", c => c.String());
            AddColumn("dbo.Products", "Material", c => c.String());
            AddColumn("dbo.Products", "Weight", c => c.Int());
            AddColumn("dbo.Sellers", "Education", c => c.String());
            AddColumn("dbo.Sellers", "ArtField", c => c.String());
            AddColumn("dbo.Sellers", "ParticipatingDomesticExhibitions", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sellers", "ParticipatingForeignExhibitions", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sellers", "MethodOfIntroduction", c => c.String());
            DropColumn("dbo.Products", "Width");
            DropColumn("dbo.Products", "Height");
            DropColumn("dbo.Products", "Size");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Size", c => c.Int());
            AddColumn("dbo.Products", "Height", c => c.Int());
            AddColumn("dbo.Products", "Width", c => c.Int());
            DropColumn("dbo.Sellers", "MethodOfIntroduction");
            DropColumn("dbo.Sellers", "ParticipatingForeignExhibitions");
            DropColumn("dbo.Sellers", "ParticipatingDomesticExhibitions");
            DropColumn("dbo.Sellers", "ArtField");
            DropColumn("dbo.Sellers", "Education");
            DropColumn("dbo.Products", "Weight");
            DropColumn("dbo.Products", "Material");
            DropColumn("dbo.Products", "Dimensions");
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "CurrentProvince");
            DropColumn("dbo.Users", "ZipCode");
            DropColumn("dbo.Users", "Phone");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Users", "BirthLocation");
            DropColumn("dbo.Users", "BirthDate");
        }
    }
}
