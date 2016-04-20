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
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Code9.net._2016.data.RestourantContext context)
        {
            context.Workers.AddOrUpdate(new Entities.Employee()
            {
                Name = "Waiter 1",
                Role = Entities.EmployeeRole.WAITER
            });
            context.Workers.AddOrUpdate(new Entities.Employee()
            {
                Name = "Waiter 2",
                Role = Entities.EmployeeRole.WAITER
            });
            context.Workers.AddOrUpdate(new Entities.Employee()
            {
                Name = "Waiter 3",
                Role = Entities.EmployeeRole.WAITER
            });
            context.Workers.AddOrUpdate(new Entities.Employee()
            {
                Name = "Bartender 1",
                Role = Entities.EmployeeRole.BARTENDER
            });
            context.Workers.AddOrUpdate(new Entities.Employee()
            {
                Name = "Cook 1",
                Role = Entities.EmployeeRole.COOK
            });

            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Coffee",
                Kind = Entities.MenuItemKind.DRINK,
                Price = 199.99
            });
            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Juice",
                Kind = Entities.MenuItemKind.DRINK,
                Price = 250.00
            });
            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Beer",
                Kind = Entities.MenuItemKind.DRINK,
                Price = 230.00
            });
            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Vodka",
                Kind = Entities.MenuItemKind.DRINK,
                Price = 270.00
            });
            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Cappuccino",
                Kind = Entities.MenuItemKind.DRINK,
                Price = 210.00
            });
            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Burger",
                Kind = Entities.MenuItemKind.MEAL,
                Price = 350.00
            });
            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Cesar salad",
                Kind = Entities.MenuItemKind.MEAL,
                Price = 370.00
            });
            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Capricciosa",
                Kind = Entities.MenuItemKind.MEAL,
                Price = 450.00
            });
            context.MenuItems.AddOrUpdate(new Entities.MenuItem()
            {
                DisplayName = "Vegetariana",
                Kind = Entities.MenuItemKind.MEAL,
                Price = 430.00
            });
        }
    }
}
