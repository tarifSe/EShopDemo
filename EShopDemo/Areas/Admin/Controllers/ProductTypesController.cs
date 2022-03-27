using EShopDemo.Data;
using EShopDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.ProductTypes.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Add(productTypes);
                await _context.SaveChangesAsync();
                TempData["succssesMessage"] = "Save Successfully Done";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        public IActionResult Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var productType = _context.ProductTypes.Find(id);
            if (productType==null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Update(productTypes);
                await _context.SaveChangesAsync();
                TempData["succssesMessage"] = "Update Successfull";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _context.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _context.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var productTypes = await _context.ProductTypes.FindAsync(id);

             _context.ProductTypes.Remove(productTypes);
            await _context.SaveChangesAsync();
            TempData["succssesMessage"] = "Delete Successfull";
            return RedirectToAction(nameof(Index));
        }
    }
}
