namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_Material : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductMaterials", "MaterialID", "dbo.Materials");
            DropIndex("dbo.ProductMaterials", new[] { "MaterialID" });
            DropPrimaryKey("dbo.Materials");
            DropPrimaryKey("dbo.ProductMaterials");
            AlterColumn("dbo.Materials", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ProductMaterials", "MaterialID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Materials", "ID");
            AddPrimaryKey("dbo.ProductMaterials", new[] { "ProductID", "MaterialID" });
            CreateIndex("dbo.ProductMaterials", "MaterialID");
            AddForeignKey("dbo.ProductMaterials", "MaterialID", "dbo.Materials", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductMaterials", "MaterialID", "dbo.Materials");
            DropIndex("dbo.ProductMaterials", new[] { "MaterialID" });
            DropPrimaryKey("dbo.ProductMaterials");
            DropPrimaryKey("dbo.Materials");
            AlterColumn("dbo.ProductMaterials", "MaterialID", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Materials", "ID", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddPrimaryKey("dbo.ProductMaterials", new[] { "ProductID", "MaterialID" });
            AddPrimaryKey("dbo.Materials", "ID");
            CreateIndex("dbo.ProductMaterials", "MaterialID");
            AddForeignKey("dbo.ProductMaterials", "MaterialID", "dbo.Materials", "ID", cascadeDelete: true);
        }
    }
}
