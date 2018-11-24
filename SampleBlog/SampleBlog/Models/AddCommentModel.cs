using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleBlog.Models
{
    public class AddCommentModel
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public int Postid { get; set; }
    }
}