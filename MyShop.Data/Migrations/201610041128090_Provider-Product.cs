namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProviderProduct : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Products", name: "VendorID", newName: "ProviderID");
            RenameIndex(table: "dbo.Products", name: "IX_VendorID", newName: "IX_ProviderID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Products", name: "IX_ProviderID", newName: "IX_VendorID");
            RenameColumn(table: "dbo.Products", name: "ProviderID", newName: "VendorID");
        }
    }
}
