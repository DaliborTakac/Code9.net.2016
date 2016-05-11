namespace Code9.net._2016.data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Code9.net._2016.data.RestourantContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Code9.net._2016.data.RestourantContext context)
        {
            if (context.MenuItems.Count() == 0)
            {
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Coffee",
                    Kind = Entities.MenuItemKind.DRINK,
                    Price = 199.99,
                    Active = true
                });
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Juice",
                    Kind = Entities.MenuItemKind.DRINK,
                    Price = 250.00,
                    Active = true
                });
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Beer",
                    Kind = Entities.MenuItemKind.DRINK,
                    Price = 230.00,
                    Active = true
                });
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Vodka",
                    Kind = Entities.MenuItemKind.DRINK,
                    Price = 270.00,
                    Active = true
                });
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Cappuccino",
                    Kind = Entities.MenuItemKind.DRINK,
                    Price = 210.00,
                    Active = true
                });
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Burger",
                    Kind = Entities.MenuItemKind.MEAL,
                    Price = 350.00,
                    Active = true
                });
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Cesar salad",
                    Kind = Entities.MenuItemKind.MEAL,
                    Price = 370.00,
                    Active = true
                });
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Capricciosa",
                    Kind = Entities.MenuItemKind.MEAL,
                    Price = 450.00,
                    Active = true
                });
                context.MenuItems.AddOrUpdate(new Entities.MenuItem()
                {
                    DisplayName = "Vegetariana",
                    Kind = Entities.MenuItemKind.MEAL,
                    Price = 430.00,
                    Active = true
                });
            }
        }
    }
}
