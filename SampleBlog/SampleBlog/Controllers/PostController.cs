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
    public class PostController : Controller
    {
        // GET: Post
        [Authorize]
        public ActionResult CreatePostForm()
        {
            // var b = RedirectToAction("WriteAuthorize", "Authorize", null);


      

                var controller = DependencyResolver.Current.GetService<AuthorizeController>();
                controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
                var us = controller.WriteAuthorize();


            if (us == true)
            {
                return View();

            } else
            {
                return View("~/Views/Authorize/NotAuthorizeView.cshtml");

            }

        }
        [Authorize]
        public ActionResult EditPostForm(Post p)
        {

            var controller = DependencyResolver.Current.GetService<AuthorizeController>();
            controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
            var us = controller.PostAuthorize(p.PostId);


            if (us == true)
            {
                return View(p);

            }
            else
            {
                return View("~/Views/Authorize/NotAuthorizeView.cshtml");

            }
        }
        [Authorize]
        public ActionResult DeletePost(Post p)
        {

            var controller = DependencyResolver.Current.GetService<AuthorizeController>();
            controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
            var us = controller.PostAuthorize(p.PostId);


            if (us == true)
            {
                DeletePostFromDatabase(p);
                return View("PostDeletedInfo");

            }
            else
            {
                return View("~/Views/Authorize/NotAuthorizeView.cshtml");

            }



        }
        [HttpPost]
        public ActionResult EditPost(CreatePostModel model , int PostId)
        {

            Post post = EditPostToDatabase(model, PostId);
            post.tags = null;
            if (TagStringValidation(model.TagString))
            {
                var tags = SeparateTags(model.TagString);
                PairPostWithNewTags(ref post, ref tags);

            }

            return RedirectToAction("ViewPost",post.PostId);

        }
        private Post EditPostToDatabase(CreatePostModel post, int PostID)
        {
            BlogContext context = new BlogContext();
            var p = context.Posts.SingleOrDefault(ps => ps.PostId == PostID);
            if(p!=null)
            {
                p.Title = post.Title;
                p.Content = post.Content;
                p.tags = null;
                context.SaveChanges();
            }
            return p;

        }
        [HttpPost]
        public ActionResult CreatePost (CreatePostModel model)
        {
           var controller = DependencyResolver.Current.GetService<HomeController>();
           controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
           User user = controller.GetActualUser();
            List<Tag> Tags = null;
            Post post = new Post()
            {
                Content = model.Content,
                Title = model.Title,
                UserID = user.UserId  
            };

            if (TagStringValidation(model.TagString))
            {
                Tags = SeparateTags(model.TagString);
                PairPostWithTags(ref post, ref Tags);

            }

            AddPostToDatabase(post);

            return RedirectToAction ("ViewPost",post );
        }
        private List<Tag> SeparateTags(string tagString)
        {
            List<Tag> TagList = new List<Tag>();
            var TagStringList = SeparateTagStrings(tagString);
            foreach(string t in TagStringList)
            {
                TagList.Add(new Tag { Tag_Name = t });

            }
         /*   BlogContext context = new BlogContext();
            List<string> NewTags = new List<string>();
            foreach(var s in TagStringList)
            {
                var tag = context.Tags.
                    Where(t => t.Tag_Name == s).
                    FirstOrDefault();
                if(tag != null)
                {
                    TagList.Add(tag);
                }else
                {
                    NewTags.Add(s);
                }
            }

            foreach(var t in NewTags)
            {
                Tag tag = new Tag { Tag_Name = t };
                TagList.Add(tag);
            }*/
            
            return TagList;
        }
        /*     [Authorize]
        public ActionResult ViewPost(Post post)
        {
            if (post == null)
            {
                return RedirectToAction("PostNotExist");
            }else

            if (PostValidation(post) == false)
            {
                throw new Exception("Invalidpostobject");
            }
            ViewBag.AuthorSignature = GenerateSignature(post.UserID);
            ViewBag.TagString = GenerateTagString(post.tags);
            return View("ViewPost", post);
        }*/
        [Authorize]
        public ActionResult ViewPost(int? postid)
        {
            var controller = DependencyResolver.Current.GetService<CommentController>();
            controller.ControllerContext = new ControllerContext(this.Request.RequestContext, controller);
            Post post = controller.FindPostwithId((int)postid);
            if (post == null || postid == null)
            {
                return RedirectToAction("PostNotExist");
            }
            else

            if (PostValidation(post) == false)
            {
                throw new Exception("Invalidpostobject");
            }
            ViewBag.AuthorSignature = GenerateSignature(post.UserID);
            return View("ViewPost", post);
        }
        public ActionResult RatePost (int? PostID , int? RateValue)
        {
            using (BlogContext context = new BlogContext())
            {
                Post post = context.Posts.SingleOrDefault(p => p.PostId == PostID);
                post.RatedTimes++;
                post.Rate = (post.Rate + (int)RateValue) / post.RatedTimes;
                context.SaveChanges();
            }
            return RedirectToAction("ViewPost", new { @postid = PostID });
        }
        public ActionResult ViewAllPostsMenu ()
        {
            using (BlogContext Context = new BlogContext())
            {
                List<Post> Posts = Context.Posts.ToList();

                return View(Posts);
            }
        }
        private bool PostValidation(Post post)
        {
            bool result = true;
            if (post == null)
            {
                result = false;

            }
            else
            if (post.UserID == null)
            {
                result = false;
            }
            else
            if (post.Title.Length < 3)
            {
                result = false;
            }
            else
            if (post.Content.Length < 6)
            {
                result = false;
            }

            return result;
        }
        private bool TagStringValidation(string tagstring)
        {
            bool result = true;
            return result;
        }
        Post GetPostWithID(int id)
        {
            Post post = null;
            BlogContext context = new BlogContext();
            post = context.Posts.Where(p => p.PostId == id).FirstOrDefault();
            return post;
        }
        public ActionResult PostNotExist()
        {
            return View();
        }
        private List<string> SeparateTagStrings(string tagstring)
        {
            if (tagstring == null || tagstring == "")
            {
                return new List<string>();
            }

            List<string> taglist = new List<string>();

            tagstring = tagstring.Replace(" ", string.Empty);
            char[] seplist = { ',', '.', };
            string[] SeparateStrings = tagstring.Split(seplist);

            foreach (string s in SeparateStrings)
            {
                taglist.Add(s);
            }

            return taglist;
        }
        private void PairPostWithTag (ref Post post , ref Tag tag) 
        {
            if(post.tags == null)
            {
                post.tags = new List<Tag>();
            }
            post.tags.Add(tag);
            tag.Posts.Add( post);

        }
        internal ApplicationUser FindAppUserwithId(string Id)
        {
            ApplicationUser user = null;
            if (Id == null)
            {
                return null;
            }
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(Id);


            return user;
        }
        internal User FindUserwithId(int? id)
        {
            User user = null;
            ApplicationDbContext Context = new ApplicationDbContext();

            var x = Context.Users
                .Where(u => u.user.UserId == id)
                .FirstOrDefault();

            user = x.user;
            return user;
        }
        private void PairPostWithTags(ref Post post, ref List<Tag> tags)
        {
           if (post.tags == null)
            {
                post.tags = new List<Tag>();
            }
            if (post.tags == null)
            {
                post.tags = new List<Tag>();
            }
            foreach (Tag t in tags)
            {
                post.tags.Add(t);
                if (t.Posts == null)
                {
                    t.Posts = new List<Post>();
                }
                t.Posts.Add( post);

            }

        }
        private void PairPostWithNewTags(ref Post post, ref List<Tag> tags)
        {

            post.tags = new List<Tag>();
            post.tags = null;


            foreach (Tag t in tags)
            {
                post.tags.Add(t);
                if (t.Posts == null)
                {
                    t.Posts = new List<Post>();
                }
                t.Posts.Add(post) ;

            }

        }
        private void PairPostWithTags(ref Post post, ref Tag[] tags)
        {
            if (post.tags == null)
            {
                post.tags = new List<Tag>();
            }
            foreach (Tag t in tags)
            {
                post.tags.Add(t);
                t.Posts.Add(post);

            }


        }
        private void AddTagToDatabase(Tag tag)
        {
            BlogContext db = new BlogContext();
    

            db.Tags.Add(tag);
            db.SaveChanges();
        }
        private void AddPostToDatabase(Post post)
        {
            BlogContext db = new BlogContext();
            post.DateAdded = DateTime.Now;
            post.Rate = 0;
            post.RatedTimes = 0;
            db.Posts.Add(post);
            db.SaveChanges();
        }
        public void UnPairTagsFromPost(Post post)
        {
            BlogContext cont = new BlogContext();
           var ps = cont.Posts.SingleOrDefault(p => p.PostId == post.PostId);
            if (ps != null)
            {
                ps.tags = null;
                cont.SaveChanges();
                cont.SaveChanges();
            }

        }
        private void DeletePostFromDatabase(Post p)
        {
            if (p != null)
            {
                UnPairTagsFromPost(p);
                BlogContext context = new BlogContext();
                context.Posts.Attach(p);
                context.Posts.Remove(p);
                context.SaveChanges();
            }
        }
        public static string GenerateSignature (User user)
        {
            string Signature = null;
            if (user == null)
            {
                Signature = "";
            } else
            {
                Signature = $"Author: {user.Name}, {user.LastName}";
            }

            return Signature;
        }
        public static string GenerateSignature(int? userid)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                User us = context.Users.SingleOrDefault(u => u.user.UserId == userid).user;
                return GenerateSignature(us);
            }


        }
        public static string GenerateTagString (Tag[] tags)
        {
            string tagstring = "" ;

            foreach (var tag in tags)
            {
                tagstring += $"{tag}, ";
            }
            tagstring.Remove(tagstring.Length - 2);

            return tagstring;
        }
        public static string GenerateTagString(List<Tag> tags)
        {
            string tagstring = "";

            if (tags.Count == 0)
                return string.Empty;

            foreach (var tag in tags)
            {
                tagstring += $"{tag.Tag_Name}, ";
            }
            tagstring.Remove(tagstring.Length - 2);

            return tagstring;
        }
        internal List<Comment> GetAllCommentOfPost(int? PostID)
        {
            using (BlogContext context = new BlogContext())
            {
                List<Comment> comments = context.Comments.Where(c => c.Post.PostId == PostID).ToList();

                if (comments.Count > 0)
                {
                    return comments;
                }
                else
                {
                    return null;
                }
            }

        }


    }
}