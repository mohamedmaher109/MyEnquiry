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
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class CaseFormsBussniess : ICaseForms
    {
      
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public CaseFormsBussniess( IConfiguration configuration, MyAppContext context, IHostingEnvironment environment)
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

        public dynamic Get(ModelStateDictionary modelState)

        {
            var type = _context.CasesTypeForms.Include(s=>s.CaseType).Select(s => new CasesTypeForms
            {
                
                CaseType=s.CaseType,
                CaseTypeId=s.CaseTypeId
                

            }).ToList();
            return type;
        }
        



        public async Task<dynamic> Add(ModelStateDictionary modelState, string Form,int CaseTypeId)
        {
            try
            {

                var checktype = _context.Questions.FirstOrDefault(s => s.CaseTypeId == CaseTypeId);
                if (checktype != null)
                {
                    modelState.AddModelError("تداخل مطلوبة", "يوجد بالفعل استمارة لهذا النوع");
                    return null;
                }

                if (string.IsNullOrEmpty(Form))
                {
                    modelState.AddModelError("بيانات مطلوبة", "يجب ادخال جميع الاسئلة");
                    return null;
                }
                var routes_list = JsonConvert.DeserializeObject<List<FormObject>>(Form);


                if (routes_list == null)
                {
                    modelState.AddModelError("بيانات مطلوبة", "يجب ادخال جميع الاسئلة");
                    return null;
                }
                var caseform = new List<Questions>();
                

                foreach (var item in routes_list)
                {
                    var caseformanswers = new HashSet<Answers>();
                    var hasfile = false;
                    if (item.hasfile == "1" || item.hasfile == "true")
                    {
                        hasfile = true;
                    }
                    var ansewr = item.answer.Split("-");

                    if (!string.IsNullOrWhiteSpace(item.answer))
                    {
                        foreach (var itemansewr in ansewr)
                        {
                            caseformanswers.Add(new Answers
                            {
                                Answer = itemansewr,
                            });
                        }
                    }
                    else
                    {
                        caseformanswers.Add(new Answers
                        {
                            Answer = "",
                        });
                    }
                    caseform.Add(new Questions
                    {
                        CaseTypeId=CaseTypeId,
                        HasFile=hasfile,
                        Question=item.question,
                        Answers= caseformanswers
                    });
                    
                }

                _context.Questions.AddRange(caseform);
                await _context.SaveChangesAsync();
                return new
                {
                    result = new
                    {

                    },
                    msg = "تم اضافة الاستمارة بنجاح"
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
                var type = _context.Questions.Where(r => r.CaseTypeId == Id).ToList();
                if (type.Count< 1)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
                    return null;
                }

                var checkifconnected = _context.Cases.FirstOrDefault(s => s.CaseTypeId == type.FirstOrDefault().CaseTypeId);
                if (checkifconnected != null)
                {
                    modelState.AddModelError("غير مسموح", "لا يمكن حذف هذة الاستمارة لارتباطها بحالة");
                    return null;
                }

                 _context.Questions.RemoveRange(type);
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف الاستمارة "
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError("تحذير", "يجب حذف كل البيانات المتعلقة بالاستمارة اولا");
                return null;
            }
        }




    }

}
