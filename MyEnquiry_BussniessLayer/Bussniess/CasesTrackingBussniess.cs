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
using MyEnquiry_BussniessLayer.ViewModels.ResponseModels;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class CasesTrackingBussniess : ICasesTracking
    {

        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;
        public static IHostingEnvironment _environment2;

        public CasesTrackingBussniess(IHostingEnvironment environment2, IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _environment2 = environment2;
            _configuration = configuration;
            _context = context;
            _environment = environment;
        }


        public dynamic GetStatus(ModelStateDictionary modelState)
        {
            var status = _context.CaseStatus.Where(b => b.Id > (int)CaseEnumStatus.RecivedFromCompany &&( b.Id < (int)CaseEnumStatus.AcceptedFromReviewer || b.Id== (int)CaseEnumStatus.StartFromRepres))
                .Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

            return status;
        }


        public dynamic GetRepresentative(ModelStateDictionary modelState,int id)

        {
            var rep = _context.Users.Where(s => !s.Deleted && s.UserType == 4).Select(s => new RoleView
            {
                Id = s.Id,
                Name = s.FullName ?? ""

            }).ToList();
      
            return rep;
        }


        public dynamic GetAllCases(ModelStateDictionary modelState, ClaimsPrincipal user, int status)
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
                if (checkuser.Reviewr)
                {
                    var ReviewrSection = _context.Cases
                                  .Include(s => s.Bank)
                                  .Include(s => s.CaseStatus)
                                  .Include(s => s.Reviewer)
                                  .Include(s => s.CaseFiles)
                                  .Include(s => s.CaseType)
                                  .Include(s => s.CasesOrders)
                                  .Where(s => !s.Deleted && s.CompanyId == checkuser.CompanyId &&
                                  s.CaseStatusId > (int)CaseEnumStatus.RecivedFromCompany &&
                                 ( s.CaseStatusId < (int)CaseEnumStatus.AcceptedFromReviewer ||s.CaseStatusId ==(int)CaseEnumStatus.StartFromRepres) && s.ReviewerId == userId)
                                  .Select(s => new Cases
                                  {
                                      Id = s.Id,
                                      ClientNumbers = s.ClientNumbers,
                                      ClientName = s.ClientName,
                                      RejectResion = s.RejectResion,
                                      HomeAddress = s.HomeAddress,
                                      HomeGovernorate = s.HomeGovernorate,
                                      WorkAddress = s.WorkAddress,
                                      WorkGovernorate = s.WorkGovernorate,
                                      Reviewer = s.Reviewer,
                                      CasesOrders = s.CasesOrders,
                                      Bank = s.Bank,
                                      IsReiew = checkuser.Reviewr,
                                      CaseType = s.CaseType,
                                      CaseStatus = s.CaseStatus,
                                      FileToShow = s.CaseFiles.FirstOrDefault() != null ? s.CaseFiles.FirstOrDefault().ExcelSheet : "",
                                      CreatedAt = s.CreatedAt,
                                      FilesANswer = s.FilesANswer

                                  }).ToList();
                    return ReviewrSection;
                }
                var type = _context.Cases
                .Include(s => s.Bank)
                .Include(s => s.CaseStatus)
                .Include(s => s.Reviewer)
                .Include(s => s.CaseFiles)
                .Include(s => s.CaseType)
                .Include(s => s.CasesOrders)
                .Where(s => !s.Deleted && s.CompanyId == checkuser.CompanyId &&
                s.CaseStatusId > (int)CaseEnumStatus.RecivedFromCompany &&
               ( s.CaseStatusId < (int)CaseEnumStatus.AcceptedFromReviewer || s.CaseStatusId == (int)CaseEnumStatus.StartFromRepres) && s.SuperVisorId == userId)
                .Select(s => new Cases
                {
                    Id = s.Id,
                    ClientNumbers = s.ClientNumbers,
                    ClientName = s.ClientName,
                    RejectResion = s.RejectResion,
                    HomeAddress = s.HomeAddress,
                    HomeGovernorate = s.HomeGovernorate,
                    WorkAddress = s.WorkAddress,
                    WorkGovernorate = s.WorkGovernorate,
                    Reviewer = s.Reviewer,
                    CasesOrders = s.CasesOrders,
                    IsReiew = checkuser.Reviewr,
                    Bank = s.Bank,
                    CaseType = s.CaseType,
                    CaseStatus = s.CaseStatus,
                    FileToShow = s.CaseFiles.FirstOrDefault() != null ? s.CaseFiles.FirstOrDefault().ExcelSheet : "",
                    CreatedAt = s.CreatedAt,
                    FilesANswer = s.FilesANswer

                }).ToList();
                return type;
            }
            else
            {
                if (checkuser.Reviewr)
                {
                    var ReviewrSection = _context.Cases
                                  .Include(s => s.Bank)
                                  .Include(s => s.CaseStatus)
                                  .Include(s => s.Reviewer)
                                  .Include(s => s.CaseFiles)
                                  .Include(s => s.CaseType)
                                  .Include(s => s.CasesOrders)
                                  .Where(s => !s.Deleted && s.CompanyId == checkuser.CompanyId &&
                                  s.CaseStatusId == status &&
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
                                      IsReiew = checkuser.Reviewr,
                                      CaseType = s.CaseType,
                                      CaseStatus = s.CaseStatus,
                                      FileToShow = s.CaseFiles.FirstOrDefault() != null ? s.CaseFiles.FirstOrDefault().ExcelSheet : "",
                                      CreatedAt = s.CreatedAt,
                                      FilesANswer = s.FilesANswer

                                  }).ToList();
                    return ReviewrSection;
                }

                var type = _context.Cases
               .Include(s => s.Bank)
               .Include(s => s.CaseStatus)
               .Include(s => s.Reviewer)
               .Include(s => s.CaseFiles)
               .Include(s => s.CaseType)
               .Include(s => s.CasesOrders)
               .Where(s => !s.Deleted && s.CompanyId == checkuser.CompanyId &&
               s.CaseStatusId == status &&
               s.SuperVisorId == userId)
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
                   IsReiew = checkuser.Reviewr,
                   CasesOrders = s.CasesOrders,
                   Bank = s.Bank,
                   CaseType = s.CaseType,
                   CaseStatus = s.CaseStatus,
                   FileToShow = s.CaseFiles.FirstOrDefault() != null ? s.CaseFiles.FirstOrDefault().ExcelSheet : "",
                   CreatedAt = s.CreatedAt,
                   FilesANswer = s.FilesANswer

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

            var getcase = _context.Cases.Include(s => s.CasesOrders).FirstOrDefault(s => s.Id == Id);


            getcase.CasesOrders.Where(s => s.Status != 3).ToList().ForEach(s => s.Status = 3);

            getcase.CasesOrders.Add(new CasesOrders
            {
                UserId = UserId,
                Status = 1,

            });

            getcase.CaseStatusId = (int)CaseEnumStatus.AssignedToRecivers;

            await _context.SaveChangesAsync();
            var r = _context.SurveyFormResponses.Where(a => a.CaseId == Id).ToList();

            if (r != null && r.Count() >= 1)
            {

                if (r != null)
                {
                    _context.SurveyFormResponses.RemoveRange(r);
                    _context.SaveChanges();
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
        public async Task<dynamic> AcceptCase(ModelStateDictionary modelState, int Id)
        {



            var getcase = _context.Cases.Include(s => s.CasesOrders).FirstOrDefault(s => s.Id == Id);
            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "هذه الحالة غير موجوده");
                return null;
            }


            getcase.CaseStatusId = (int)CaseEnumStatus.AcceptedFromReviewer;

            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تم قبول الحالة وارسالها الى الجودة"
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

            var getacceptedorder = getcase.CasesOrders.FirstOrDefault(s => s.Status == 2);

            getacceptedorder.OrderFiles.Add(new OrderFiles
            {
                Type = 2,
                Url = url,
                FromType = 2,

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
                Message = Message,
                Rate = Rate,
                Type = 1,


            });


            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تم تقييم المندوب بنجاح"
            };
        }




        public dynamic GetFilesFromRep(ModelStateDictionary modelState, int Id)

        {
            var files = _context.OrderFiles.Include(s => s.Order).Where(s => s.Order.CaseId == Id && s.Order.Status == 2 && s.FromType == 1 && s.Type == 2).Select(s => new SelectView
            {

                Id = s.Id,
                Name = s.Url,


            }).ToList();
            return files;
        }



        public dynamic ReviewCase(ModelStateDictionary modelState, int Id)
        {

            var getcase = _context.Cases.FirstOrDefault(s => s.Id == Id);
            if (getcase == null)
            {
                modelState.AddModelError("غير موجودة", "هذة الحالة غير موجوده");
                return null;
            }
            var checkhasforms = _context.Questions.Include(s => s.Answers).Where(s => s.CaseTypeId == getcase.CaseTypeId).ToList();
            if (checkhasforms.Count < 1)
            {
                modelState.AddModelError("غير موجودة", "لا يوجد استمارة لهذه الحالة");
                return null;
            }

            var formanswers = _context.Answers.Include(s => s.Questions).Where(a => a.Questions.CaseTypeId == getcase.CaseTypeId).ToList();
            var caseformanswers = _context.CaseFormAnswers.Where(s => formanswers.Select(a => a.Id).Contains(s.AnswerId)).ToList();
            var result1 = checkhasforms.Select(s => new AnswerVm
            {
                CaseId = Id,
                FormId = s.Id,
                HasFile = s.HasFile,
                Question = s.Question,
                Qtype = s.choice,
                SelectedAnswerId = caseformanswers.FirstOrDefault(a => a.Answers.QuestionId == s.Id) != null ? caseformanswers.FirstOrDefault(a => a.Answers.QuestionId == s.Id).AnswerId : null,
                SelectedAnswertext = caseformanswers.FirstOrDefault(a => a.Answers.QuestionId == s.Id) != null ? caseformanswers.FirstOrDefault(a => a.Answers.QuestionId == s.Id).Answer : "",
                formanswers = formanswers.Where(f => f.QuestionId == s.Id).Select(f => new formanswers { Answer = f.Answer, AnswerId = f.Id }).ToList()


            }).ToList();
            //var result = checkhasforms.Select(s => new AnswerVm
            //{
            //    CaseId = Id,
            //    FormId = s.Id,
            //    HasFile = s.HasFile,
            //    Question = s.Question,

            //    FromAnswer = formanswers.FirstOrDefault(a => a.QuestionId == s.Id) != null ? formanswers.FirstOrDefault(a => a.QuestionId == s.Id).Answer : "",



            //}).ToList();


            return result1;
        }



        //public dynamic ReviewCase1(ModelStateDictionary modelState, int Id)
        //{

        //    var getcase = _context.Cases.FirstOrDefault(s => s.Id == Id);
        //    if (getcase == null)
        //    {
        //        modelState.AddModelError("غير موجودة", "هذة الحالة غير موجوده");
        //        return null;
        //    }
        //    var checkhasforms = _context.CasesTypeForms.Where(s => s.CaseTypeId == getcase.CaseTypeId).ToList();
        //    if (checkhasforms.Count < 1)
        //    {
        //        modelState.AddModelError("غير موجودة", "لا يوجد استمارة لهذه الحالة");
        //        return null;
        //    }

        //    var formanswers = _context.CaseFormAnswers.Where(a => a.CasesTypeForms.CaseTypeId == getcase.CaseTypeId).ToList();

        //    var result1 = checkhasforms.Select(s => new AnswerVm
        //    {
        //        CaseId=Id,
        //        FormId=s.Id,
        //        HasFile=s.HasFile,
        //        Question=s.Question,
        //        //Qtype=s.CaseFormAnswers.FirstOrDefault().Type,
        //        formanswers= formanswers.Where(f=>f.FormId==s.Id).Select(f=>new formanswers {Answer=f.Answer,AnswerId=f.Id }).ToList()


        //    }).ToList();
        //    var result = checkhasforms.Select(s => new AnswerVm
        //    {
        //        CaseId=Id,
        //        FormId = s.Id,
        //        HasFile = s.HasFile,
        //        Question = s.Question,

        //        FromAnswer= formanswers.FirstOrDefault(a=>a.FormId==s.Id)!=null? formanswers.FirstOrDefault(a => a.FormId == s.Id).Answer:"",



        //    }).ToList();


        //    return result1;
        //}




        public dynamic UploadAnswerForBank(ModelStateDictionary modelState, UploadfileAnswer model)
        {
            if (model.file == null || model.id == null|| model.id == 0)
            {
                modelState.AddModelError("غير موجودة", "لا يوجد بيانات");
                return null;
            }
            var Case = _context.Cases.Where(a => !a.Deleted&&a.Id== model.id).FirstOrDefault();
            Case.FilesANswer = Files.SavePdf(model.file, _environment2);
            _context.Cases.Update(Case);
            _context.SaveChanges();
            return new
            {
                result = new
                {

                },
                msg = "تم رفع الرد بنجاح"
            };

        }
        public async Task<dynamic> EditCaseForm(ModelStateDictionary modelState, List<AnswerVm> model)
        {

            if (model == null)
            {
                modelState.AddModelError("غير موجودة", "لا يوجد بيانات");
                return null;
            }

            if (model.Count < 1)
            {
                modelState.AddModelError("غير موجودة", "لا يوجد بيانات");
                return null;
            }



            var getcase = _context.Cases.FirstOrDefault(s => s.Id == model.FirstOrDefault().CaseId);
            if (getcase == null)
            {
                modelState.AddModelError("غير موجودة", "هذة الحالة غير موجوده");
                return null;
            }


            var answers = new List<CaseFormAnswers>();

            var getoldanswers = _context.CaseFormAnswers.Include(s => s.FormAnswersFiles).Where(s => s.CaseId == model.FirstOrDefault().CaseId).ToList();
            //var getoldFilesanswers = _context.FormAnswersFiles.Where(s => getoldanswers.Select(a=>a.Id).Contains(s.CaseFormAnsweId)).ToList();
            _context.CaseFormAnswers.RemoveRange(getoldanswers);

            foreach (var item in model)
            {
                var answer = new CaseFormAnswers();
                var filesmodel = new FormAnswersFiles();
              
                if (item.SelectedAnswerId != null)
                {


                    answer.AnswerId = (int)item.SelectedAnswerId;
                    answer.CaseId = item.CaseId;


                }
                else
                {

                    answer.AnswerId = item.formanswers.FirstOrDefault().AnswerId;
                    answer.Answer = item.SelectedAnswertext;
                        answer.CaseId = item.CaseId;
                }
                if (item.FileUpload != null)
                {
                    var fileurl = Files.SaveImage(item.FileUpload, _environment);

                    filesmodel.Url = fileurl;
                    answer.FormAnswersFiles.Add(filesmodel);
                }

                answers.Add(answer);
            }
            _context.CaseFormAnswers.AddRange(answers);
            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تم مراجعة الحالة"
            };
        }






        public dynamic GetFormFile(ModelStateDictionary modelState, int Id)

        {
            var getanswer = _context.CaseFormAnswers.Include(s=>s.Answers).FirstOrDefault(s => s.Answers.QuestionId == Id);
            if (getanswer == null)
            {
                modelState.AddModelError("غير موجودة", "لم يتم الاجابة على السؤال بعد");
                return null;
            }

            var getanswerfile = _context.FormAnswersFiles.Where(s => s.CaseFormAnsweId == getanswer.Id).ToList();
            if (getanswerfile == null)
            {
                modelState.AddModelError("غير موجودة", "لم يتم اضافة مرفق بعد");
                return null;
            }
            var files = new List<SelectView>();
            foreach (var item in getanswerfile)
            {
                files.Add(new SelectView { 
                
                Name=item.Url,
                });
            }
            
            

            return files;
        }

        public List<SurveyFormResponse> GetUserResponse(string userId, Guid formId, int? companyId,int id)
        {
            var query = _context.SurveyFormResponses.Where(r => r.UserId == userId && r.SurveyForm.LinkIdentifier == formId);
            if (companyId.HasValue)
            {
                query = query.Where(r => r.CompanyId == companyId);
            }

            var response = query.ToList().GroupBy(r => r.GroupId).FirstOrDefault()?.Select(r => r).ToList();
            return response;
        }
        public dynamic Userlocation(ModelStateDictionary modelState, int Id)
        {
            var location = new latAndlng();

            var getaccepted = _context.CasesOrders.Include(s=>s.User).FirstOrDefault(s => s.CaseId == Id && s.Status == 2);

            if (getaccepted == null)
            {
                modelState.AddModelError("غير موجودة", "لم يتم قبول الحالة من قبل المندوب بعد");
                return null;
            }

            var getuser = getaccepted.User;
            location.lat = getuser.lat!=null?(decimal)getuser.lat : 0;
            location.lng = getuser.lng!=null?(decimal)getuser.lng:0;
            return location;
        }

    }

}
