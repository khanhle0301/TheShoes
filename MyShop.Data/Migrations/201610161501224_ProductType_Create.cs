namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductType_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductTypes",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.TypeId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Types", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductTypes", "TypeId", "dbo.Types");
            DropForeignKey("dbo.ProductTypes", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductTypes", new[] { "TypeId" });
            DropIndex("dbo.ProductTypes", new[] { "ProductId" });
            DropTable("dbo.Types");
            DropTable("dbo.ProductTypes");
        }
    }
}
