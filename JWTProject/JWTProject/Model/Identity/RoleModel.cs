using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTProject.Model
{
    public class RoleModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string RoleName { get; set; }
    }
}
