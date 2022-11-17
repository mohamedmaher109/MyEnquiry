using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels.Api
{
    public class RegisterRM
    {
        [Required]
        //public byte[] fullName { get; set; }
        public string fullName { get; set; }
        [EmailAddress]
        [Required]
        public string email { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public string cashNumber { get; set; }
   /*     [Required]
        public float price { get; set; }*/
        [Required]
        public int cityid { get; set; }
        [Required]
        // public byte[] education { get; set; }
        public string education { get; set; }

        [Required]
        //public byte[] address { get; set; }
        public string address { get; set; }

        [Required]
        //public byte[] addressDetails { get; set; }
        public string addressDetails { get; set; }

        [Required]
        public string nationalId { get; set; }
        public IFormFile profileImage { get; set; }
        public IFormFile nationalIdFront { get; set; }
        public IFormFile nationalIdBack { get; set; }
        public IFormFile criminalFish { get; set; }
        public IFormFile academicQualification { get; set; }
        public List<int> coverageArea { get; set; }
        [Required]
        public int companyId { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        //public byte[] someOneFullName { get; set; }
        public string someOneFullName { get; set; }

        public string someOnePhoneNumber { get; set; }
        //public byte[] someOneRelationShip { get; set; }
        public string someOneRelationShip { get; set; }

        public string someOneDescription { get; set; }
    }
}
