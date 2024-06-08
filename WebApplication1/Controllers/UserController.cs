using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var dbUser = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (dbUser != null)
            {
                return RedirectToAction("Profile", dbUser);
            }
            ModelState.AddModelError("", "Неправильна спроба входу.");
            return View(user);
        }

        public IActionResult Profile(User user)
        {
            return View(user);
        }
    }
}
