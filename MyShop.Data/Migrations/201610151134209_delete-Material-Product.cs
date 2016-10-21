namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteMaterialProduct : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Materials");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Materials", c => c.String());
        }
    }
}
