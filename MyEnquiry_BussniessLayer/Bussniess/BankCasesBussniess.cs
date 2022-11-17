using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModel;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MyEnquiry.Helper;

namespace MyEnquiry_BussniessLayer.Bussniess
{
	public class BankCasesBussniess : IBankCases
	{

		private IConfiguration _configuration;
		private MyAppContext _context;
		public static IWebHostEnvironment _environment;

		public BankCasesBussniess(IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
		{
			_configuration = configuration;

			_context = context;
			_environment = environment;
		}



		public dynamic GetCaseTypes(ModelStateDictionary modelState)
		{
			var types = _context.CaseTypes.Where(b => b.Active).Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

			return types;
		}

		public dynamic GetCompanies(ModelStateDictionary modelState)
		{
			var companies = _context.Companies.Where(b => b.Active && !b.Deleted).Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

			return companies;
		}
		public dynamic Get(ModelStateDictionary modelState, ClaimsPrincipal user)

		{
			var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			var getUser = _context.Users.Where(a => !a.Deleted && a.Id == userId).FirstOrDefault();
			var bankcase = 
				_context.Cases.Include(s => s.CaseType).Include(s => s.Company)
				.Include(s => s.Bank).ThenInclude(s => s.SurveyForms)
				.Where(s => !s.Deleted && s.CaseStatusId == (int)CaseEnumStatus.SentFromBank && s.BankId== getUser.BankId).Select(s => new Cases
			{
				Id = s.Id,
				Company = s.Company,
				CaseType = s.CaseType,
				FileToShow = s.CaseFiles.FirstOrDefault() != null ? s.CaseFiles.FirstOrDefault().ExcelSheet : "",
				CreatedAt = s.CreatedAt,
				Bank = s.Bank,
				CaseTypeId = s.CaseTypeId,
				CompanyId = s.CompanyId
			}).ToList();
			return bankcase;
		}
		public async Task<dynamic> Add(ModelStateDictionary modelState, Cases model, ClaimsPrincipal user)
		{
			try
			{

				var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null)
				{
					modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
					return null;
				}

				var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);

				if ((checkuser.UserType != 1 || checkuser.BankId == null))
				{
					modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
					return null;
				}

				var casefile = new CaseFiles();
				try
				{
					var excelsheet = Files.SaveExcel(model.File, _environment);

					casefile.ExcelSheet = excelsheet;
					casefile.Type = 1;
				}
				catch (Exception)
				{


				}

				model.CaseStatusId = (int)CaseEnumStatus.SentFromBank;
				model.BankId = (int)checkuser.BankId;
				model.CaseFiles.Add(casefile);
				_context.Cases.Add(model);
				await _context.SaveChangesAsync();


				return new
				{
					result = new
					{

					},
					msg = "تم ارسال الحالة بنجاح"
				};


			}
			catch (Exception ex)
			{
				modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
				return null;

			}



		}

		public dynamic GetById(ModelStateDictionary modelState, int Id)
		{
			try
			{
				var x = _context.Cases.FirstOrDefault(r => r.Id == Id);
				if (x == null)
				{
					modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
					return null;
				}



				return x;

			}
			catch (Exception ex)
			{

				modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
				return null;
			};
		}

		public async Task<dynamic> Edit(ModelStateDictionary modelState, Cases model)
		{
			try
			{

				var caseedit = _context.Cases.FirstOrDefault(r => r.Id == model.Id);
				if (caseedit == null)
				{
					modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
					return null;
				}

				if (model.File != null)
				{
					try
					{
						var casefile = Files.SaveImage(model.File, _environment);
						var checkhasfile = _context.CaseFiles.FirstOrDefault(s => s.CaseId == model.Id);

						if (checkhasfile != null)
						{
							checkhasfile.ExcelSheet = casefile;
						}
						else
						{
							var casefiletoadd = new CaseFiles();
							casefiletoadd.ExcelSheet = casefile;
							casefiletoadd.CaseId = model.Id;
							_context.CaseFiles.Add(casefiletoadd);
						}


					}
					catch (Exception)
					{


					}
				}

				caseedit.CompanyId = model.CompanyId;
				caseedit.CaseTypeId = model.CaseTypeId;

				await _context.SaveChangesAsync();

				return new
				{
					result = new
					{

					},
					msg = "تم تعديل الحالة بنجاح"
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
				var casedelete = _context.Cases.FirstOrDefault(r => r.Id == Id);
				if (casedelete == null)
				{
					modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
					return null;
				}
				if (casedelete.CaseStatusId != 1)
				{
					modelState.AddModelError("غير مسموح", "عفوا و لا تستطيع حذف هذه الحالة لانه تم استلامها من قبل الشركة");
					return null;
				}

				casedelete.Deleted = true;

				await _context.SaveChangesAsync();

				return new
				{
					result = new
					{

					},
					msg = "تم حذف  الحالة"
				};
			}
			catch (Exception ex)
			{
				modelState.AddModelError("تحذير", "يجب حذف كل البيانات المتعلقة بالحالة اولا");
				return null;
			}
		}




	}

}
