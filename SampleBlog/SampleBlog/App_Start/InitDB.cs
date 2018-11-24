using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SampleBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleBlog.App_Start
{
  /*  public class InitDb : DropCreateDatabaseAlways<ApplicationDbContext>
    {
 
        protected override void Seed(ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var info = new User() { Address = "ExampleAdress", City = "ExampleCity", LastName = "JustLastName", Name = "JustName", PhoneNumber = "+00-888-888-888", Type = AccountType.Moderator };

          

            string roleName = "admin";
            string roleName2 = "user";
            string password = "Admin12!";
            UserManager.Create(new ApplicationUser()
            {
                UserName = "admin@admin.pl",
                Email = "admin@admin.pl",
                user = info

            }, password
            );
            if (!RoleManager.RoleExists(roleName))
            {
                var roleresult = RoleManager.Create(new IdentityRole(roleName));

            }

            if (!RoleManager.RoleExists(roleName2))
            {
                var roleresult = RoleManager.Create(new IdentityRole(roleName));

            }

            var info2 = new User   { Address = "TestUser", City = "TestUser", LastName = "TestUser", Name = "TestUser", PhoneNumber = "+00-000-000-000", Type = AccountType.Reader };
            var user = new ApplicationUser();

            user.UserName = "test@test.com";
            user.Email = "test@test.com";
            user.user = info2;
            var x = UserManager.Create(user, roleName2);
            if (x.Succeeded)
            {
                var roleresult = UserManager.AddToRole(user.Id , roleName);

            }

            base.Seed(context);
        }

    }*/
}