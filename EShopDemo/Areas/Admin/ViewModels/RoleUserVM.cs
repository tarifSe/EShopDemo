﻿using System;
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
        public string UserId { get; set; }

        [Required]
        [Display(Name ="Role")]
        public string RoleId { get; set; }
    }
}
