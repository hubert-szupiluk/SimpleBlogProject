using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SampleBlog.Models
{
    public class CreatePostModel
    {
        [StringLength(200, ErrorMessage = "Post length must have length from 3 to 200", MinimumLength = 3)]
        [Required(ErrorMessage = "This field is required")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Post length must have length from 6 to 1000", MinimumLength = 6)]
        [Required(ErrorMessage = "This field is required")]
        public string Content { get; set; }
        public string TagString { get; set; }
    }
}