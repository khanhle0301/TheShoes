namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FooterEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Footers", "Name", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Footers", "Name");
        }
    }
}
