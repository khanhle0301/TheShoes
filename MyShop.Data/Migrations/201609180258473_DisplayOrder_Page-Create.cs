namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisplayOrder_PageCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "DisplayOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "DisplayOrder");
        }
    }
}
