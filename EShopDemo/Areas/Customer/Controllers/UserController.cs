using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShopDemo.Models;
using Microsoft.AspNetCore.Identity;
using EShopDemo.Data;

namespace EShopDemo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _context = applicationDbContext;
        }

        public IActionResult Index()
        {
            var userList = _context.ApplicationUsers.ToList();
            return View(userList);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser appUser)
        {
            if (ModelState.IsValid)
            {
                //appUser.Email = appUser.UserName;
                var result = await _userManager.CreateAsync(appUser, appUser.PasswordHash);
                if (result.Succeeded)
                {
                    TempData["doneMessage"] = "User has been created";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        public IActionResult Edit(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                return View(user);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo != null)
            {
                userInfo.FirstName = user.FirstName;
                userInfo.LastName = user.LastName;

                var result = await _userManager.UpdateAsync(userInfo);
                if (result.Succeeded)
                {
                    TempData["doneMessage"] = "User has been Updated";
                    return RedirectToAction(nameof(Index));
                }
            }
            else return NotFound();

            return View(userInfo);
        }

        public IActionResult Details(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                return View(user);
            }
            else return NotFound();
        }

        public IActionResult Lockout(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                return View(user);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Lockout(ApplicationUser applicationUser)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == applicationUser.Id);
            if (user != null)
            {
                user.LockoutEnd = DateTime.Now.AddYears(5);
                //int isExecuted = await _context.SaveChangesAsync();
                if (await _context.SaveChangesAsync() > 0)
                {
                    TempData["doneMessage"] = "User has been LOCKED OUT!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else return NotFound();
            return View(user);
        }
    }
}
