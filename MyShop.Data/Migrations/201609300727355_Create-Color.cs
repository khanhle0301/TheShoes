namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateColor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Background = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductColors",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        ColorID = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.ColorID })
                .ForeignKey("dbo.Colors", t => t.ColorID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.ColorID);
            
            AddColumn("dbo.Products", "Colors", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductColors", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductColors", "ColorID", "dbo.Colors");
            DropIndex("dbo.ProductColors", new[] { "ColorID" });
            DropIndex("dbo.ProductColors", new[] { "ProductID" });
            DropColumn("dbo.Products", "Colors");
            DropTable("dbo.ProductColors");
            DropTable("dbo.Colors");
        }
    }
}
