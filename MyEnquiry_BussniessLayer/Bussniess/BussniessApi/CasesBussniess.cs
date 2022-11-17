using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MyEnquiry.Helper;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface.InterfaceApi;
using MyEnquiry_BussniessLayer.ViewModels.Api;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess.BussniessApi
{
    public class CasesBussniess : ICases
    {
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public CasesBussniess(UserManager<ApplicationUser> userManager, IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _environment = environment;
        }

 
        public async Task<dynamic> ChangeStatus(ModelStateDictionary modelState, int Id, int status)
        {
            var getorder = _context.CasesOrders.FirstOrDefault(s => s.Id == Id);



            if (getorder == null)
            {
                modelState.AddModelError("غير موجود", "هذا الطلب غير موجود");
                return null;
            }


            var getcase = _context.Cases.FirstOrDefault(s => s.Id == getorder.CaseId);


            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "هذه الحالة غير موجوده");
                return null;
            }


            getcase.CaseStatusId = status;

            await _context.SaveChangesAsync();

            return new
            {
                result = new
                {

                },
                msg = "Successfully Message"
            };
        }



        public dynamic GetCaseStatus(ModelStateDictionary modelState)
        {


            var result = new List<object>();

          
           
            return new
            {
                result = new
                {
                    result
                },
                msg = "Successfully Message"
            };


        }
        public dynamic GetCaseForm(ModelStateDictionary modelState,int caseId)
        {



            var getcase = _context.Cases.FirstOrDefault(s => s.Id == caseId&&!s.Deleted);
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
            var result = checkhasforms.Select(s => new 
            {
                CaseId = caseId,
                questionId = s.Id,
                HasFile = s.HasFile,
                Question = s.Question,
                Qtype = s.choice,
                SelectedAnswerId = caseformanswers.FirstOrDefault(a => a.Answers.QuestionId == s.Id) != null ? caseformanswers.FirstOrDefault(a => a.Answers.QuestionId == s.Id).AnswerId : 0,
                SelectedAnswertext = caseformanswers.FirstOrDefault(a => a.Answers.QuestionId == s.Id) != null ? caseformanswers.FirstOrDefault(a => a.Answers.QuestionId == s.Id).Answer : "",
                answers = formanswers.Where(f => f.QuestionId == s.Id).Select(f => new  { Answer = f.Answer, AnswerId = f.Id }).ToList()


            }).ToList();
         
            return new
            {
                result = new
                {
                    result
                },
                msg = "Successfully Message"
            };


        }


        public async Task<dynamic> PostForm(ModelStateDictionary modelState,List<PostForm> model)
        {
           
            if (model.Count < 1)
            {
                modelState.AddModelError("غير موجودة", "لا يوجد بيانات");
                return null;
            }



            var getcase = _context.Cases.FirstOrDefault(s => s.Id == model.FirstOrDefault().CaseId && !s.Deleted);
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

                    answer.AnswerId = _context.Questions.Include(s=>s.Answers).FirstOrDefault(s=>s.Id==item.QuestionId).Answers.FirstOrDefault().Id;
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


    }
}
