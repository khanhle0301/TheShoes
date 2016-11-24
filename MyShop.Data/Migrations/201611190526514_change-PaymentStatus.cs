namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePaymentStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.String());
        }
    }
}
