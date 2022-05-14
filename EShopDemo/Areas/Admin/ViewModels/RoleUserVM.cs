using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Areas.Admin.ViewModels
{
    public class RoleUserVM
    {
        [Required]
        [Display(Name ="User")]
        public int UserId { get; set; }
        [Required]
        [Display(Name ="Role")]
        public int RoleId { get; set; }
    }
}
