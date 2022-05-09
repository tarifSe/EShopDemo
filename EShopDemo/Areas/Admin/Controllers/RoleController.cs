using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityUser> _roleManager;
        public RoleController(RoleManager<IdentityUser> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
