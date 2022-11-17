using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
	public class SurveyFormVm
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsAcceptingResponses { get; set; }
		public IFormFile Logo1 { get; set; }
		public string Logo1Text { get; set; }
		public IFormFile Logo2 { get; set; }
		public string Logo2Text { get; set; }
		public int? CaseTypeId { get; set; }
		public int? BankId { get; set; }
	}
}
