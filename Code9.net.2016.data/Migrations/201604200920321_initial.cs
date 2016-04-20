namespace Code9.net._2016.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Table = c.Int(nullable: false),
                        CheckedOut = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        PersonServing_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employees", t => t.PersonServing_ID)
                .Index(t => t.PersonServing_ID);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Fulfilled = c.Boolean(nullable: false),
                        Bill_ID = c.Int(),
                        Item_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Bills", t => t.Bill_ID)
                .ForeignKey("dbo.MenuItems", t => t.Item_ID)
                .Index(t => t.Bill_ID)
                .Index(t => t.Item_ID);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(),
                        Kind = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "PersonServing_ID", "dbo.Employees");
            DropForeignKey("dbo.OrderItems", "Item_ID", "dbo.MenuItems");
            DropForeignKey("dbo.OrderItems", "Bill_ID", "dbo.Bills");
            DropIndex("dbo.OrderItems", new[] { "Item_ID" });
            DropIndex("dbo.OrderItems", new[] { "Bill_ID" });
            DropIndex("dbo.Bills", new[] { "PersonServing_ID" });
            DropTable("dbo.Employees");
            DropTable("dbo.MenuItems");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Bills");
        }
    }
}
