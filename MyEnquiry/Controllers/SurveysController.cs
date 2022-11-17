using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
	[Authorize]
	public class SurveysController : Controller
	{
		private ISurvey _survey;
		private ICaseTypes _caseTypes;
		private IBanks _banks;

		public SurveysController(ISurvey survey, ICaseTypes caseTypes, IBanks banks)
		{
			_survey = survey;
			_caseTypes = caseTypes;
			_banks = banks;
		}

		public IActionResult Index()
		{
			var caseTypes = _caseTypes.Get(null) as List<CaseTypes>;
			var banks = _banks.Get(null) as List<Banks>;

			ViewBag.CaseTypes = new SelectList(caseTypes, "Id", "NameAr");
			ViewBag.Banks = new SelectList(banks, "Id", "NameAr");

			return View();
		}

		public IActionResult DisplayGrid()
		{
			var surveys = _survey.GetAll();

			return PartialView("_DisplaySurveys", surveys);
        }
   
        public IActionResult CopySurvey(int id)
        {
			try
			{
				var result =  _survey.CopySurvey(ModelState, id);

				if (!ModelState.IsValid)
				{
					return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
				}
				return Json(result);

			}
			catch (Exception ex)
			{
				return CustomBadRequest.CustomExErrorResponse(ex);
			}
		}

        [HttpPost]
		public async Task<IActionResult> Add(SurveyFormVm model)
		{
			try
			{
				var result = await _survey.Add(ModelState, model);

				if (!ModelState.IsValid)
				{
					return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
				}

				return Json(result);
			}
			catch (Exception ex)
			{
				return CustomBadRequest.CustomExErrorResponse(ex);
			}
		}

		public IActionResult Details(int id)
		{
			var survey = _survey.Get(ModelState, id, true);
			if (survey == null)
			{
				return RedirectToAction("Index");
			}
			return View(survey);
		}

		public IActionResult Responses(int id)
		{
			var survey = _survey.Get(ModelState, id, true);
			if (survey == null)
			{
				return RedirectToAction("Index");
			}
			return View(survey);
		}

		public dynamic GetSurvey(int Id)
		{
			try
			{
				var result = _survey.GetVM(ModelState, Id);

				if (!ModelState.IsValid)
				{
					return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
				}

				var caseTypes = _caseTypes.Get(null) as List<CaseTypes>;
				var banks = _banks.Get(null) as List<Banks>;

				ViewBag.CaseTypes = new SelectList(caseTypes, "Id", "NameAr", result.CaseTypeId);
				ViewBag.Banks = new SelectList(banks, "Id", "NameAr", result.BankId);

				return PartialView("_EditSurvey", result);
			}
			catch (Exception ex)
			{
				return CustomBadRequest.CustomExErrorResponse(ex);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(SurveyFormVm model)
		{
			try
			{
				var result = await _survey.Edit(ModelState, model);

				if (!ModelState.IsValid)
				{
					return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
				}

				return Json(result);
			}
			catch (Exception ex)
			{
				return CustomBadRequest.CustomExErrorResponse(ex);
			}
		}

		public async Task<IActionResult> Delete(int Id)
		{
			try
			{
				var result = await _survey.Delete(ModelState, Id);

				if (!ModelState.IsValid)
				{
					return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
				}

				return Json(result);
			}
			catch (Exception ex)
			{
				return CustomBadRequest.CustomExErrorResponse(ex);
			}
		}

		public async Task<IActionResult> DeleteLogo(int surveyId, bool isFirst)
		{
			var isFound = await _survey.DeleteLogo(surveyId, isFirst);
			if (isFound == false)
			{
				return RedirectToAction("Index");
			}

			return RedirectToAction("Details", new { id = surveyId });
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Form(Guid formId, string userId, int? companyId = null) 
		{
			var survey = _survey.GetByFormIdentifier(formId);

			if (survey == null)
			{
				return null;
			}
			if (companyId.HasValue)
			{
				ViewBag.Company = _banks.GetCompany(survey.BankId, companyId.Value, survey.CaseTypeId);
			}
			ViewBag.Survey = survey;

			if (!string.IsNullOrWhiteSpace(userId))
			{
			//	ViewBag.UserResponse = _survey.GetUserResponse(userId, formId, companyId);
			}
			return View();
		}
	
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Respond()
		{
			try
			{
				var result = await _survey.SubmitResponse(ModelState, Request.Form);

				if (!ModelState.IsValid)
				{
					return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
				}

				return Json(result);
			}
			catch (Exception ex)
			{
				return CustomBadRequest.CustomExErrorResponse(ex);
			}
		}

		[HttpGet]
		public IActionResult GenerateResponseWordDocument(int responseGroupId)
		{

			var word = _survey.GenerateResponseWordDocument(responseGroupId);
			if (word == null)
			{
				return RedirectToAction("Index");
			}

			return File(word.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{responseGroupId}.docx");
		}
	}
}
