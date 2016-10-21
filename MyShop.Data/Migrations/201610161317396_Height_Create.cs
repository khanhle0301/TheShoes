namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Height_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Heights",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductHeights",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        HeightId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.HeightId })
                .ForeignKey("dbo.Heights", t => t.HeightId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.HeightId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductHeights", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductHeights", "HeightId", "dbo.Heights");
            DropIndex("dbo.ProductHeights", new[] { "HeightId" });
            DropIndex("dbo.ProductHeights", new[] { "ProductId" });
            DropTable("dbo.ProductHeights");
            DropTable("dbo.Heights");
        }
    }
}
