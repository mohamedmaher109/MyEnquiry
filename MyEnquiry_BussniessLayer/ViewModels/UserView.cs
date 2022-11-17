using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModel
{
    public class UserView
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string confirmpassword { get; set; }
        public string Phone { get; set; }
        public float? Price { get; set; }
        public string CashPhone { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int Usertype { get; set; }
        public int? BankId { get; set; }
        public int? CompanyId { get; set; }
        
        public int CitiyId { get; set; }
        public string CompanyName { get; set; }

        public List<int> CoverageIds { get; set; }
        public string Coveragenames { get; set; }

        public string City { get; set; }
       
        public string Education { get; set; }

        public string Address { get; set; }
        public float? price { get; set; }
        public string AddressDetails { get; set; }

        public string NationalId { get; set; }

        public IFormFile NationalMediaFront { get; set; }
        public IFormFile NationalMediaBack { get; set; }
        public IFormFile CriminalFish { get; set; }
        public IFormFile AcadimicQualification { get; set; }

        public string ReferFullName { get; set; }

        public string ReferPhoneNumber { get; set; }
        public string ReferRelationShip { get; set; }
        public string ReferDescription { get; set; }
        public bool? Blocked{ get; set; }
        public bool IsAccepted { get; set; }

        public string place { get; set; }
        public float? lat { get;  set; }
        public float? lng { get;  set; }
    }
}
