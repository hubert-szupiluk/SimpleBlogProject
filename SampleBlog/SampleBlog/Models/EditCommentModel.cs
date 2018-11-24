using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleBlog.Models
{
    public class EditCommentModel
    {
        public int CommentID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}