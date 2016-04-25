namespace Code9.net._2016.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(),
                        Kind = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Delivered = c.Boolean(nullable: false),
                        Table = c.Int(nullable: false),
                        Payed = c.Boolean(nullable: false),
                        Item_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuItems", t => t.Item_ID)
                .Index(t => t.Item_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "Item_ID", "dbo.MenuItems");
            DropIndex("dbo.OrderItems", new[] { "Item_ID" });
            DropTable("dbo.OrderItems");
            DropTable("dbo.MenuItems");
        }
    }
}
