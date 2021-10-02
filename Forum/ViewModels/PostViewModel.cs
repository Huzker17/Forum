using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModels
{
    public class PostViewModel
    {
        public IEnumerable<Post> posts { get; set; }
        public Pager Pager { get; set; }

    }
}
