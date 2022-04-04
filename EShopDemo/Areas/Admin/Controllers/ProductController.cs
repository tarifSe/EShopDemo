using EShopDemo.Data;
using EShopDemo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        public ProductController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment)
        {
            _context = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(c => c.ProductTypes).Include(d => d.SpecialTag).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewData["ProductTypesId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(_context.TagLists.ToList(), "Id", "TagName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_hostingEnvironment.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;
                }
                else
                {
                    product.Image = "Images/DefaultImage.PNG";
                }
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["successMsg"] = "Product has been save successfull";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int? id)
        {
            ViewData["ProductTypesId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(_context.TagLists.ToList(), "Id", "TagName");

            if (id == null)
            {
                NotFound();
            }
            var product = _context.Products.Include(c => c.ProductTypes).Include(t => t.SpecialTag)
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                NotFound();
            }
            return View(product);
        }
    }
}
