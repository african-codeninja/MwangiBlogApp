namespace MwangiBlogApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MwangiBlogApp.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MwangiBlogApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MwangiBlogApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            #region rolemanager

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));



            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            #endregion

            //I need to creat a few users that will eventually occupy the roles of either Admin or Moderator

            #region usermanager

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "mosesmwangi@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "mosesmwangi@Mailinator.com",
                    Email = "mosesmwangi@Mailinator.com",
                    FirstName = "Moses",
                    LastName = "Mwangi",
                    DisplayName = "Boss",
                }, "Abc&123");

                if (!context.Users.Any(u => u.Email == "Joeshmoe@Mailinator.com"))
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "Joeshmoe@Mailinator.com",
                        Email = "Joeshmoe@Mailinator.com",
                        FirstName = "Joe",
                        LastName = "Shmoe",
                        DisplayName = "Twitch",
                    }, "Abc&123");
                }
            }

            #endregion

            var userId = userManager.FindByEmail("mosesmwangi@Mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");

            userId = userManager.FindByEmail("joeshmoe@Mailinator.com").Id;
            userManager.AddToRole(userId, "Moderator");

        }

    }
}
