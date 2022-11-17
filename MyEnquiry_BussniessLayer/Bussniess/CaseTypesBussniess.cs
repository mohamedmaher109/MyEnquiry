using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
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
    public class CaseTypesBussniess : ICaseTypes
    {
      
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public CaseTypesBussniess( IConfiguration configuration, MyAppContext context, IHostingEnvironment environment)
        {
            _configuration = configuration;
            
            _context = context;
            _environment = environment;
        }

        public dynamic Get(ModelStateDictionary modelState)

        {
            var type = _context.CaseTypes.Select(s => new CaseTypes
            {
                Id = s.Id,
                NameAr = s.NameAr ?? "",
                NameEn = s.NameEn ?? "",
                Active=s.Active,
                Word=s.Word,
                CreatedAt=s.CreatedAt

            }).ToList();
            return type;
        }
        



        public async Task<dynamic> Add(ModelStateDictionary modelState, CaseTypes model)
        {
            try
            {
                var x = _context.CaseTypes.FirstOrDefault(b => b.NameAr == model.NameAr|| b.NameEn == model.NameEn);
                if (x==null)
                {
                    
                    try
                    {
                        model.Word = Files.SaveWord(model.Wordfile, _environment);
                    }
                    catch (Exception)
                    {

                        
                    }
                    // first we create Admin rool    

                    _context.CaseTypes.Add(model);
                    await _context.SaveChangesAsync();


                    return new
                    {
                        result = new
                        {

                        },
                        msg = "تم اضافة الحالة بنجاح"
                    };

                }
                else
                {
                    modelState.AddModelError("تداخل بيانات", "هذه الحالة موجوده من قبل");
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
                var x = _context.CaseTypes.FirstOrDefault(r => r.Id == Id);
                if (x==null)
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

        public async Task<dynamic> Edit(ModelStateDictionary modelState, CaseTypes model)
        {
            try
            {

                var type = _context.CaseTypes.FirstOrDefault(r => r.Id == model.Id);
                if (type == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
                    return null;
                }

                if (model.Wordfile != null )
                {
                    try
                    {
                        model.Word = Files.SaveWord(model.Wordfile, _environment);
                        type.Word = model.Word;
                    }
                    catch (Exception)
                    {


                    }
                }

                type.NameAr = model.NameAr;
                type.NameEn = model.NameEn;
                type.Active = model.Active;
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

        public async Task<dynamic>  Delete(ModelStateDictionary modelState, int Id)
        {
            try
            {
                var type = _context.CaseTypes.FirstOrDefault(r => r.Id == Id);
                if (type == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
                    return null;
                }


                 _context.CaseTypes.Remove(type);
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف الحالة "
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
