using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class PermissionPages
    {
        public int Id { get; set; }

        public int PageId { get; set; }

        public int ParentId { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public string AreaName { get; set; }

        public string PageName { get; set; }

        public string Icon { get; set; }

        

        
    }
}
