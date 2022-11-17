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
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MyEnquiry.Helper;
using OfficeOpenXml;
using MyEnquiry_BussniessLayer.ViewModels;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class SuperVisorCasesBussniess : ISuperVisorCases
    {
      
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public SuperVisorCasesBussniess( IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            
            _context = context;
            _environment = environment;
        }


        public dynamic GetStatus(ModelStateDictionary modelState)
        {
            var status = _context.CaseStatus.Where(b => b.Id>=(int)CaseEnumStatus.AcceptedFromReviewer && b.Id < (int)CaseEnumStatus.CaseDone)
                .Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

            return status;
        }


        public dynamic GetRepresentative(ModelStateDictionary modelState,int id)

        {
            var rep = _context.Users.Where(s=>!s.Deleted&&s.UserType==4).Select(s => new RoleView
            {
                Id = s.Id,
                Name = s.FullName ?? ""

            }).ToList();
       
            return rep;
        }


        public dynamic GetAllCases(ModelStateDictionary modelState, ClaimsPrincipal user,int status)
        {

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);


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
            if (status == 0)
            {
                var type = _context.Cases
                .Include(s => s.Bank)
                .Include(s => s.CaseStatus)
                .Include(s => s.Reviewer)
                .Include(s => s.CaseFiles)
                .Include(s => s.CaseType)
                .Include(s => s.CasesOrders)
                .Where(s => !s.Deleted && s.CompanyId == checkuser.CompanyId && 
                s.CaseStatusId >= (int)CaseEnumStatus.AcceptedFromReviewer
                && s.CaseStatusId < (int)CaseEnumStatus.CaseDone)
                .Select(s => new Cases
                {
                    Id = s.Id,
                    ClientNumbers = s.ClientNumbers,
                    ClientName = s.ClientName,
                    EnquirerName = s.EnquirerName,
                    HomeAddress = s.HomeAddress,
                    HomeGovernorate = s.HomeGovernorate,
                    WorkAddress = s.WorkAddress,
                    WorkGovernorate = s.WorkGovernorate,
                    Reviewer = s.Reviewer,
                    CasesOrders = s.CasesOrders,
                    Bank = s.Bank,
                    CaseType = s.CaseType,
                    CaseStatus = s.CaseStatus,
                    FileToShow = s.CaseFiles.FirstOrDefault() != null ? s.CaseFiles.FirstOrDefault().ExcelSheet : "",
                    CreatedAt = s.CreatedAt

                }).ToList();
                return type;
            }
            else
            {
                var type = _context.Cases
               .Include(s => s.Bank)
               .Include(s => s.CaseStatus)
               .Include(s => s.Reviewer)
               .Include(s => s.CaseFiles)
               .Include(s => s.CaseType)
               .Include(s => s.CasesOrders)
               .Where(s => !s.Deleted && s.CompanyId == checkuser.CompanyId &&
               s.CaseStatusId ==status &&
               s.ReviewerId == userId)
               .Select(s => new Cases
               {
                   Id = s.Id,
                   ClientNumbers = s.ClientNumbers,
                   ClientName = s.ClientName,
                   EnquirerName = s.EnquirerName,
                   HomeAddress = s.HomeAddress,
                   HomeGovernorate = s.HomeGovernorate,
                   WorkAddress = s.WorkAddress,
                   WorkGovernorate = s.WorkGovernorate,
                   Reviewer = s.Reviewer,
                   CasesOrders = s.CasesOrders,
                   Bank = s.Bank,
                   CaseType = s.CaseType,
                   CaseStatus = s.CaseStatus,
                   FileToShow = s.CaseFiles.FirstOrDefault() != null ? s.CaseFiles.FirstOrDefault().ExcelSheet : "",
                   CreatedAt = s.CreatedAt

               }).ToList();
                return type;
            }
            
        }

        public async Task<dynamic> SendCase(ModelStateDictionary modelState, int Id, string UserId)
        {

            var checkuser = _context.Users.FirstOrDefault(u => u.Id == UserId);

            if (checkuser == null)
            {
                modelState.AddModelError("غير موجود", "هذا المندوب غير موجود");
                return null;
            }

            var checkhasmaxorders = _context.CasesOrders.Where(s => s.UserId == UserId && s.Status == 2).ToList();

            if (checkhasmaxorders.Count >= 6)
            {
                modelState.AddModelError("المندوب", "لا يمكن ارسال الى المندوب فى الوقت الحالى لوصوله الى الحد الاقصى من الحالات");
                return null;
            }

            var getcase = _context.Cases.Include(s=>s.CasesOrders).FirstOrDefault(s => s.Id == Id);


            getcase.CasesOrders.Where(s => s.Status != 3).ToList().ForEach(s=>s.Status=3);

            getcase.CasesOrders.Add(new CasesOrders
            {
                UserId=UserId,
                Status=1,

            });

            getcase.CaseStatusId = (int)CaseEnumStatus.AssignedToRecivers;

            await _context.SaveChangesAsync();
            var r = _context.SurveyFormResponses.Where(a => a.CaseId == Id).ToList();

            if (r != null && r.Count() >= 1)
            {

                if (r != null)
                {
                    _context.SurveyFormResponses.RemoveRange(r);
                    await _context.SaveChangesAsync();
                }
            }
            return new
            {
                result = new
                {

                },
                msg = "تم ارسال الحالة الى المندوب"
            };

        }
        public async Task<dynamic> AcceptCase(ModelStateDictionary modelState, int Id, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);


            var getcase = _context.Cases.Include(s=>s.CasesOrders).FirstOrDefault(s => s.Id == Id);
            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "هذه الحالة غير موجوده");
                return null;
            }

            getcase.SuperVisorId = userId;
            getcase.CaseStatusId = (int)CaseEnumStatus.CaseDone;

            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تم قبول الحالة وارسالها الى البنك"
            };

        }

        public async Task<dynamic> UploadFile(ModelStateDictionary modelState, int Id, IFormFile file)
        {
            var getcase = _context.Cases.Include(s => s.CasesOrders).FirstOrDefault(s => s.Id == Id);
            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "هذه الحالة غير موجوده");
                return null;
            }

            var url = "";
            try
            {
                 url = Files.SaveImage(file, _environment);
            }
            catch (Exception)
            {
                modelState.AddModelError("تحذير", "يرجى اختيار صورة");
                return null;
            }
            
            var getacceptedorder=getcase.CasesOrders.FirstOrDefault(s => s.Status == 2);

            getacceptedorder.OrderFiles.Add(new OrderFiles
            {
                Type = 2,
                Url = url,
                FromType=2,
                
            });


            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تم ارفاق الصورة"
            };
        }

        public async Task<dynamic> SendRate(ModelStateDictionary modelState, int Id, int Rate, string Message)
        {

            var getorder = _context.CasesOrders.FirstOrDefault(s => s.CaseId == Id && s.Status == 2);

            if (getorder == null)
            {
                modelState.AddModelError("لم تقبل", "لم يتم قبول الحالة من قبل المندوب");
                return null;
            }

            getorder.OrderReview.Add(new OrderReview
            {
                Message=Message,
                Rate=Rate,
                Type=2,


            });


            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تم تقييم المراجع بنجاح"
            };
        }




        public dynamic GetFilesFromRep(ModelStateDictionary modelState, int Id,int type)

        {
            var files = _context.OrderFiles.Include(s=>s.Order).Where(s => s.Order.CaseId == Id&&s.Order.Status==2&&s.FromType== type && s.Type== 2).Select(s => new SelectView
            {

                Id = s.Id,
                Name = s.Url,


            }).ToList();
            return files;
        }


        public async Task<dynamic> EditCaseForm(ModelStateDictionary modelState, List<AnswerVm> model)
        {

            //if (model == null)
            //{
            //    modelState.AddModelError("غير موجودة", "لا يوجد بيانات");
            //    return null;
            //}

            //if (model.Count < 1)
            //{
            //    modelState.AddModelError("غير موجودة", "لا يوجد بيانات");
            //    return null;
            //}



            //var getcase = _context.Cases.FirstOrDefault(s => s.Id == model.FirstOrDefault().CaseId);
            //if (getcase == null)
            //{
            //    modelState.AddModelError("غير موجودة", "هذة الحالة غير موجوده");
            //    return null;
            //}


            //var answers = new List<CaseFormAnswers>();

            //var getoldanswers = _context.CaseFormAnswers.Include(s => s.FormAnswersFiles).Where(s => s.CaseId == model.FirstOrDefault().CaseId).ToList();
            ////var getoldFilesanswers = _context.FormAnswersFiles.Where(s => getoldanswers.Select(a=>a.Id).Contains(s.CaseFormAnsweId)).ToList();
            ////_context.CaseFormAnswers.RemoveRange(getoldanswers);

            //foreach (var item in model)
            //{
            //    var toedit = getoldanswers.FirstOrDefault(s => s.FormId == item.FormId);
            //    var filesmodel = new FormAnswersFiles();
            //    if (item.FileUpload != null)
            //    {
            //        var fileurl = Files.SaveImage(item.FileUpload, _environment);
            //        filesmodel.Url = fileurl;
            //    }
            //    if (toedit != null)
            //    {
            //        toedit.Answer = item.FromAnswer;
            //        toedit.FormAnswersFiles.Add(filesmodel);
            //    }
            //    else
            //    {
            //        var answer = new CaseFormAnswers();



            //        answer.CaseId = item.CaseId;
            //        answer.Answer = item.FromAnswer;
            //        answer.FormId = item.FormId;
            //        answer.FormAnswersFiles.Add(filesmodel);
            //        answers.Add(answer);
            //    }

            //}
            //_context.CaseFormAnswers.AddRange(answers);
            //await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تم مراجعة الحالة"
            };
        }




    }

}
