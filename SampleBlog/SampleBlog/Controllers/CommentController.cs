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
    public class CommentController : Controller
    {
        // GET: Comment

       // [HttpPost]
        [Authorize]
        public ActionResult AddCommentForm(Post post )
        {
            ViewBag.PostID = post.PostId;
            return View();
        }
        public ActionResult EditCommentForm(Comment comment)
        {

            var controller = DependencyResolver.Current.GetService<AuthorizeController>();
            controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
            var us = controller.CommentAuthorize(comment.CommentId);


            if (us == true)
            {

                EditCommentModel model = new EditCommentModel() { CommentID = comment.CommentId, Content = comment.Content, Title = comment.Title };
                return View(model);
            } else
            {
                return View("~/Views/Authorize/NotAuthorizeView.cshtml");
            }
        }
        private void PairCommentWithPost(ref Post post , ref Comment comment)
        {
            comment.Post = post;
            post.comments.Add(comment);

        }
        [HttpPost]
        public ActionResult AddComment(AddCommentModel model)
        {
            var controller = DependencyResolver.Current.GetService<HomeController>();
            controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
            User user = controller.GetActualUser();
            Comment comment = null;
            Post post = null;
            using (BlogContext context = new BlogContext())
            {
                post = context.Posts.SingleOrDefault(p => p.PostId == model.Postid);
                comment = new Comment()
                {
                    Title = model.Title,
                    Content = model.Content,
                    UserID = user.UserId,
                    Post = post
                };
                PairCommentWithPost(ref post, ref comment);
                context.Comments.Add(comment);
                context.SaveChanges();
                return RedirectToAction("ViewPost", "Post", comment.Post);
               // return RedirectToAction("ViewSingleComment", new { @CommentID = comment.CommentId });
            }


        }
        public ActionResult DeleteComment(int? CommentID)
        {
            var controller = DependencyResolver.Current.GetService<AuthorizeController>();
            controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
            var us = controller.CommentAuthorize((int)CommentID);


            if (us == true)
            {

                using (BlogContext context = new BlogContext())
                {

                    Comment comment = context.Comments.SingleOrDefault(c => c.CommentId == CommentID);
                    int postid = comment.Post.PostId;
                    comment.Post.comments.Remove(comment);

                    comment.Post = null;
                    context.Comments.Attach(comment);
                    context.Comments.Remove(comment);
                    context.SaveChanges();

                    return RedirectToAction("ViewPost", "Post", new { postid });
                }
            } else
            {
                return View("~/Views/Authorize/NotAuthorizeView.cshtml");

            }
        }
        public ActionResult EditComment(EditCommentModel model)
        {
            using (BlogContext context = new BlogContext())
            {
                Comment comment = context.Comments.SingleOrDefault(c => c.CommentId == model.CommentID);
                comment.Title = model.Title;
                comment.Content = model.Content;
                context.SaveChanges();
                return RedirectToAction("ViewPost", "Post", new { comment.Post.PostId });
               // return RedirectToAction("ViewSingleComment", new { @CommentID = comment.CommentId });

            }
        }
        public ActionResult ViewSingleComment(int? CommentID)
        {
            using (BlogContext context = new BlogContext())
            {
                Comment comment = context.Comments.SingleOrDefault(c => c.CommentId == CommentID);

                if (comment != null)
                {
                    using (ApplicationDbContext AppContext = new ApplicationDbContext())
                    {
                        User user = AppContext.Users.SingleOrDefault(u => u.user.UserId==comment.UserID).user;
                        ViewBag.Signature = PostController.GenerateSignature(user);
                    }
                    return View(comment);
                }
                else
                {
                    return View("CommentNotFound");
                }
            }
        }
        public ActionResult ViewAllCommentOfPost(int? PostID)
        {
            using (BlogContext context = new BlogContext())
            {
                List<Comment> comments = context.Comments.Where(c => c.Post.PostId == PostID).ToList();

                if (comments.Count >0)
                {
                    return View(comments);
                }
                else
                {
                    return View("NoComments");
                }
            }

        }
        internal Post FindPostwithId(int Postid)
        {
            Post post = null;
            BlogContext context = new BlogContext();

            post = context.Posts.SingleOrDefault(p => p.PostId == Postid);
            return post;
        }
        internal void AddCommentToDatabase(Comment comment)
        {
            BlogContext db = new BlogContext();
            db.Comments.Add(comment);
            db.SaveChanges();
        }

    }
}