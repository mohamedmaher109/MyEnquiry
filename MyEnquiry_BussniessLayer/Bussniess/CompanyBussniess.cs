using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyEnquiry.Helper;
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
    public class CompanyBussniess : ICompany
    {
      
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public CompanyBussniess( IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            
            _context = context;
            _environment = environment;
        }

        public dynamic Get(ModelStateDictionary modelState)

        {
            var company = _context.Companies.Select(s => new Companies
            {
                Id = s.Id,
                NameAr = s.NameAr ?? "",
                NameEn = s.NameEn ?? "",
                Active=s.Active,
                Logo=s.Logo,
                CreatedAt=s.CreatedAt

            }).ToList();
            return company;
        }





        public dynamic GetStatistics(ModelStateDictionary modelState, ClaimsPrincipal user)

        {

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var getuser = _context.Users.FirstOrDefault(s => s.Id == userId);

            var model = new HomeStatistics();
            model.usertype = getuser.UserType;
            if (getuser.UserType == 1)
            {
                var cases = _context.Cases.Where(s => s.BankId == getuser.BankId && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank).ToList();
                var casesnum = cases.Count;
                var comapnynumber = cases.GroupBy(s => s.CompanyId).Count();
                model.Casesnumber = casesnum.ToString();
                model.companynumber = comapnynumber.ToString();
            }
            else if (getuser.UserType == 2)
            {
                var cases= _context.Cases.Where(s => s.CompanyId == getuser.CompanyId && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank).ToList();
                var rep = _context.Users.Where(s => !s.Deleted && s.UserType == 4&&s.CompanyId==getuser.CompanyId).ToList();
                model.Casesnumber = cases.Count.ToString();
                model.repnumber = rep.Count.ToString();
           
            }
            return model;
        }



        public async Task<dynamic> Add(ModelStateDictionary modelState, Companies model)
        {
            try
            {
                var x = _context.Companies.FirstOrDefault(b => b.NameAr == model.NameAr|| b.NameEn == model.NameEn);
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

                    _context.Companies.Add(model);
                    await _context.SaveChangesAsync();


                    return new
                    {
                        result = new
                        {

                        },
                        msg = "تم اضافة الشركة بنجاح"
                    };

                }
                else
                {
                    modelState.AddModelError("تداخل بيانات", "هذا الشركة موجوده من قبل");
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
                var x = _context.Companies.FirstOrDefault(r => r.Id == Id);
                if (x==null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الشركة");
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

        public async Task<dynamic> Edit(ModelStateDictionary modelState, Companies model)
        {
            try
            {

                var company = _context.Companies.FirstOrDefault(r => r.Id == model.Id);
                if (company == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذ الشركة");
                    return null;
                }

                if (model.Logofile != null )
                {
                    try
                    {
                        model.Logo = Files.SaveImage(model.Logofile, _environment);
                        company.Logo = model.Logo;
                    }
                    catch (Exception)
                    {


                    }
                }

                company.NameAr = model.NameAr;
                company.NameEn = model.NameEn;
                company.Active = model.Active;
               await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم تعديل الشركة بنجاح"
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
                var company = _context.Companies.FirstOrDefault(r => r.Id == Id);
                if (company == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الشركة");
                    return null;
                }


                 _context.Companies.Remove(company);
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف الشركة "
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError("تحذير", "يجب حذف كل البيانات المتعلقة بالشركة اولا");
                return null;
            }
        }




    }

}
