namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductHeel_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Heels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductHeels",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        HeelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.HeelId })
                .ForeignKey("dbo.Heels", t => t.HeelId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.HeelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductHeels", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductHeels", "HeelId", "dbo.Heels");
            DropIndex("dbo.ProductHeels", new[] { "HeelId" });
            DropIndex("dbo.ProductHeels", new[] { "ProductId" });
            DropTable("dbo.ProductHeels");
            DropTable("dbo.Heels");
        }
    }
}
