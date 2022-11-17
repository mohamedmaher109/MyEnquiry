using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
	public class SurveyElementsController : Controller
	{
		private ISurvey _survey;

		public SurveyElementsController(ISurvey survey)
		{
			_survey = survey;

		}

		public IActionResult Index(int surveyId)
		{
			var survey = _survey.Get(ModelState, surveyId, true);
			if (survey == null)
			{
				return RedirectToAction(nameof(SurveysController.Index), "Surveys");
			}
			return View(survey);
		}

		[HttpGet]
		public IActionResult Create(int surveyId)
		{
			var survey = _survey.Get(ModelState, surveyId, false);
			if (survey == null)
			{
				return RedirectToAction(nameof(SurveysController.Index), "Surveys");
			}
			ViewBag.Survey = survey;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(SurveyFormElementVm model)
		{
			try
			{
				var result = await _survey.CreateSurveyElement(ModelState, model);
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
		public IActionResult Edit(int elementId)
		{
			var survey = _survey.GetElement(elementId);
			if (survey == null)
			{
				return RedirectToAction(nameof(SurveysController.Index), "Surveys");
			}
			return View(survey);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(SurveyFormElementVm model)
		{
			try
			{
				var result = await _survey.EditSurveyElement(ModelState, model);
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
				var result = await _survey.DeleteElement(ModelState, Id);

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
	}
}
