using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SampleBlog.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "This field is required")]

        public string Content { get; set; }
        public virtual Post Post { get; set; }
        [Required]
        public int? UserID { get; set; }


    }
}