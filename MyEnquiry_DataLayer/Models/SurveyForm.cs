using System;
using System.Collections.Generic;

namespace MyEnquiry_DataLayer.Models
{
	public class SurveyForm
	{
		public SurveyForm()
		{
			Elements = new HashSet<SurveyFormElement>();
			Responses = new HashSet<SurveyFormResponse>();
		}

        public int Id { get; set; }
		public Guid LinkIdentifier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsAcceptingResponses { get; set; }
		public string Logo1Url { get; set; }
		public string Logo1Text { get; set; }
		public string Logo2Url { get; set; }
		public string Logo2Text { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsDeleted { get; set; }
		public int? CaseTypeId { get; set; }
		public CaseTypes CaseType { get; set; }
		public int? BankId { get; set; }
		public Banks Bank { get; set; }
		public ICollection<SurveyFormElement> Elements { get; set; }
		public ICollection<SurveyFormResponse> Responses { get; set; }
	}
}
