using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Display(Name = "Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [Display(Name = "Product")]
        public int ProductsId { get; set; }
        public Products Products { get; set; }

    }
}
