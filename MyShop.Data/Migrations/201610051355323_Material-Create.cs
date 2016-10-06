namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaterialCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Materials", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Materials");
        }
    }
}
