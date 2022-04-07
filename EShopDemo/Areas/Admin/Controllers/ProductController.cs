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
        private IWebHostEnvironment _hostingEnvironment;
        public ProductController(ApplicationDbContext applicationDbContext, IWebHostEnvironment hostingEnvironment)
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
                var searchProduct = _context.Products.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["ProductTypesId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["SpecialTagId"] = new SelectList(_context.TagLists.ToList(), "Id", "TagName");
                    return View(product);
                }

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

        [HttpPost]
        public async Task<IActionResult> Edit(Products product, IFormFile image)
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

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDetails = _context.Products.Include(c => c.ProductTypes).Include(d => d.SpecialTag).FirstOrDefault(p => p.Id == id);
            if (productDetails == null)
            {
                return NotFound();
            }
            return View(productDetails);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(p => p.ProductTypes).Include(t => t.SpecialTag).FirstOrDefault(c => c.Id == id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                var product = await _context.Products.FindAsync(id);

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["successMsg"] = "Product has been Deleted!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
