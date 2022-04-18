using EShopDemo.Data;
using EShopDemo.Models;
using EShopDemo.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach (var item in products)
                {
                    OrderDetails orderDetails = new OrderDetails() 
                    { 
                        ProductsId = item.Id 
                    };
                    //orderDetails.ProductsId = item.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }
            anOrder.OrderNo = GetOrderNo();
            _context.Orders.Add(anOrder);
            await _context.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Products>());
            return View();
        }

        private string GetOrderNo()
        {
            int count = _context.Orders.ToList().Count() + 1;
            return count.ToString("000");
        }
    }
}
