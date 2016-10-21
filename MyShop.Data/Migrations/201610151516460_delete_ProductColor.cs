namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete_ProductColor : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Colors");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Colors", c => c.String());
        }
    }
}
