using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleBlog.Models
{
    public enum SortType { NameA = 0, NameD = 1 , DateA = 2 , DateD = 3 , RateA = 4 , RateD = 5}
    public class SearchInput
    {
        public string TagName { get; set; }
        public string TitleName { get; set; }
        public SortType SortingType { get; set; }




    }
}