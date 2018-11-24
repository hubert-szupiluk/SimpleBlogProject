using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SampleBlog.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string Tag_Name { get; set; }
        public virtual List<Post> Posts { get; set; }
       


    }
}