using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
	public interface ISurvey
	{
		List<SurveyForm> GetAll();
		dynamic CopySurvey(ModelStateDictionary modelState, int id);
		Task<dynamic> Add(ModelStateDictionary modelState, SurveyFormVm model);
		SurveyForm Get(ModelStateDictionary modelState, int Id, bool withElements);
		SurveyFormVm GetVM(ModelStateDictionary modelState, int Id);
		Task<dynamic> Delete(ModelStateDictionary modelState, int Id);
		Task<dynamic> Edit(ModelStateDictionary modelState, SurveyFormVm model);
		List<SurveyFormElement> GetElements(int surveyId);
		Task<dynamic> CreateSurveyElement(ModelStateDictionary modelState, SurveyFormElementVm model);
		Task<dynamic> EditSurveyElement(ModelStateDictionary modelState, SurveyFormElementVm model);
		Task<dynamic> DeleteElement(ModelStateDictionary modelState, int id);
		SurveyFormElementVm GetElement(int elementId);
		Task<bool> DeleteLogo(int surveyId, bool isFirst);
		SurveyForm GetByFormIdentifier(Guid formId);
		Task<dynamic> SubmitResponse(ModelStateDictionary modelState, IFormCollection form);
		MemoryStream GenerateResponseWordDocument(int id);
		Task<MemoryStream> GenerateResponseWordDocumentCase(int id);
		List<SurveyFormResponse> GetUserResponse(string userId, Guid formId, int? companyId);
	}
}
