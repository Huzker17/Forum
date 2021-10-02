using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class User: IdentityUser
    {
        public string Email { get; set; }
        public string Photo { get; set; }
        public string Login { get; internal set; }
        public ICollection<Post> Posts { get; set; }
    }
}
