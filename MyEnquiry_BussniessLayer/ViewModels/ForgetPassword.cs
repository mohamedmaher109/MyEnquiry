using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class ForgetPassword
    {
        public string Id { get; set; }
        
        public string Password { get; set; }
        [Compare("Password")]
        public string confirmpassword { get; set; }
    }
}
