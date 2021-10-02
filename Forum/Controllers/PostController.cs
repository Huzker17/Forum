using Forum.Models;
using Forum.ViewModels;
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
        public IActionResult Index(int page = 1)
        {
            var posts = _db.Posts.Include(p => p.Author).Include(c => c.Comments).OrderByDescending(x => x.CreationTime).ToList();
            return View(posts);
        }
        public IActionResult Comment(int? postId)
        {
                var post = _db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == postId);
                post.Comments = _db.Comments.Include(p => p.Commentator).Where(c => c.PostId == postId).OrderBy(x => x.PostedTime).ToList();
                return View(post);
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
        public JsonResult Pagination()
        {
            var posts = _db.Posts.ToList();
            return Json(new { posts});
        }
    }
    }

