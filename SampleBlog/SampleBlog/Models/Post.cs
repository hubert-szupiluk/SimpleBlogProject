using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SampleBlog.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [StringLength(200, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [StringLength(1000, MinimumLength = 6)]
        [Required]
        public string Content { get; set; }
        [Required]
        public int? UserID { get; set; }
        public virtual List<Tag> tags {get ; set;}
        public virtual List<Comment> comments { get; set; }
        public float Rate { get; set; }
        public int RatedTimes { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }
    }

}