using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<User> _userManager;
        private ApplicationDbContext _db;

        public CommentController(UserManager<User> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        private async Task<User> CurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
        [HttpPost]
        public async Task<JsonResult> Add(int postId, string commentText)
        {
            var post = _db.Posts.FirstOrDefault(p => p.Id == postId);
            Comment comment = new Comment
            {
                PostId = postId,
                PostedTime = DateTime.Now,
                Commentator = CurrentUser().Result,
                Post = post,
                Text = commentText
            };

            var result = _db.Comments.AddAsync(comment);
            if (result.IsCompleted)
            {
                await _db.SaveChangesAsync();
            }

            return Json(new { comment });
        }
    }
}
