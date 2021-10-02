using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PostedTime { get; set; }
        public string UserId { get; set; }
        public User Commentator { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
