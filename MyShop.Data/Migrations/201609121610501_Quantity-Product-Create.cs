namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuantityProductCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int());
            AddColumn("dbo.Products", "QuantitySold", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "QuantitySold");
            DropColumn("dbo.Products", "Quantity");
        }
    }
}
