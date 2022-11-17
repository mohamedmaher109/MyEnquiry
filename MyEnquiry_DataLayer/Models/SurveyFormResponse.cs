using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
	public class SurveyFormResponse
	{
		public int Id { get; set; }
		public int SurveyFormId { get; set; }
		public SurveyForm SurveyForm { get; set; }
		public int? CompanyId { get; set; }
		public Companies Company { get; set; }
		public Guid GroupId { get; set; }
		public int SurveyFormElementId { get; set; }
		public SurveyFormElement SurveyFormElement { get; set; }
		public string Result { get; set; }
		public bool IsFile { get; set; }
		public string FileUrls { get; set; }
		public DateTime CreatedAt { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
		public int? CaseId { get; set; }

		[ForeignKey("CaseId")]
		public Cases Cases { get; set; }

	}
}
