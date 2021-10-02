using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<User> _userManager;
        private ApplicationDbContext _db;

        public PostController(UserManager<User> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        private async Task<User> CurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
        public IActionResult Index()
        {
            var posts = _db.Posts.Include(p => p.Author).OrderBy(x => x.CreationTime).ToList();
            return View(posts);
        }
        public IActionResult Comment(int? postId)
        {
            //if (postId != null)
            //{
                var post = _db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == postId);
                post.Comments = _db.Comments.Include(p => p.Commentator).Where(c => c.PostId == postId).ToList();
                return View(post);
            //}
            //else
            //{
            //    return RedirectToAction("Index");
            //}
        }
        
        
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Post p)
        {
            var user = CurrentUser().Result;
            if (p != null)
            {
                p.CreationTime = DateTime.Now;
                p.UserId = user.Id;
                p.Author = user;
                _db.Posts.Add(p);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
    }

