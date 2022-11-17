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
    public class RegionBussniess : IRegion
    {
      
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public RegionBussniess( IConfiguration configuration, MyAppContext context, IHostingEnvironment environment)
        {
            _configuration = configuration;
            
            _context = context;
            _environment = environment;
        }

        public dynamic Get(ModelStateDictionary modelState)

        {
            var region = _context.Regions.Include(a=>a.Cities) .ToList();
            return region;
        }
        



        public async Task<dynamic> Add(ModelStateDictionary modelState, Regions model)
        {
            try
            {
                var x = _context.Regions.FirstOrDefault(b => b.NameAr == model.NameAr|| b.NameEn == model.NameEn);
                if (x==null)
                {
                    _context.Regions.Add(model);
                    await _context.SaveChangesAsync();


                    return new
                    {
                        result = new
                        {

                        },
                        msg = "تم اضافة المنطقة بنجاح"
                    };

                }
                else
                {
                    modelState.AddModelError("تداخل بيانات", "هذا المنطقة موجوده من قبل");
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
                var x = _context.Regions.FirstOrDefault(r => r.Id == Id);
                if (x==null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه المنطقة");
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

        public async Task<dynamic> Edit(ModelStateDictionary modelState, Regions model)
        {
            try
            {

                var region = _context.Regions.FirstOrDefault(r => r.Id == model.Id);
                if (region == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذ المنطقة");
                    return null;
                }


                region.NameAr = model.NameAr;
                region.NameEn = model.NameEn;
                region.Active = model.Active;
               await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم تعديل الشركة المنطقة"
                };

            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }
        public dynamic GetCitiies(ModelStateDictionary modelState)
        {
            var companies = _context.Cities.Where(b => !b.Deleted).Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

            return companies;
        }

        public async Task<dynamic>  Delete(ModelStateDictionary modelState, int Id)
        {
            try
            {
                var region = _context.Regions.FirstOrDefault(r => r.Id == Id);
                if (region == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه المنطقة");
                    return null;
                }


                 _context.Regions.Remove(region);
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف المنطقة "
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError("تحذير", "يجب حذف كل البيانات المتعلقة بالمنطقة اولا");
                return null;
            }
        }




    }

}
