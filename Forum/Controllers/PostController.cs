using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
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

