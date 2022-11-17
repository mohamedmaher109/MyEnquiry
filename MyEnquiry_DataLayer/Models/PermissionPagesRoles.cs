using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class PermissionPagesRoles
    {

        public int Id { get; set; }

        public int PageId { get; set; }

        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; }

        [ForeignKey("PageId")]
        public virtual PermissionPages Page { get; set; }
    }
}
