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
    public class TagListController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TagListController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.TagLists.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagList tagList)
        {
            if (ModelState.IsValid)
            {
                _context.TagLists.Add(tagList);
                await _context.SaveChangesAsync();
                TempData["message"] = "Save Successfully Done!";
                return RedirectToAction(nameof(Index));
            }
            return View(tagList);
        }

        public IActionResult Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var tagName = _context.TagLists.Find(id);
            return View(tagName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagList tag)
        {
            if (ModelState.IsValid)
            {
                _context.TagLists.Update(tag);
                await _context.SaveChangesAsync();
                TempData["message"] = "Update Successful";
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tagName = _context.TagLists.Find(id);
            return View(tagName);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tagName = _context.TagLists.Find(id);
            return View(tagName);
        }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tagName = await _context.TagLists.FindAsync(id);

            _context.TagLists.Remove(tagName);
            await _context.SaveChangesAsync();
            TempData["message"] = "Delete Successful";
            return RedirectToAction(nameof(Index));
        }
    }
}
