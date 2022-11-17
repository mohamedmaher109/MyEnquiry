using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels.Api
{
    public class ResetPassword
    {
        [Required]
        public string phone { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirmPassword { get; set; }

    }
}
