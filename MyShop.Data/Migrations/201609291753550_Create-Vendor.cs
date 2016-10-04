namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateVendor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 500),
                        DisplayOrder = c.Int(),
                        Image = c.String(maxLength: 256),
                        HomeFlag = c.Boolean(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        MetaKeyword = c.String(maxLength: 256),
                        MetaDescription = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Products", "VendorID", c => c.Int());
            CreateIndex("dbo.Products", "VendorID");
            AddForeignKey("dbo.Products", "VendorID", "dbo.Vendors", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "VendorID", "dbo.Vendors");
            DropIndex("dbo.Products", new[] { "VendorID" });
            DropColumn("dbo.Products", "VendorID");
            DropTable("dbo.Vendors");
        }
    }
}
