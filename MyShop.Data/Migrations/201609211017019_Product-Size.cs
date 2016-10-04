namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Sizes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Sizes");
        }
    }
}
