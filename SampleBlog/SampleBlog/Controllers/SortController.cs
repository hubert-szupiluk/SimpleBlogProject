using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SampleBlog.Models;
using SampleBlog.DAL;

namespace SampleBlog.Controllers
{
    public class SortController : Controller
    {
        // GET: Sort
        private delegate List<Post> SortDelegate(List<Post> input);
        public ActionResult SortCriteria()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchusingCriteria(SearchInput input)
        {
            List<Post> Result = new List<Post>();
            using (BlogContext context = new BlogContext())
            {
                if (input!=null)
                {
                    if (input.TitleName != null)
                    {
                        List<Post> Posts1 = context.Posts.Where(p => p.Title.Contains(input.TitleName)).ToList();
                        Result.AddRange(Posts1);
                    }

                    if (input.TagName != null)
                    {
                        List<Tag> tags = context.Tags.Where(t => t.Tag_Name == input.TagName).ToList();
                        List<Post> Posts2 = new List<Post>();
                        foreach (Tag t in tags)
                        {
                         Posts2.AddRange(t.Posts);
                        }
                        Posts2.AddRange(Result);
                        Result = Posts2.Distinct().ToList();

                    }
                }

            }
            SortDelegate sortOption = NotSort;
            switch(input.SortingType)
            {
                case SortType.DateA:
                    sortOption = DateA;
                    break;
                case SortType.DateD:
                    sortOption = DateD;
                    break;
                case SortType.NameA:
                    sortOption = NameA;
                    break;
                case SortType.NameD:
                    sortOption = NameD;
                    break;
                case SortType.RateA:
                    sortOption = RateA;
                    break;
                case SortType.RateD:
                    sortOption = RateD;
                    break;

            }


                return View("~/Views/Post/ViewAllPostsMenu.cshtml", sortOption(Result));
        }
        #region SortMethod
        public List<Post> NotSort (List<Post> input)
        {
            return input;
        }



        public List<Post> DateA(List<Post> input)
        {
           input.Sort(ComparebyDate);

            return input;
        }
        public List<Post> DateD(List<Post> input)
        {
            input.Sort(ComparebyDate);
            input.Reverse();
            return input;
        }
        public List<Post> RateA(List<Post> input)
        {
            input.Sort(ComparebyRate);
            return input;
        }
        public List<Post> RateD(List<Post> input)
        {
            input.Sort(ComparebyRate);
            input.Reverse();
            return input;
        }
        public List<Post> NameA(List<Post> input)
        {
            input.Sort(ComparebyName);
            return input;
        }
        public List<Post> NameD(List<Post> input)
        {
            input.Sort(ComparebyName);
            input.Reverse();
            return input;
        }
        #endregion

        #region Comparsions
        private static int ComparebyRate(Post x, Post y)
        {
            if (x != null && y != null)
            {
                return x.Rate.CompareTo(y.Rate);

            }
            else if (x == null)
            {
                return -1;
            }
            else if (y == null)
            {
                return 1;
            }
            else return 0;
        }

        private static int ComparebyName(Post x, Post y)
        {
                return x.Title.CompareTo(y.Rate);
        }
        private static int ComparebyDate(Post x, Post y)
        {
            return DateTime.Compare(x.DateAdded, y.DateAdded);
        }
        #endregion

    }
}