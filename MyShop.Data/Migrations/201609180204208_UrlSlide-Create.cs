namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrlSlideCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slides", "Url", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slides", "Url");
        }
    }
}
