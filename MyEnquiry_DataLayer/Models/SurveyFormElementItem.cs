namespace MyEnquiry_DataLayer.Models
{
	public class SurveyFormElementItem
	{
		public int Id { get; set; }
		public int SurveyFormElementId { get; set; }
		public SurveyFormElement SurveyFormElement { get; set; }
		public string Text { get; set; }
		public bool IsDeleted { get; set; }
	}
}
