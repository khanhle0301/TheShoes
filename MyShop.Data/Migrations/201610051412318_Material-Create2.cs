namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaterialCreate2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductMaterials",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        MaterialID = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.MaterialID })
                .ForeignKey("dbo.Materials", t => t.MaterialID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.MaterialID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductMaterials", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductMaterials", "MaterialID", "dbo.Materials");
            DropIndex("dbo.ProductMaterials", new[] { "MaterialID" });
            DropIndex("dbo.ProductMaterials", new[] { "ProductID" });
            DropTable("dbo.ProductMaterials");
            DropTable("dbo.Materials");
        }
    }
}
