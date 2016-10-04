namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrlBannerCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Banners", "Url", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Banners", "Url");
        }
    }
}
