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

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class BanksBussniess : IBanks
    {
      
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public BanksBussniess( IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            
            _context = context;
            _environment = environment;
        }

        public dynamic Get(ModelStateDictionary modelState)

        {
            var banks = _context.Banks.Select(s => new Banks
            {
                Id = s.Id,
                NameAr = s.NameAr ?? "",
                NameEn = s.NameEn ?? "",
                Active=s.Active,
                Logo=s.Logo,
                CreatedAt=s.CreatedAt

            }).ToList();
            return banks;
        }
        



        public async Task<dynamic> Add(ModelStateDictionary modelState, Banks model)
        {
            try
            {
                var x = _context.Banks.FirstOrDefault(b => b.NameAr == model.NameAr|| b.NameEn == model.NameEn);
                if (x==null)
                {
                    
                    try
                    {
                        model.Logo = Files.SaveImage(model.Logofile, _environment);
                    }
                    catch (Exception)
                    {

                        
                    }
                    // first we create Admin rool    

                    _context.Banks.Add(model);
                    await _context.SaveChangesAsync();


                    return new
                    {
                        result = new
                        {

                        },
                        msg = "تم اضافة البنك بنجاح"
                    };

                }
                else
                {
                    modelState.AddModelError("تداخل بيانات", "هذا البنك موجود من قبل");
                    return null;
                }
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
                var x = _context.Banks.FirstOrDefault(r => r.Id == Id);
                if (x==null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذ البنك");
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

        public async Task<dynamic> Edit(ModelStateDictionary modelState, Banks model)
        {
            try
            {

                var bank = _context.Banks.FirstOrDefault(r => r.Id == model.Id);
                if (bank == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذ البنك");
                    return null;
                }

                if (model.Logofile != null )
                {
                    try
                    {
                        model.Logo = Files.SaveImage(model.Logofile, _environment);
                        bank.Logo = model.Logo;
                    }
                    catch (Exception)
                    {


                    }
                }

                bank.NameAr = model.NameAr;
                bank.NameEn = model.NameEn;
                bank.Active = model.Active;
               await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم تعديل البنك بنجاح"
                };

            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

        public async Task<dynamic>  Delete(ModelStateDictionary modelState, int Id)
        {
            try
            {
                var bank = _context.Banks.FirstOrDefault(r => r.Id == Id);
                if (bank == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذا البنك");
                    return null;
                }


                 _context.Banks.Remove(bank);
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف  البنك"
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError("تحذير", "يجب حذف كل البيانات المتعلقة بالبنك اولا");
                return null;
            }
        }

		public Companies GetCompany(int? bankId, int? companyId, int? caseTypeId)
		{
            return _context.Cases.Include(c => c.Company).FirstOrDefault(b => b.BankId == bankId && b.CompanyId == companyId && b.CaseTypeId == caseTypeId)?.Company;
		}
	}

}
