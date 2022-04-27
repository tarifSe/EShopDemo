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
    }
}
