using System.Collections.Generic;

namespace MyEnquiry_DataLayer.Models
{
	public class SurveyFormElement
	{
		public SurveyFormElement()
		{
			Items = new HashSet<SurveyFormElementItem>();
			Responses = new HashSet<SurveyFormResponse>();
		}

		public int Id { get; set; }
		public int SurveyFormId { get; set; }
		public SurveyForm SurveyForm { get; set; }
		public string Label { get; set; }
		public string Notes { get; set; }
		public bool IsCheckbox { get; set; }
		public bool IsTextbox { get; set; }
		public bool IsTextarea { get; set; }
		public bool IsRadioButton { get; set; }
		public bool IsSelect { get; set; }
		public bool IsDate { get; set; }
		public bool IsFile { get; set; }
		public bool IsCamira { get; set; } = false;
		public bool IsRequired { get; set; }
		public string CheckBoxText { get; set; }
		public bool IsTextboxEmail { get; set; }
		public bool IsTextboxPassword { get; set; }
		public bool IsTextboxNumber { get; set; }
		public int? TextboxMinLength { get; set; }
		public int? TextboxMaxLength { get; set; }
		public int? TextareaMinLength { get; set; }
		public int? TextareaMaxLength { get; set; }
		public string FileAcceptedFileExtensions { get; set; }
		public int? FileNumberOfMinimumFilesAllowed { get; set; }
		public int? FileNumberOfMaximumFilesAllowed { get; set; }
		public string SelectBoxOptionText { get; set; }
		public bool IsDeleted { get; set; }
		public int Order { get; set; }
		public ICollection<SurveyFormElementItem> Items { get; set; }
		public ICollection<SurveyFormResponse> Responses { get; set; }
	}
}
