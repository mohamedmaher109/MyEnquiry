using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Companies
    {
        public Companies()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.SurveyFormResponses = new HashSet<SurveyFormResponse>();
        }
        public int Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string Logo { get; set; }
        [NotMapped]
        public IFormFile Logofile { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public HashSet<ApplicationUser> Users { get; set; }
        public ICollection<SurveyFormResponse> SurveyFormResponses { get; set; }
    }
}
