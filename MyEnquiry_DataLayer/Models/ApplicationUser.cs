using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models {
    public partial class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.UserMedia = new HashSet<UserMedia>();
            this.CoverageArea = new HashSet<CoverageArea>();
            this.SurveyFormResponses = new HashSet<SurveyFormResponse>();
        }

        public DateTime Createdon { get; set; } = DateTime.Now;
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? FromDash { get; set; }
        public bool Reviewr { get; set; }=false;
        public string UserImg { get; set; }
        public string UserIdApp { get; set; }
        public string DeviceType { get; set; }
        public string DeviceToken { get; set; }
        public string CashNumber { get; set; }
        public int? VerificationCode { get; set; }


        public string SecurityCode { get; set; }

        public int UserType { get; set; }//1 for bank 2for company 3 for SystemManger 4 for Representative
        public int? BankId { get; set; }

        public int? CompanyId { get; set; }

        public float? lat { get; set; }
        public float? lng { get; set; }


        //public string City { get; set; }
        public float? price { get; set; }

        public string Education { get; set; }

        public string Address { get; set; }
        public string AddressDetails { get; set; }

        public string NationalId { get; set; }

        public int? ReferId { get; set; }
        public bool Deleted { get; set; } = false;
        public bool Blocked { get; set; } = false;
        public bool IsAccepted { get; set; } = false;


        [ForeignKey("BankId")]
        public Banks Bank { get; set; }
        [ForeignKey("CompanyId")]
        public Companies Company { get; set; }
        [ForeignKey("ReferId")]
        public UserRefer UserRefer { get; set; }

        public HashSet<UserMedia> UserMedia { get; set; }
        public HashSet<CoverageArea> CoverageArea { get; set; }
        //public int? CitiesId { get; set; }
        public int? cityId { get; set; }
        [ForeignKey("cityId")]
        public Cities city { get; set; }

		public ICollection<SurveyFormResponse> SurveyFormResponses { get; set; }
		//[NotMapped]
		//public int[] CoverageIds { get; set; }
		//[NotMapped]
		//public IFormFile NationalMediaFront { get; set; }
		//[NotMapped]
		//public IFormFile NationalMediaBack { get; set; }
		//[NotMapped]
		//public IFormFile CriminalFish { get; set; }
		//[NotMapped]
		//public IFormFile AcadimicQualification { get; set; }
	}
}
