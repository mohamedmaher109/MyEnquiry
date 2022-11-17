using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class CaseTypes
    {
		public CaseTypes()
		{
            SurveyForms = new HashSet<SurveyForm>();
        }

        public int Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Active { get; set; }

        public string Word { get; set; }

        [NotMapped]
        public IFormFile Wordfile { get; set; }
		public ICollection<SurveyForm> SurveyForms { get; set; }
	}
}
