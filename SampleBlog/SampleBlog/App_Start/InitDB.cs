using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SampleBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SampleBlog.App_Start
{
    public class InitDb : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed (ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)) ;
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));



        }

    }
}