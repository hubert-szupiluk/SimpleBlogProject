using SampleBlog.DAL;
using SampleBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleBlog.Controllers
{
    public class AuthorizeController : Controller
    {
        // GET: Authorize
        public ActionResult Index()
        {
            return View();
        }

        public bool PostAuthorize(int PostID)
        {
            bool result = false;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                using (BlogContext Blogcontext = new BlogContext())
                {
                    var controller = DependencyResolver.Current.GetService<HomeController>();
                    controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                    User us = controller.GetActualUser();
                    if (us == null)
                    {
                        return false;
                    }
                    var post = Blogcontext.Posts.SingleOrDefault(p => p.PostId == PostID);
                    if (us.Type == AccountType.Moderator || post.UserID == us.UserId)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public bool CommentAuthorize(int CommentID)
        {
            bool result = false;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                using (BlogContext Blogcontext = new BlogContext())
                {
                    var controller = DependencyResolver.Current.GetService<HomeController>();
                    controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                    User us = controller.GetActualUser();
                    if (us == null)
                    {
                        return false;
                    }
                    var comm = Blogcontext.Comments.SingleOrDefault(p => p.CommentId == CommentID);
                    if (us.Type == AccountType.Moderator || comm.UserID == us.UserId)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public bool RateAuthorize()
        {
            bool result = false;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var controller = DependencyResolver.Current.GetService<HomeController>();
                controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                User us = controller.GetActualUser();
                if (us == null)
                {
                    return false;
                }
                if (us.Type == AccountType.Reader)
                {
                    result = true;
                }
            }

            return result;
        }

        public ActionResult NotAuthorizeView (object o)
        {
            return View();
        }

        public bool WriteAuthorize()
        {
            bool result = false;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                var controller = DependencyResolver.Current.GetService<HomeController>();
                controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                User us = controller.GetActualUser();
                if (us == null)
                {
                    return false;
                }
                if (us.Type == AccountType.Writer)
                {
                    result = true;
                }
            }

            return result;
        }

        public bool LoggedInAuthorize ()
        {
            var controller = DependencyResolver.Current.GetService<HomeController>();
            controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
            User us = controller.GetActualUser();
            if (us!= null)
            {
                return true;
            } else
            {
                return false;
            }
        }

    }
}