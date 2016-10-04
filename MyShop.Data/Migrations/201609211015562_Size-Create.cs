namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SizeCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductSizes",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        SizeID = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.SizeID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.SizeID);
            
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductSizes", "SizeID", "dbo.Sizes");
            DropForeignKey("dbo.ProductSizes", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductSizes", new[] { "SizeID" });
            DropIndex("dbo.ProductSizes", new[] { "ProductID" });
            DropTable("dbo.Sizes");
            DropTable("dbo.ProductSizes");
        }
    }
}
