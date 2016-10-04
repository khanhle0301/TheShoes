namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Image2ProductCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Image2", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Image2");
        }
    }
}
