using SampleBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SampleBlog.DAL
{
    public class BlogContext : DbContext
    {
        public BlogContext(): base("DBConnectionString3")
        {

        }

       

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }





    }
}