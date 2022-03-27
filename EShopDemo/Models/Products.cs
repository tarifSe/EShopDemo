using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        //[Required]
        public string Image { get; set; }
        [Required]
        [Display(Name="Product Color")]
        public string ProductColor { get; set; }
        [Required]
        [Display(Name ="Available")]
        public bool IsAvailable { get; set; }

        [Display(Name ="Product Type")]
        public int ProductTypesId { get; set; }
        public ProductTypes ProductTypes { get; set; }
        [Display(Name = "Special Tag")]
        public int SpecialTagId { get; set; }
        public TagList SpecialTag { get; set; }
    }
}
