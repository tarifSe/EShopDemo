using EShopDemo.Areas.Admin.ViewModels;
using EShopDemo.Data;
using EShopDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;
        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _context = applicationDbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            IdentityRole iRole = new IdentityRole()
            {
                Name = name
            };
            var isExist = await _roleManager.RoleExistsAsync(name);
            if (isExist)
            {
                ViewBag.msg = "This role is already exist!";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.CreateAsync(iRole);
            if (result.Succeeded)
            {
                TempData["succssesMessage"] = "Role has been created";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(name);
            if (isExist)
            {
                ViewBag.msg = "This role is already exist!";
                ViewBag.id = role.Id;
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["succssesMessage"] = "Role has been Updated";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["DeleteMessage"] = "Role has been Deleted";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Assign()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers.Where(c => c.LockoutEnd < DateTime.Now || c.LockoutEnd == null).ToList(), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserVM roleUser)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == roleUser.UserId);

            if (await _userManager.IsInRoleAsync(user, roleUser.RoleId))
            {
                ViewBag.msg = "This role '" + roleUser.RoleId + "' is already Assigned!";
                ViewData["UserId"] = new SelectList(_context.ApplicationUsers.Where(c => c.LockoutEnd < DateTime.Now || c.LockoutEnd == null).ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return View();
            }

            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);
            if (role.Succeeded)
            {
                TempData["succssesMessage"] = "Role has been Assined";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
