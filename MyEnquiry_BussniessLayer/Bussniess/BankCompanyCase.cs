using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModel;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class BankCompanyCase : IBankCompanyCase
    {
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public BankCompanyCase(IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }



        public async Task<dynamic> Delete(ModelStateDictionary modelState, int Id)
        {
            try
            {
                var BankCompanydelete = _context.BankCompany.FirstOrDefault(r => r.Id == Id);
                if (BankCompanydelete == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
                    return null;
                }
                /*  if (BankCompanydelete.CaseStatusId != 1)
                  {
                      modelState.AddModelError("غير مسموح", "عفوا و لا تستطيع حذف هذه الحالة لانه تم استلامها من قبل الشركة");
                      return null;
                  }*/

                BankCompanydelete.Deleted = true;

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

  

        public dynamic GetCompany(ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }

            var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);

            if ((checkuser.UserType != 2 || checkuser.CompanyId == null) )
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }
            var CompanyBank = _context.Banks.Where(a=>!a.Deleted).ToList();
      
            return CompanyBank;
        }     
        public dynamic Get(ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }

            var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);

            if ((checkuser.UserType != 1 || checkuser.BankId == null) )
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }
            var CompanyBank = _context.Companies.Where(a=>!a.Deleted).ToList();
      
            return CompanyBank;
        }

        public dynamic GetBankCompany(ModelStateDictionary modelState, ClaimsPrincipal user, int Id)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user3 = _context.Users.Include(a => a.Bank).Where(a => !a.Deleted).FirstOrDefault();

            if (userId == null)
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }

            var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);

            if ((checkuser.UserType != 1 || checkuser.BankId == null) )
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }
        
            var banks = _context.BankCompany.Where(a=>!a.Deleted&&a.CompaniesId==Id&&a.BanksId== checkuser.BankId).Include(a => a.Banks).Include(a => a.Companies).Include(a => a.Cities).Include(a => a.CaseTypes).Select(s => new BankCompanyVm
            {
                Id = s.Id,
                BanksId = s.Banks.NameAr ?? "",
                CitiesId = s.Cities.NameAr ?? "",
                CaseTypesId = s.CaseTypes.NameAr ?? "",
                CompaniesId = s.Companies.NameAr,
                PriceCase = s.PriceCase

            }).ToList();
            return banks;
        }    
        public dynamic GetAllCompany(ModelStateDictionary modelState, ClaimsPrincipal user, int Id)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user3 = _context.Users.Include(a => a.Bank).Where(a => !a.Deleted).FirstOrDefault();

            if (userId == null)
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }

            var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);

            if ((checkuser.UserType != 2 || checkuser.CompanyId == null))
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }
        
            var banks = _context.BankCompany.Where(a=>!a.Deleted&&a.BanksId==Id&&a.CompaniesId== checkuser.CompanyId).Include(a => a.Banks).Include(a => a.Companies).Include(a => a.Cities).Include(a => a.CaseTypes).Select(s => new BankCompanyVm
            {
                Id = s.Id,
                BanksId = s.Banks.NameAr ?? "",
                CitiesId = s.Cities.NameAr ?? "",
                CaseTypesId = s.CaseTypes.NameAr ?? "",
                CompaniesId = s.Companies.NameAr,
                PriceCase = s.PriceCase

            }).ToList();
            return banks;
        }
        public dynamic GetById(ModelStateDictionary modelState, int Id)
        {
            try
            {
                var x = _context.BankCompany.FirstOrDefault(r => r.Id == Id);
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

        public dynamic GetCaseTypes(ModelStateDictionary modelState)
        {
            var companies = _context.CaseTypes.Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

            return companies;
        }

        public dynamic GetCities(ModelStateDictionary modelState)
        {
            var companies = _context.Cities.Where(b =>  !b.Deleted).Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

            return companies;
        }

        public async Task<dynamic> Add(ModelStateDictionary modelState,BankCompanyVmAdd model, ClaimsPrincipal user)
        {

            try
            {

                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                //var user3 = _context.Users.Include(a=>a.Bank).Where(a=>!a.Deleted).FirstOrDefault();


                if (userId == null)
                {
                    modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                    return null;
                }

                var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);

                if ((checkuser.UserType != 1 || checkuser.BankId == null) )
                {
                    modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                    return null;
                }    
                if (_context.BankCompany.Any(a=>a.CitiesId==model.CitiesId&&a.CaseTypesId==model.CaseTypesId&&!a.Deleted&&a.BanksId== checkuser.BankId&&a.CompaniesId==model.CompaniesId) )
                {
                    modelState.AddModelError("غير مسموح", "متوفر من قبل برجاء عمل تعديل له");
                    return null;
                }

                var casefile = new MyEnquiry_DataLayer.Models.BankCompany();

                casefile.CitiesId = model.CitiesId;
                casefile.CaseTypesId = model.CaseTypesId;
                casefile.CompaniesId = model.CompaniesId;
                casefile.BanksId = checkuser.BankId;
                casefile.PriceCase = model.PriceCase;
                
                _context.BankCompany.Add(casefile);
                await _context.SaveChangesAsync();



                return new
                {
                    result = new
                    {

                    },
                    msg = "تم إضافة السعر  بنجاح"
                };






            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;

            }
        }

        public async Task <dynamic> Edit(ModelStateDictionary modelState, BankCompanyVmAdd model, ClaimsPrincipal user)
        {
            try
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var user3 = _context.Users.Include(a => a.Bank).Where(a => !a.Deleted).FirstOrDefault();


                if (userId == null)
                {
                    modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                    return null;
                }
                var BankCompanyedit = _context.BankCompany.FirstOrDefault(r => r.Id == model.Id);
                if (BankCompanyedit == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
                    return null;
                }
                if (_context.BankCompany.Any(a => a.CitiesId == model.CitiesId && a.CaseTypesId == model.CaseTypesId && !a.Deleted && a.BanksId == user3.BankId&&a.Id!= model.Id && a.CompaniesId == model.CompaniesId))
                {
                    modelState.AddModelError("غير مسموح", "متوفر من قبل برجاء عمل تعديل له");
                    return null;
                }
                BankCompanyedit.CitiesId = model.CitiesId;
                BankCompanyedit.CaseTypesId = model.CaseTypesId;
                BankCompanyedit.CompaniesId = model.CompaniesId;
                BankCompanyedit.PriceCase = model.PriceCase;

                await _context.SaveChangesAsync();
;

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم تعديل  الحالة بنجاح"
                };

            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

    }
}
