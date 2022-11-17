using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using MyEnquiry_BussniessLayer.AppsettingsModels;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace MyEnquiry_BussniessLayer.Bussniess
{
	public class SurveysBusiness : ISurvey
	{
		private MyAppContext _context;
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IWebHostEnvironment _environment;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly SurveysConfig _surveysConfig;

		public SurveysBusiness(MyAppContext context, IWebHostEnvironment environment, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IOptions<SurveysConfig> surveysConfig)
		{
			_context = context;
			_environment = environment;
			_hostingEnvironment = hostingEnvironment;
			_httpContextAccessor = httpContextAccessor;
			_surveysConfig = surveysConfig.Value;
		}
		public dynamic CopySurvey(ModelStateDictionary modelState,int id)
		{
			try
			{
				var model = _context.SurveyForms.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
				if (model != null)
				{
					var surveyObject = new SurveyForm()
					{
						CreatedAt = DateTime.Now,
						Description = model.Description,
						IsAcceptingResponses = model.IsAcceptingResponses,
						Logo1Text = model.Logo1Text,
						Logo2Text = model.Logo2Text,
						IsDeleted = false,
				        Logo1Url = model.Logo1Url != null ? model.Logo1Url : null,
				    	Logo2Url = model.Logo2Url != null ? model.Logo1Url : null,
					    Name = model.Name,
						LinkIdentifier = Guid.NewGuid(),
						CaseTypeId= model.CaseTypeId

					};

					_context.SurveyForms.Add(surveyObject);
					_context.SaveChanges();
					var surveyElementNmQ = _context.SurveyFormElements.Where(a => !a.IsDeleted && a.SurveyFormId == model.Id).ToList();
                    if (surveyElementNmQ == null)
                    {
						modelState.AddModelError("تداخل بيانات", "عفواً ، حدث خطأ ما");
						return null;
					}
					foreach (var surveyElementNm in surveyElementNmQ)
					{


						var surveyElement = new SurveyFormElement()
						{
							CheckBoxText = surveyElementNm.CheckBoxText,
							IsCheckbox = surveyElementNm.IsCheckbox,
							IsDate = surveyElementNm.IsDate,
							IsDeleted = false,
							IsFile = surveyElementNm.IsFile,
							IsCamira = surveyElementNm.IsCamira,
							FileAcceptedFileExtensions = surveyElementNm.IsFile ? surveyElementNm.FileAcceptedFileExtensions : null,
							FileNumberOfMaximumFilesAllowed = surveyElementNm.IsFile && surveyElementNm.FileNumberOfMaximumFilesAllowed.HasValue && surveyElementNm.FileNumberOfMaximumFilesAllowed > 0 ? surveyElementNm.FileNumberOfMaximumFilesAllowed : null,
							FileNumberOfMinimumFilesAllowed = surveyElementNm.IsFile && surveyElementNm.FileNumberOfMinimumFilesAllowed.HasValue && surveyElementNm.FileNumberOfMinimumFilesAllowed > 0 ? surveyElementNm.FileNumberOfMinimumFilesAllowed : null,
							IsRadioButton = surveyElementNm.IsRadioButton,
							IsRequired = surveyElementNm.IsRequired,
							IsSelect = surveyElementNm.IsSelect,
							SelectBoxOptionText = surveyElementNm.IsSelect ? surveyElementNm.SelectBoxOptionText : null,
							IsTextarea = surveyElementNm.IsTextarea,
							TextareaMaxLength = surveyElementNm.IsTextarea ? surveyElementNm.TextareaMaxLength : null,
							TextareaMinLength = surveyElementNm.IsTextarea ? surveyElementNm.TextareaMinLength : null,
							IsTextbox = surveyElementNm.IsTextbox,
							TextboxMaxLength = surveyElementNm.IsTextbox ? surveyElementNm.TextboxMaxLength : null,
							TextboxMinLength = surveyElementNm.IsTextbox ? surveyElementNm.TextboxMinLength : null,
							IsTextboxEmail = surveyElementNm.IsTextbox ? surveyElementNm.IsTextboxEmail : false,
							IsTextboxPassword = surveyElementNm.IsTextbox ? surveyElementNm.IsTextboxPassword : false,
							IsTextboxNumber = surveyElementNm.IsTextbox ? surveyElementNm.IsTextboxNumber : false,
							Label = surveyElementNm.Label?.Trim(),
							Notes = surveyElementNm.Notes?.Trim(),
							Order = surveyElementNm.Order < 0 ? 0 : surveyElementNm.Order,
							SurveyFormId = surveyObject.Id,
						};

						/*	if (surveyElementNm.IsFile == true)
							{
								if (surveyElementNm.IsCamira == true)
								{
									surveyElement.FileAcceptedFileExtensions = "";
								}
							};*/

						_context.SurveyFormElements.Add(surveyElement);
						_context.SaveChanges();
						var CheckItemId = _context.SurveyFormElementItems.Where(a => !a.IsDeleted && a.SurveyFormElementId == surveyElementNm.Id).ToList();
						if (CheckItemId!=null|| CheckItemId.Count()!=0)
						{
							foreach (var item in CheckItemId)
							{
								var NewIwm = new SurveyFormElementItem();
								NewIwm.SurveyFormElementId = surveyElement.Id;
								NewIwm.Text = item.Text;
								NewIwm.IsDeleted = item.IsDeleted;
								_context.SurveyFormElementItems.Add(NewIwm);
								_context.SaveChanges();

							}
						}


					}
					return new
					{
						msg = "تم نسخ الإستعلام بنجاح"
					};
				}

              
				
				else
				{
					modelState.AddModelError("تداخل بيانات", "عفواً ، حدث خطأ ما");
					return null;
				}
			}
			catch (Exception ex)
			{
				modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
				return null;
			}
		}
	   public async Task<dynamic> Add(ModelStateDictionary modelState, SurveyFormVm model)
		{
			try
			{
				var surveyForm = _context.SurveyForms.FirstOrDefault(b => b.BankId == model.BankId && b.CaseTypeId == model.CaseTypeId && !b.IsDeleted);
				if (surveyForm == null)
				{
					var surveyObject = new SurveyForm()
					{
						CreatedAt = DateTime.Now,
						Description = model.Description,
						IsAcceptingResponses = model.IsAcceptingResponses,
						Logo1Text = model.Logo1Text,
						IsDeleted = false,
						Name = model.Name,
						Logo2Text = model.Logo2Text,
						Logo1Url = model.Logo1 != null ? Files.SaveImage(model.Logo1, _environment) : null,
						Logo2Url = model.Logo2 != null ? Files.SaveImage(model.Logo2, _environment) : null,
						LinkIdentifier = Guid.NewGuid(),
						BankId = model.BankId,
						CaseTypeId = model.CaseTypeId
					};

					_context.SurveyForms.Add(surveyObject);
					await _context.SaveChangesAsync();

					return new
					{
						msg = "تم اضافة الإستعلام بنجاح"
					};

				}
				else
				{
					modelState.AddModelError("تداخل بيانات", "عفواً ، تم اضافة إستعلام لهذا البنك بنفس الحالة من قبل");
					return null;
				}
			}
			catch (Exception ex)
			{
				modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
				return null;
			}
		}

		public List<SurveyForm> GetAll()
		{
			return _context.SurveyForms.Include(s => s.CaseType).Include(s => s.Bank).Where(f => !f.IsDeleted).OrderByDescending(f => f.CreatedAt).ToList();
		}

		public SurveyForm Get(ModelStateDictionary modelState, int Id, bool withElements)
		{
			try
			{
				var surveyQuery = _context.SurveyForms.AsQueryable();
				if (withElements)
				{
					surveyQuery = surveyQuery.Include(f => f.Bank).Include(f => f.CaseType).Include(f => f.Responses).ThenInclude(f => f.User).Include(f => f.Responses).ThenInclude(f => f.Company).Include(f => f.Elements).ThenInclude(f => f.Items);
				}

				var survey = surveyQuery.FirstOrDefault(r => r.Id == Id && !r.IsDeleted);
				if (survey == null)
				{
					modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذا الإستعلام");
					return null;
				}

				return survey;
			}
			catch (Exception ex)
			{
				modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
				return null;
			};
		}

		public SurveyFormVm GetVM(ModelStateDictionary modelState, int Id)
		{
			try
			{
				var survey = _context.SurveyForms.FirstOrDefault(r => r.Id == Id && !r.IsDeleted);
				if (survey == null)
				{
					modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذا الإستعلام");
					return null;
				}

				return new SurveyFormVm()
				{
					Description = survey.Description,
					Id = survey.Id,
					IsAcceptingResponses = survey.IsAcceptingResponses,
					Logo1Text = survey.Logo1Text,
					Logo2Text = survey.Logo2Text,
					Name = survey.Name,
					BankId = survey.BankId,
					CaseTypeId = survey.CaseTypeId,
				};
			}
			catch (Exception ex)
			{
				modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
				return null;
			};
		}

		public async Task<dynamic> Edit(ModelStateDictionary modelState, SurveyFormVm model)
		{
			try
			{
				var survey = _context.SurveyForms.FirstOrDefault(r => r.Id == model.Id);
				if (survey == null)
				{
					modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذا الإستعلام");
					return null;
				}

				var otherForms = _context.SurveyForms.FirstOrDefault(b => b.BankId == model.BankId && b.CaseTypeId == model.CaseTypeId && b.Id != model.Id && !b.IsDeleted);
				if (otherForms != null)
				{
					modelState.AddModelError("تداخل بيانات", "عفواً ، تم اضافة إستعلام لهذا البنك بنفس الحالة من قبل");
					return null;
				}

				survey.Name = model.Name?.Trim();
				survey.Description = model.Description?.Trim();
				survey.Logo1Text = model.Logo1Text?.Trim();
				survey.Logo2Text = model.Logo2Text?.Trim();
				survey.Logo1Url = model.Logo1 != null ? Files.SaveImage(model.Logo1, _environment) : survey.Logo1Url;
				survey.Logo2Url = model.Logo2 != null ? Files.SaveImage(model.Logo2, _environment) : survey.Logo2Url;
				survey.IsAcceptingResponses = model.IsAcceptingResponses;
				survey.CaseTypeId = model.CaseTypeId;
				survey.BankId = model.BankId;

				await _context.SaveChangesAsync();

				return new
				{
					msg = "تم تعديل الإستعلام "
				};
			}
			catch (Exception ex)
			{
				modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
				return null;
			}
		}

		public async Task<dynamic> Delete(ModelStateDictionary modelState, int Id)
		{
			try
			{
				var survey = _context.SurveyForms.FirstOrDefault(r => r.Id == Id);
				if (survey == null)
				{
					modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذا الإستعلام");
					return null;
				}
				survey.IsDeleted = true;
				await _context.SaveChangesAsync();

				return new
				{
					msg = "تم حذف الإستعلام "
				};
			}
			catch (Exception ex)
			{
				modelState.AddModelError("تحذير", "يجب حذف كل البيانات المتعلقة بالإستعلام اولا");
				return null;
			}
		}

		public async Task<dynamic> DeleteElement(ModelStateDictionary modelState, int Id)
		{
			try
			{
				var survey = _context.SurveyFormElements.FirstOrDefault(r => r.Id == Id);
				if (survey == null)
				{
					modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذا العنصر");
					return null;
				}
				survey.IsDeleted = true;
				await _context.SaveChangesAsync();

				return new
				{
					msg = "تم حذف العنصر "
				};
			}
			catch (Exception ex)
			{
				modelState.AddModelError("تحذير", "يجب حذف كل البيانات المتعلقة بالعنصر اولا");
				return null;
			}
		}

		public List<SurveyFormElement> GetElements(int surveyId)
		{
			return _context.SurveyFormElements.Where(f => !f.SurveyForm.IsDeleted && !f.IsDeleted && f.SurveyFormId == surveyId).ToList();
		}

		public async Task<dynamic> CreateSurveyElement(ModelStateDictionary modelState, SurveyFormElementVm model)
		{
			if (string.IsNullOrWhiteSpace(model.Label))
			{
				modelState.AddModelError("خطأ فى البيانات", "برجاء ادخال اسم للعنصر");
				return null;
			}

			if (model.IsRadioButton && string.IsNullOrWhiteSpace(model.RadioButtonOptions))
			{
				modelState.AddModelError("خطأ فى البيانات", "برجاء ادخال اختيارات العنصر Radio Button");
				return null;
			}

			if (model.IsSelect && string.IsNullOrWhiteSpace(model.SelectOptions))
			{
				modelState.AddModelError("خطأ فى البيانات", "برجاء ادخال اختيارات العنصر اختيار من متعدد");
				return null;
			}

			if (model.IsCheckbox && string.IsNullOrWhiteSpace(model.CheckBoxText))
			{
				modelState.AddModelError("خطأ فى البيانات", "برجاء ادخال االنص الخاص بخانه الإختيار");
				return null;
			}

			var survey = _context.SurveyForms.FirstOrDefault(s => s.Id == model.SurveyFormId && !s.IsDeleted);
			if (survey == null)
			{
				modelState.AddModelError("خطأ فى البيانات", "الاستعلام المطلوب غير متوفر");
				return null;
			}

			if (model.IsTextarea && model.TextareaMaxLength.HasValue && model.TextareaMinLength.HasValue && model.TextareaMaxLength < model.TextareaMinLength)
			{
				modelState.AddModelError("خطأ فى البيانات", "الحد الأقصى من الحروف أقل من الحد الأدنى");
				return null;
			}

			if (model.IsTextbox && model.TextboxMaxLength.HasValue && model.TextboxMinLength.HasValue && model.TextboxMaxLength < model.TextboxMinLength)
			{
				modelState.AddModelError("خطأ فى البيانات", "الحد الأقصى من الحروف أقل من الحد الأدنى");
				return null;
			}

			var surveyElement = new SurveyFormElement()
			{
				CheckBoxText = model.CheckBoxText,
				IsCheckbox = model.IsCheckbox,
				IsDate = model.IsDate,
				IsDeleted = false,
				IsFile = model.IsFile,
				IsCamira = model.IsCamira,
				FileAcceptedFileExtensions = model.IsFile ? model.FileAcceptedFileExtensions : null,
				FileNumberOfMaximumFilesAllowed = model.IsFile && model.FileNumberOfMaximumFilesAllowed.HasValue && model.FileNumberOfMaximumFilesAllowed > 0 ? model.FileNumberOfMaximumFilesAllowed : null,
				FileNumberOfMinimumFilesAllowed = model.IsFile && model.FileNumberOfMinimumFilesAllowed.HasValue && model.FileNumberOfMinimumFilesAllowed > 0 ? model.FileNumberOfMinimumFilesAllowed : null,
				IsRadioButton = model.IsRadioButton,
				IsRequired = model.IsRequired,
				IsSelect = model.IsSelect,
				SelectBoxOptionText = model.IsSelect ? model.SelectBoxOptionText : null,
				IsTextarea = model.IsTextarea,
				TextareaMaxLength = model.IsTextarea ? model.TextareaMaxLength : null,
				TextareaMinLength = model.IsTextarea ? model.TextareaMinLength : null,
				IsTextbox = model.IsTextbox,
				TextboxMaxLength = model.IsTextbox ? model.TextboxMaxLength : null,
				TextboxMinLength = model.IsTextbox ? model.TextboxMinLength : null,
				IsTextboxEmail = model.IsTextbox ? model.IsTextboxEmail : false,
				IsTextboxPassword = model.IsTextbox ? model.IsTextboxPassword : false,
				IsTextboxNumber = model.IsTextbox ? model.IsTextboxNumber : false,
				Label = model.Label?.Trim(),
				Notes = model.Notes?.Trim(),
				Order = model.Order < 0 ? 0 : model.Order,
				SurveyFormId = model.SurveyFormId,
			};
			
			if (model.IsFile == true)
			{
				if (model.IsCamira == true)
				{
					surveyElement.FileAcceptedFileExtensions = "";
				}
			};
			
			_context.SurveyFormElements.Add(surveyElement);

			if (model.IsRadioButton)
			{
				surveyElement.Items = new List<SurveyFormElementItem>();
				var options = model.RadioButtonOptions.Trim().Split(":").Where(opt => !string.IsNullOrWhiteSpace(opt)).ToList();
				options.ForEach(opt => surveyElement.Items.Add(new SurveyFormElementItem()
				{
					IsDeleted = false,
					Text = opt.Trim()
				}));
			}
			if (model.IsCheckbox)
			{
				if (surveyElement.Items == null)
				{
					surveyElement.Items = new List<SurveyFormElementItem>();
				}
				var options = model.CheckBoxText.Trim().Split(":").Where(opt => !string.IsNullOrWhiteSpace(opt)).ToList();
				options.ForEach(opt => surveyElement.Items.Add(new SurveyFormElementItem()
				{
					IsDeleted = false,
					Text = opt.Trim()
				}));
			}

			if (model.IsSelect)
			{
				if (surveyElement.Items == null)
				{
					surveyElement.Items = new List<SurveyFormElementItem>();
				}

				var options = model.SelectOptions.Trim().Split(":").Where(opt => !string.IsNullOrWhiteSpace(opt)).ToList();
				options.ForEach(opt => surveyElement.Items.Add(new SurveyFormElementItem()
				{
					IsDeleted = false,
					Text = opt.Trim()
				}));
			}

			await _context.SaveChangesAsync();

			return new
			{
				msg = "تم إضافة العنصر "
			};
		}

		public async Task<dynamic> EditSurveyElement(ModelStateDictionary modelState, SurveyFormElementVm model)
		{
			if (string.IsNullOrWhiteSpace(model.Label))
			{
				modelState.AddModelError("خطأ فى البيانات", "برجاء ادخال اسم للعنصر");
				return null;
			}

			if (model.IsRadioButton && string.IsNullOrWhiteSpace(model.RadioButtonOptions))
			{
				modelState.AddModelError("خطأ فى البيانات", "برجاء ادخال اختيارات العنصر Radio Button");
				return null;
			}

			if (model.IsSelect && string.IsNullOrWhiteSpace(model.SelectOptions))
			{
				modelState.AddModelError("خطأ فى البيانات", "برجاء ادخال اختيارات العنصر اختيار من متعدد");
				return null;
			}

			if (model.IsCheckbox && string.IsNullOrWhiteSpace(model.CheckBoxText))
			{
				modelState.AddModelError("خطأ فى البيانات", "برجاء ادخال االنص الخاص بخانه الإختيار");
				return null;
			}

			var surveyElement = _context.SurveyFormElements.Include(e => e.Items).FirstOrDefault(s => s.Id == model.Id && !s.IsDeleted && !s.SurveyForm.IsDeleted);
			if (surveyElement == null)
			{
				modelState.AddModelError("خطأ فى البيانات", "العصنر المطلوب غير متوفر");
				return null;
			}

			if (model.IsTextarea && model.TextareaMaxLength.HasValue && model.TextareaMaxLength.Value <= 0)
			{
				model.TextareaMaxLength = null;
			}

			surveyElement.CheckBoxText = model.CheckBoxText;
			surveyElement.IsCheckbox = model.IsCheckbox;
			surveyElement.IsDate = model.IsDate;
			surveyElement.IsFile = model.IsFile;
			surveyElement.IsCamira = model.IsCamira;
			surveyElement.FileAcceptedFileExtensions = model.IsFile ? model.FileAcceptedFileExtensions : null;
			surveyElement.FileNumberOfMaximumFilesAllowed = model.IsFile && model.FileNumberOfMaximumFilesAllowed.HasValue && model.FileNumberOfMaximumFilesAllowed > 0 ? model.FileNumberOfMaximumFilesAllowed : null;
			surveyElement.FileNumberOfMinimumFilesAllowed = model.IsFile && model.FileNumberOfMinimumFilesAllowed.HasValue && model.FileNumberOfMinimumFilesAllowed > 0 ? model.FileNumberOfMinimumFilesAllowed : null;
			surveyElement.IsRadioButton = model.IsRadioButton;
			surveyElement.IsRequired = model.IsRequired;
			surveyElement.IsSelect = model.IsSelect;
			surveyElement.SelectBoxOptionText = model.IsSelect ? model.SelectBoxOptionText : null;
			surveyElement.IsTextarea = model.IsTextarea;
			surveyElement.TextareaMaxLength = model.IsTextarea ? model.TextareaMaxLength : null;
			surveyElement.TextareaMinLength = model.IsTextarea ? model.TextareaMinLength : null;
			surveyElement.IsTextbox = model.IsTextbox;
			surveyElement.IsTextboxEmail = model.IsTextbox ? model.IsTextboxEmail : false;
			surveyElement.IsTextboxPassword = model.IsTextbox ? model.IsTextboxPassword : false;
			surveyElement.IsTextboxNumber = model.IsTextbox ? model.IsTextboxNumber : false;
			surveyElement.TextboxMaxLength = model.IsTextbox ? model.TextboxMaxLength : null;
			surveyElement.TextboxMinLength = model.IsTextbox ? model.TextboxMinLength : null;
			surveyElement.Label = model.Label?.Trim();
			surveyElement.Notes = model.Notes?.Trim();
			surveyElement.Order = model.Order < 0 ? 0 : model.Order;
			surveyElement.SurveyFormId = model.SurveyFormId;

			if (surveyElement.Items != null && surveyElement.Items.Any())
			{
				surveyElement.Items.ToList().ForEach(e => e.IsDeleted = true);
			}

			if (model.IsRadioButton)
			{
				if (surveyElement.Items == null)
				{
					surveyElement.Items = new List<SurveyFormElementItem>();
				}
				var options = model.RadioButtonOptions.Trim().Split(":").Where(opt => !string.IsNullOrWhiteSpace(opt)).ToList();
				options.ForEach(opt => surveyElement.Items.Add(new SurveyFormElementItem()
				{
					IsDeleted = false,
					Text = opt.Trim()
				}));
			}
			if (model.IsCheckbox)
			{
				if (surveyElement.Items == null)
				{
					surveyElement.Items = new List<SurveyFormElementItem>();
				}
				var options = model.CheckBoxText.Trim().Split(":").Where(opt => !string.IsNullOrWhiteSpace(opt)).ToList();
				options.ForEach(opt => surveyElement.Items.Add(new SurveyFormElementItem()
				{
					IsDeleted = false,
					Text = opt.Trim()
				}));
			}

			if (model.IsSelect)
			{
				if (surveyElement.Items == null)
				{
					surveyElement.Items = new List<SurveyFormElementItem>();
				}

				var options = model.SelectOptions.Trim().Split(":").Where(opt => !string.IsNullOrWhiteSpace(opt)).ToList();
				options.ForEach(opt => surveyElement.Items.Add(new SurveyFormElementItem()
				{
					IsDeleted = false,
					Text = opt.Trim()
				}));
			}

			await _context.SaveChangesAsync();

			return new
			{
				msg = "تم تعديل العنصر "
			};
		}

		public SurveyFormElementVm GetElement(int elementId)
		{
			var element = _context.SurveyFormElements.Include(e => e.Items.Where(e => !e.IsDeleted)).FirstOrDefault(e => e.Id == elementId && !e.IsDeleted && !e.SurveyForm.IsDeleted);
			if (element == null)
			{
				return null;
			}

			SurveyFormElementVm elementVm = new SurveyFormElementVm()
			{
				Id = element.Id,
				CheckBoxText = element.CheckBoxText,
				FileAcceptedFileExtensions = element.FileAcceptedFileExtensions,
				IsCheckbox = element.IsCheckbox,
				IsDate = element.IsDate,
				IsFile = element.IsFile,
				IsCamira = element.IsCamira,
				IsRadioButton = element.IsRadioButton,
				IsRequired = element.IsRequired,
				IsSelect = element.IsSelect,
				IsTextarea = element.IsTextarea,
				IsTextbox = element.IsTextbox,
				IsTextboxEmail = element.IsTextboxEmail,
				IsTextboxNumber = element.IsTextboxNumber,
				IsTextboxPassword = element.IsTextboxPassword,
				Label = element.Label,
				Notes = element.Notes,
				Order = element.Order,
				SelectBoxOptionText = element.SelectBoxOptionText,
				SurveyFormId = element.SurveyFormId,
				TextareaMaxLength = element.TextareaMaxLength,
				FileNumberOfMaximumFilesAllowed = element.FileNumberOfMaximumFilesAllowed,
				FileNumberOfMinimumFilesAllowed = element.FileNumberOfMinimumFilesAllowed,
				TextareaMinLength = element.TextareaMinLength,
				TextboxMaxLength = element.TextboxMaxLength,
				TextboxMinLength = element.TextboxMinLength
			};

			if (element.IsSelect && element.Items != null && element.Items.Any())
			{
				elementVm.SelectOptions = string.Join(':', element.Items.Select(e => e.Text));
			}

			if (element.IsRadioButton && element.Items != null && element.Items.Any())
			{
				elementVm.RadioButtonOptions = string.Join(':', element.Items.Select(e => e.Text));
			}

			return elementVm;
		}

		public async Task<bool> DeleteLogo(int surveyId, bool isFirst)
		{
			var survey = _context.SurveyForms.FirstOrDefault(s => s.Id == surveyId && !s.IsDeleted);
			if (survey == null)
			{
				return false;
			}

			survey.Logo1Text = isFirst ? null : survey.Logo1Text;
			survey.Logo1Url = isFirst ? null : survey.Logo1Url;

			survey.Logo2Text = !isFirst ? null : survey.Logo2Text;
			survey.Logo2Url = !isFirst ? null : survey.Logo2Url;

			await _context.SaveChangesAsync();
			return true;
		}

		public SurveyForm GetByFormIdentifier(Guid formId)
		{
			return _context.SurveyForms
				.Include(s => s.CaseType).Include(s => s.Bank).Include(s => s.Elements).ThenInclude(e => e.Items).FirstOrDefault(s => s.LinkIdentifier == formId && !s.IsDeleted);
		}

		public async Task<dynamic> SubmitResponse(ModelStateDictionary modelState, IFormCollection form)
		{
			if (int.TryParse(form["SurveyId"], out int surveyId))
			{
				string currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (!string.IsNullOrWhiteSpace(form["UserId"]))
				{
					currentUserId = form["UserId"];
				}



				/*	if (string.IsNullOrWhiteSpace(currentUserId))
					{
						modelState.AddModelError("خطأ فى البيانات", $"برجاء تسجيل الدخول أولاً");
						return null;
					}*/

				var survey = Get(modelState, surveyId, true);
				if (survey != null)
				{
					List<SurveyFormResponse> responses = new List<SurveyFormResponse>();
					Guid groupId = Guid.NewGuid();

					bool isCompanyIdFound = form.TryGetValue("CompanyId", out StringValues companyId);
					bool isCaseIdFound = form.TryGetValue("CaseId", out StringValues CaseId);
					//var isCaseIdIdFound = form["CaseId"];
					/*	if (isCaseIdIdFound == 0 || isCaseIdIdFound == "")
						{
							modelState.AddModelError("خطأ فى البيانات", $"خطأ فى الحاله");
							return null;
						}

	*/

					/*var r = _context.SurveyFormResponses.Where(s=>s.CaseId==CaseId);
					if (r != null )
					{
						modelState.AddModelError("خطأ فى البيانات", $"الحاله لها رد من قبل ");
						return null;
					}*/
					var casesin = _context.SurveyFormResponses.Where(a => a.CaseId == int.Parse(CaseId)).ToList();
					if(casesin !=null)
                    {
						_context.SurveyFormResponses.RemoveRange(casesin);
						_context.SaveChanges();

					}
					foreach (var item in form.Keys.Where(k => k != "SurveyId" && k != "companyId" && k != "CaseId" && k != "UserId"))
					{
						SurveyFormResponse response = new SurveyFormResponse()
						{
							SurveyFormId = surveyId,
							GroupId = groupId,
							UserId = currentUserId,
							CreatedAt = DateTime.Now,
							CompanyId = isCompanyIdFound ? int.Parse(companyId) : null,
                            CaseId = isCaseIdFound ? int.Parse(CaseId) : null
                        };

						var elementId = int.Parse(item.Replace("Element_", ""));
						var element = survey.Elements.FirstOrDefault(e => e.Id == elementId);
						if (element != null)
						{
							response.SurveyFormElementId = elementId;

							var elementValue = form[item].ToString().Trim();

							if (element.IsCheckbox)
							{
								if (element.IsRequired && elementValue.ToLower() != "true")
								{
									modelState.AddModelError("خطأ فى البيانات", $"إجابة السؤال {element.Label} غير صحيحة");
									return null;
								}

								response.Result = element.CheckBoxText;
							}
							else if (element.IsTextbox)
							{
								if (element.IsTextboxEmail)
								{
									string emailSyntax = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
									bool isEmail = Regex.IsMatch(elementValue, emailSyntax, RegexOptions.IgnoreCase);
									if (!isEmail)
									{
										modelState.AddModelError("خطأ فى البيانات", $"البريد الإلكترونى غير صحيح");
										return null;
									}

									if (element.TextboxMinLength.HasValue && elementValue.Length < element.TextboxMinLength.Value)
									{
										modelState.AddModelError("خطأ فى البيانات", $"يجب ألا يقل عدد حروف البريد الإلكترونى عن {element.TextboxMinLength.Value} حرف");
										return null;
									}

									if (element.TextboxMaxLength.HasValue && elementValue.Length > element.TextboxMaxLength.Value)
									{
										modelState.AddModelError("خطأ فى البيانات", $"يجب ألا يزيد عدد حروف البريد الإلكترونى عن {element.TextboxMaxLength.Value} حرف");
										return null;
									}
								}
								else if (element.IsTextboxPassword)
								{
									if (element.TextboxMinLength.HasValue && elementValue.Length < element.TextboxMinLength.Value)
									{
										modelState.AddModelError("خطأ فى البيانات", $"يجب ألا يقل عدد حروف إجابة السؤال {element.Label} عن {element.TextboxMinLength.Value} حرف");
										return null;
									}

									if (element.TextboxMaxLength.HasValue && elementValue.Length > element.TextboxMaxLength.Value)
									{
										modelState.AddModelError("خطأ فى البيانات", $"يجب ألا يزيد عدد حروف إجابة السؤال {element.Label} عن {element.TextboxMaxLength.Value} حرف");
										return null;
									}
								}
								else if (element.IsTextboxNumber)
								{
									if (element.TextboxMinLength.HasValue && int.Parse(elementValue) < element.TextboxMinLength.Value)
									{
										modelState.AddModelError("خطأ فى البيانات", $"يجب ألا يقل رقم إجابة السؤال {element.Label} عن {element.TextboxMinLength.Value} حرف");
										return null;
									}

									if (element.TextboxMaxLength.HasValue && int.Parse(elementValue) > element.TextboxMaxLength.Value)
									{
										modelState.AddModelError("خطأ فى البيانات", $"يجب ألا يزيد رقم إجابة السؤال {element.Label} عن {element.TextboxMaxLength.Value} حرف");
										return null;
									}
								}
								response.Result = elementValue;
							}
							else if (element.IsTextarea)
							{
								if (element.TextareaMinLength.HasValue && elementValue.Length < element.TextareaMinLength.Value)
								{
									modelState.AddModelError("خطأ فى البيانات", $"يجب ألا يقل عدد حروف إجابة السؤال {element.Label} عن {element.TextareaMinLength.Value} حرف");
									return null;
								}

								if (element.TextareaMaxLength.HasValue && elementValue.Length > element.TextareaMaxLength.Value)
								{
									modelState.AddModelError("خطأ فى البيانات", $"يجب ألا يزيد عدد حروف إجابة السؤال {element.Label} عن {element.TextareaMaxLength.Value} حرف");
									return null;
								}

								response.Result = elementValue;
							}
							else if (element.IsRadioButton || element.IsSelect)
							{
								response.Result = element.Items.FirstOrDefault(i => i.Id == int.Parse(elementValue))?.Text;
							}
							else if (element.IsDate)
							{
								response.Result = elementValue;
							}

							responses.Add(response);
						}
					}

					if (form.Files.Any())
					{
						foreach (var items in form.Files.GroupBy(f => f.Name).ToList())
						{
							SurveyFormResponse response = new SurveyFormResponse()
							{
								SurveyFormId = surveyId,
								GroupId = groupId,
								IsFile = true,
								FileUrls = "",
								CreatedAt = DateTime.Now,
								UserId = currentUserId,
								CompanyId = isCompanyIdFound ? int.Parse(companyId) : null,
								CaseId = isCaseIdFound ? int.Parse(CaseId) : null
							};

							var elementId = int.Parse(items.Key.Replace("Element_", ""));
							var element = survey.Elements.FirstOrDefault(e => e.Id == elementId);
							if (element != null)
							{
								response.SurveyFormElementId = elementId;

								if (element.FileNumberOfMinimumFilesAllowed.HasValue)
								{
									var totalFilesCount = items.Count();
									if (totalFilesCount < element.FileNumberOfMinimumFilesAllowed)
									{
										modelState.AddModelError("خطأ فى البيانات", $"الملفات المرفقة للسؤال {element.Label} أقل من العدد المطلوب");
										return null;
									}
								}

								if (element.FileNumberOfMaximumFilesAllowed.HasValue)
								{
									var totalFilesCount = items.Count();
									if (totalFilesCount > element.FileNumberOfMinimumFilesAllowed)
									{
										modelState.AddModelError("خطأ فى البيانات", $"الملفات المرفقة للسؤال {element.Label} أكثر من العدد المطلوب");
										return null;
									}
								}

								foreach (var file in items)
								{
									if (!string.IsNullOrWhiteSpace(element.FileAcceptedFileExtensions))
									{
										string extensions = element.FileAcceptedFileExtensions?.Replace(" ", "").Replace(".", "");
										List<string> allowedExtensions = element.FileAcceptedFileExtensions.Trim().Split(":").Where(e => !string.IsNullOrWhiteSpace(e)).Select(e => "." + e.Trim()).ToList();
										if (allowedExtensions.Any() && !allowedExtensions.Contains(Path.GetExtension(file.FileName)))
										{
											modelState.AddModelError("خطأ فى البيانات", $"الملفات المرفقة للسؤال {element.Label} غير صحيحة");
											return null;
										}
									}

									if (response.FileUrls.Length > 0)
									{
										response.FileUrls += ",";
									}

									if (Files.IsImage(file))
									{
										response.FileUrls += Files.SaveImage(file, _environment);
									}
									else if (Files.IsWord(file))
									{
										response.FileUrls += Files.SaveWord(file, _hostingEnvironment);
									}
									else if (Files.IsExcel(file))
									{
										response.FileUrls += Files.SaveWord(file, _hostingEnvironment);
									}
									else
									{
										response.FileUrls += Files.SaveImage(file, _environment);
									}
								}

								responses.Add(response);
							}
						}
					}
                    var r =  _context.SurveyFormResponses.ToList();

					if (r != null && r.Count() <= 1)
					{
						var x = r.Where(s => s.SurveyFormId == surveyId && s.UserId == currentUserId && s.GroupId == groupId && s.CaseId == CaseId && s.CompanyId == companyId);
						if (x != null)
						{
							_context.SurveyFormResponses.RemoveRange(r);
							await _context.SaveChangesAsync();
						}
					}

                    _context.SurveyFormResponses.AddRange(responses);
					await _context.SaveChangesAsync();

					return new
					{
						msg = "تم إرسال الرد بنجاح "
					};
				}
			}
			modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذا الإستعلام");
			return null;
		}

		public MemoryStream GenerateResponseWordDocument(int id)
		{
			var groupedResponses = _context.SurveyFormResponses.Include(r => r.SurveyFormElement).Include(a=>a.User).Include(r => r.User).Include(r => r.SurveyForm).ThenInclude(r => r.Bank).Include(r => r.SurveyForm).ThenInclude(r => r.CaseType).Include(r => r.Company).Where(s => s.CaseId == id).ToList();
			if (groupedResponses.Any())
			{
				string templatePath = _environment.WebRootPath +   _surveysConfig.WordTemplatePath;
				if (File.Exists(templatePath))
				{
					using (var document = DocX.Load(templatePath))
					{
						#region Logos
						var logosTable = document.Tables.FirstOrDefault();
						var firstLogoCell = logosTable.Paragraphs.FirstOrDefault(x => x.Text.Contains("##LOGO1##"));
						
						if (firstLogoCell != null)
						{
							string bankLogoPath = groupedResponses.First().SurveyForm.Bank.Logo;
							var pathe = _environment.WebRootPath ;
							//var localfirstLogoCell = SaveDataIntoLocalFolder(bankLogoPath, pathe);
							if (File.Exists(bankLogoPath))
							{
								firstLogoCell.InsertPicture(document.AddImage(bankLogoPath).CreatePicture(80, 80), 0);
							}

							firstLogoCell.ReplaceText("##LOGO1##", "");
						}

						var secondLogoCell = logosTable.Paragraphs.FirstOrDefault(x => x.Text.Contains("##LOGO2##"));
						if (secondLogoCell != null)
						{
							if (groupedResponses.First().Company != null && !string.IsNullOrWhiteSpace(groupedResponses.First().Company.Logo))
							{
								string companyLogoPath = _environment.WebRootPath + groupedResponses.First().Company.Logo.Split("images/")[1];
								if (File.Exists(companyLogoPath))
								{
									secondLogoCell.InsertPicture(document.AddImage(companyLogoPath).CreatePicture(80, 80), 0);
								}
							}

							secondLogoCell.ReplaceText("##LOGO2##", "");
						}
						#endregion

						var dataTable = document.Tables.Skip(1).FirstOrDefault();
						dataTable.InsertColumn();

						#region Header
						string bankName = groupedResponses.First().SurveyForm.Bank.NameAr?.Replace("البنك", "").Replace("بنك", "").Trim();
						string caseType = groupedResponses.First().SurveyForm.CaseType.NameAr;
						string headerTitle = $"تقرير استعلام بنك " + bankName + " - " + caseType;

						var headerParagraph = dataTable.Paragraphs.FirstOrDefault(x => x.Text.Contains("##Header##"));
						if (headerParagraph != null)
						{
							headerParagraph.ReplaceText("##Header##", headerTitle);
						}
						var headerRow = dataTable.Rows.FirstOrDefault();
						headerRow.MergeCells(0, 1);
						headerRow.Cells.ForEach(c => { c.MarginBottom = 15; c.MarginTop = 15; });
						#endregion

						#region Data
						Dictionary<string, (bool isFile, string value)> data = new Dictionary<string, (bool isFile, string value)>();
						/*	data.Add("التاريخ", (false, DateTime.Now.ToString("yyyy/MM/dd")));
							data.Add("اسم العميل", (false, groupedResponses.First().User.FullName));
	*/
						foreach (var response in groupedResponses)
						{
							if (response.SurveyFormElement.IsFile && !string.IsNullOrWhiteSpace(response.FileUrls))
							{
								data.Add(response.SurveyFormElement.Label, (true, response.FileUrls.Split("images/")[1]));
							}
							else
							{
								data.Add(response.SurveyFormElement.Label, (false, response.Result));
							}
						}
						data.Add("الموقع",(false,"lat "+ groupedResponses.FirstOrDefault().User.lat.ToString() +"  lng  " +groupedResponses.FirstOrDefault().User.lng.ToString()));
						foreach (var item in data)
						{
							var dataRow = dataTable.InsertRow();
							dataRow.SetDirection(Direction.RightToLeft);
							dataRow.Cells[0].Paragraphs.FirstOrDefault().InsertText(item.Key);
							dataRow.Cells[0].Paragraphs.FirstOrDefault().Bold(true);
							dataRow.Cells[0].Width = 200;
							dataRow.Cells[0].MarginTop = 7;
							dataRow.Cells[0].MarginBottom = 7;

							if (item.Value.isFile)
							{
								var files = item.Value.value.Split(",").ToList();
								if (files != null && files.Any())
								{
									List<string> imageExtensions = new List<string> { ".jpg", ".jpeg", ".jpe", ".bmp", ".gif", ".png" };
									foreach (var file in files)
									{
										string path =  item.Value.value;
										if (imageExtensions.Contains(Path.GetExtension(file.ToLower())) && File.Exists(path))
										{
											Bitmap img = new Bitmap(path);
											dataRow.Cells[1].Paragraphs.FirstOrDefault().InsertPicture(document.AddImage(path).CreatePicture(img.Height, (float)dataRow.Cells[1].Width - 60), 0);
										}
										else
										{
											dataRow.Cells[1].Paragraphs.FirstOrDefault().InsertText(_httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host+"/" + file + Environment.NewLine);
											dataRow.Cells[1].Paragraphs.FirstOrDefault().Bold(true);
										}

									}
								}
							}
							else
							{
								dataRow.Cells[1].Paragraphs.FirstOrDefault().InsertText(item.Value.value);
								dataRow.Cells[1].Paragraphs.FirstOrDefault().Bold(true);
							}

						}
						#endregion

						MemoryStream ms;
						using (ms = new MemoryStream())
						{
							document.SaveAs(ms);
						}
						return ms;
					}
				}
			}
			return null;
		}
		public async Task<MemoryStream>  GenerateResponseWordDocumentCase(int id)
		{
			var groupedResponses = _context.SurveyFormResponses.Include(r => r.SurveyFormElement).Include(r => r.User).Include(r => r.SurveyForm).ThenInclude(r => r.Bank).Include(r => r.SurveyForm).ThenInclude(r => r.CaseType).Include(r => r.Company).Where(s => s.CaseId == id).ToList();
			if (groupedResponses.Any())
			{
				string templatePath = _environment.WebRootPath + _surveysConfig.WordTemplatePath;
				if (File.Exists(templatePath))
				{
					using (var document = DocX.Load(templatePath))
					{
						#region Logos
						var logosTable = document.Tables.FirstOrDefault();
						var firstLogoCell = logosTable.Paragraphs.FirstOrDefault(x => x.Text.Contains("##LOGO1##"));
						
						if (firstLogoCell != null)
						{
							if (!Directory.Exists(_environment.WebRootPath + "/uploads/"))
							{
								Directory.CreateDirectory(_environment.WebRootPath + "/uploads/");
							}
							var localfirstLogoCell = SaveDataIntoLocalFolder(groupedResponses.First().SurveyForm.Bank.Logo, groupedResponses.First().SurveyForm.Bank.Logo.Split("images/")[1]);

							string bankLogoPath = _environment.WebRootPath+"/uploads/" + groupedResponses.First().SurveyForm.Bank.Logo.Split("images/")[1];
							if (File.Exists(bankLogoPath))
							{
								firstLogoCell.InsertPicture(document.AddImage(bankLogoPath).CreatePicture(100, 100), 0);
								
							}

							firstLogoCell.ReplaceText("##LOGO1##", "");
						}

						var secondLogoCell = logosTable.Paragraphs.FirstOrDefault(x => x.Text.Contains("##LOGO2##"));
						if (secondLogoCell != null)
						{
							var localsecondLogoCell = SaveDataIntoLocalFolder(groupedResponses.First().Company.Logo, groupedResponses.First().Company.Logo.Split("images/")[1]);

							if (groupedResponses.First().Company != null && !string.IsNullOrWhiteSpace(groupedResponses.First().Company.Logo))
							{
								string companyLogoPath = _environment.WebRootPath + "/uploads/"  + groupedResponses.First().Company.Logo.Split("images/")[1];
								if (File.Exists(companyLogoPath))
								{
									secondLogoCell.InsertPicture(document.AddImage(companyLogoPath).CreatePicture(80, 80), 0);
								}
							}

							secondLogoCell.ReplaceText("##LOGO2##", "");
						}
						#endregion

						var dataTable = document.Tables.Skip(1).FirstOrDefault();
						dataTable.InsertColumn();

						#region Header
						string bankName = groupedResponses.First().SurveyForm.Bank.NameAr?.Replace("البنك", "").Replace("بنك", "").Trim();
						string caseType = groupedResponses.First().SurveyForm.CaseType.NameAr;
						string headerTitle = $"تقرير استعلام بنك " + bankName + " - " + caseType;

						var headerParagraph = dataTable.Paragraphs.FirstOrDefault(x => x.Text.Contains("##Header##"));
						if (headerParagraph != null)
						{
							headerParagraph.ReplaceText("##Header##", headerTitle);
						}
						var headerRow = dataTable.Rows.FirstOrDefault();
						headerRow.MergeCells(0, 1);
						headerRow.Cells.ForEach(c => { c.MarginBottom = 15; c.MarginTop = 15; });
						#endregion

						#region Data
						Dictionary<string, (bool isFile, string value)> data = new Dictionary<string, (bool isFile, string value)>();
					/*	data.Add("التاريخ", (false, DateTime.Now.ToString("yyyy/MM/dd")));
						data.Add("اسم العميل", (false, groupedResponses.First().User.FullName));
*/
						foreach (var response in groupedResponses)
						{
							if (response.SurveyFormElement.IsFile && !string.IsNullOrWhiteSpace(response.FileUrls))
							{
								data.Add(response.SurveyFormElement.Label, (true, response.FileUrls));
							}
							else
							{
								data.Add(response.SurveyFormElement.Label, (false, response.Result));
							}
						}
						data.Add("الموقع", (false, "lat " + groupedResponses.FirstOrDefault().User.lat.ToString() + "  lng  " + groupedResponses.FirstOrDefault().User.lng.ToString()));

						foreach (var item in data)
						{
							var dataRow = dataTable.InsertRow();
							dataRow.SetDirection(Direction.RightToLeft);
							dataRow.Cells[0].Paragraphs.FirstOrDefault().InsertText(item.Key);
							dataRow.Cells[0].Paragraphs.FirstOrDefault().Bold(true);
							dataRow.Cells[0].Width = 200;
							dataRow.Cells[0].MarginTop = 7;
							dataRow.Cells[0].MarginBottom = 7;

							if (item.Value.isFile)
							{
								var files = item.Value.value.Split(",").ToList();
								if (files != null && files.Any())
								{
									List<string> imageExtensions = new List<string> { ".jpg", ".jpeg", ".jpe", ".bmp", ".gif", ".png" };
									foreach (var file in files)
									{
										var localsecondLogoCell = SaveDataIntoLocalFolder(file, file.Split("images/")[1]);

										string path = _environment.WebRootPath+ "/uploads/" + file.Split("images/")[1];
										if (imageExtensions.Contains(Path.GetExtension(file.Split("images/")[1].ToLower())) && File.Exists(path))
										{
											Bitmap img = new Bitmap(path);
											dataRow.Cells[1].Paragraphs.FirstOrDefault().InsertPicture(document.AddImage(path).CreatePicture(100, 100),	0);
											dataRow.Cells[1].Width=15;
										}
										else
										{
											dataRow.Cells[1].Paragraphs.FirstOrDefault().InsertText( file + Environment.NewLine);
											dataRow.Cells[1].Paragraphs.FirstOrDefault().Bold(true);
										}

									}
								}
							}
							else
							{
								dataRow.Cells[1].Paragraphs.FirstOrDefault().InsertText(item.Value.value);
								dataRow.Cells[1].Paragraphs.FirstOrDefault().Bold(true);
							}

						}
						#endregion

						MemoryStream ms;
						using (ms = new MemoryStream())
						{
							document.SaveAs(ms);
						}
						ms.Close();
						return ms;
					}
				}
			}
			return null;
		}
        public async Task SaveDataIntoLocalFolder(string url, string fileName)
        {
			using (WebClient client = new WebClient())
			{
				client.DownloadFile(new Uri(url), _environment.WebRootPath + "/uploads/" + fileName);

			}
		}
/*		public async Task SaveDataIntoLocalFolder(string url, string fileName)
		{
			Uri source = new Uri(url);
			StorageFile destinationFile;
			try
			{
				destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
					url.Split("images/")[1], CreationCollisionOption.GenerateUniqueName);
			}
			catch (FileNotFoundException ex)
			{
				System.Diagnostics.Debug.WriteLine("bingooooo" + ex.ToString());
				return;
			}
			BackgroundDownloader downloader = new BackgroundDownloader();
			DownloadOperation download = downloader.CreateDownload(source, destinationFile);
			await download.StartAsync();
			ResponseInformation response = download.GetResponseInformation();
		}*/
		public List<SurveyFormResponse> GetUserResponse(string userId, Guid formId, int? companyId)
		{
			var query = _context.SurveyFormResponses.Where(r => r.UserId == userId && r.SurveyForm.LinkIdentifier == formId);
			if (companyId.HasValue)
			{
				query = query.Where(r => r.CompanyId == companyId);
			}

			var response = query.ToList().GroupBy(r => r.GroupId).FirstOrDefault()?.Select(r => r).ToList();
			return response;
		}
	}
}
