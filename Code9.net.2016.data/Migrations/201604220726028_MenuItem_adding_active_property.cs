namespace Code9.net._2016.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MenuItem_adding_active_property : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItems", "Active", c => c.Boolean(nullable: false));
            Sql("UPDATE dbo.MenuItems SET Active = 1");
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItems", "Active");
        }
    }
}
