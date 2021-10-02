﻿using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    public class AccountController : Controller
    {
            private readonly UserManager<User> _userManager;
            private ApplicationDbContext _db;
            private readonly SignInManager<User> _signInManager;
            IWebHostEnvironment _appEnvironment;


            public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment appEnvironment, ApplicationDbContext db)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _appEnvironment = appEnvironment;
                _db = db;
            }

            [HttpGet]

            public IActionResult Register()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Register(RegisterViewModel model, IFormFile uploadedFile)
            {
                if (ModelState.IsValid)
                {
                    string path = "/Files/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        uploadedFile.CopyTo(fileStream);
                    }
                    FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                    User user = new User
                    {
                        Login = model.Login,
                        Email = model.Email,
                        Photo = file.Path,
                        UserName = model.Login,
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);

            }

            [HttpGet]
            public IActionResult Login(string returnUrl = null)
            {
                return View(new LoginViewModel { ReturnUrl = returnUrl });
            }


            [HttpPost]
            [ValidateAntiForgeryToken]

            public async Task<IActionResult> Login(LoginViewModel model)
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByEmailAsync(model.Email);
                    var result = await _signInManager.PasswordSignInAsync(
                        user,
                        model.Password,
                        model.RememberMe,
                        false
                        );
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
                return View(model);
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> LogOff()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");

            }
        }
    }
