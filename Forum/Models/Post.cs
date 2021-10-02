using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public string UserId { get; set; }
        public User Author { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
