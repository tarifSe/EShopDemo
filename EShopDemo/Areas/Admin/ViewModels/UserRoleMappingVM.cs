using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopDemo.Areas.Admin.ViewModels
{
    public class UserRoleMappingVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
