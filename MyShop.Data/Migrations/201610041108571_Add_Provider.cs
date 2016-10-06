namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Provider : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Vendors", newName: "Providers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Providers", newName: "Vendors");
        }
    }
}
