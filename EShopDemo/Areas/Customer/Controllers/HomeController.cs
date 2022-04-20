using EShopDemo.Data;
using EShopDemo.Models;
using EShopDemo.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        //private readonly ILogger<HomeController> _logger;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index(int pg=1)
        {
            var products = _context.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();

            const int pageSize = 6;
            if (pg < 1)
            {
                pg = 1;
            }

            int rowCount = products.Count();
            var pager = new Pager(rowCount, pg, pageSize);

            int rowSkip = (pg - 1) * pageSize;

            var data = products.Skip(rowSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            //return View(products);
            return View(data);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDetails = _context.Products.Include(c => c.ProductTypes).FirstOrDefault(p => p.Id == id);
            if (productDetails == null)
            {
                return NotFound();
            }
            return View(productDetails);
        }

        [HttpPost]
        [ActionName("Details")]
        public IActionResult ProductDetails(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.Include(c => c.ProductTypes).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            var product = products.FirstOrDefault(c => c.Id == id);
            if (product != null)
            {
                products.Remove(product);
                HttpContext.Session.Set("products", products);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }



        /// <summary>
        /// Auto generated methode...
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
