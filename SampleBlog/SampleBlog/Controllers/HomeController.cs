using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SampleBlog.DAL;
using SampleBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleBlog.Controllers
{

    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {

            var currentuser = GetActualUser();

            try
            {

                var info = string.Format($"Imie {currentuser.Name} Nazwisko {currentuser.LastName} Typ uzytkownika {currentuser.Type} ");
                ViewBag.ProfileInformation = info;


            }
            catch
            {
                Session.Abandon();
                Session.Clear();
                RedirectToAction("Index");
            }


            return View();
        }



        public User GetActualUser ()
        {

            var currentUserID = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserID);
            if (currentUser == null)
            {
                return null;
            }
            return currentUser.user;

        }

        public ApplicationUser GetActualApplicationUser()
        {

            var currentUserID = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserID);

            return currentUser;

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}