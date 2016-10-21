namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_Sizes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductSizes", "SizeID", "dbo.Sizes");
            DropIndex("dbo.ProductSizes", new[] { "SizeID" });
            DropPrimaryKey("dbo.ProductSizes");
            DropPrimaryKey("dbo.Sizes");
            AlterColumn("dbo.ProductSizes", "SizeID", c => c.Int(nullable: false));
            AlterColumn("dbo.Sizes", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ProductSizes", new[] { "ProductID", "SizeID" });
            AddPrimaryKey("dbo.Sizes", "ID");
            CreateIndex("dbo.ProductSizes", "SizeID");
            AddForeignKey("dbo.ProductSizes", "SizeID", "dbo.Sizes", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductSizes", "SizeID", "dbo.Sizes");
            DropIndex("dbo.ProductSizes", new[] { "SizeID" });
            DropPrimaryKey("dbo.Sizes");
            DropPrimaryKey("dbo.ProductSizes");
            AlterColumn("dbo.Sizes", "ID", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.ProductSizes", "SizeID", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddPrimaryKey("dbo.Sizes", "ID");
            AddPrimaryKey("dbo.ProductSizes", new[] { "ProductID", "SizeID" });
            CreateIndex("dbo.ProductSizes", "SizeID");
            AddForeignKey("dbo.ProductSizes", "SizeID", "dbo.Sizes", "ID", cascadeDelete: true);
        }
    }
}
