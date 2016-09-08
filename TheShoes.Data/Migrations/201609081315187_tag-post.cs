namespace TheShoes.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagpost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Tags", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Tags");
        }
    }
}
