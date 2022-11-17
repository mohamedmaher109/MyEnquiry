using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class CitiesBusiness: ICity
    {
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public CitiesBusiness(IConfiguration configuration, MyAppContext context, IHostingEnvironment environment)
        {
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }

        public dynamic Get(ModelStateDictionary modelState)

        {
            var region = _context.Cities.Select(s => new Cities
            {
                Id = s.Id,
                NameAr = s.NameAr ?? "",
                NameEn = s.NameEn ?? "",
                Active = s.Active,
                CreatedAt = s.CreatedAt

            }).ToList();
            return region;
        }




        public async Task<dynamic> Add(ModelStateDictionary modelState, Cities model)
        {
            try
            {
                var x = _context.Cities.FirstOrDefault(b => b.NameAr == model.NameAr || b.NameEn == model.NameEn);
                if (x == null)
                {



                    _context.Cities.Add(model);
                    await _context.SaveChangesAsync();


                    return new
                    {
                        result = new
                        {

                        },
                        msg = "تم اضافة المحافظه بنجاح"
                    };

                }
                else
                {
                    modelState.AddModelError("تداخل بيانات", "هذا المحافظه موجوده من قبل");
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
                var x = _context.Cities.FirstOrDefault(r => r.Id == Id);
                if (x == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه المحافظه");
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

        public async Task<dynamic> Edit(ModelStateDictionary modelState, Cities model)
        {
            try
            {

                var region = _context.Cities.FirstOrDefault(r => r.Id == model.Id);
                if (region == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذ المحافظه");
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
                    msg = "تم تعديل المحافظه "
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
                var region = _context.Cities.FirstOrDefault(r => r.Id == Id);
                if (region == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه المحافظه");
                    return null;
                }
                _context.Cities.Remove(region);
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف المحافظه "
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError("تحذير", "يجب حذف كل البيانات المتعلقة بالمحافظه اولا");
                return null;
            }
        }

    }
}
