using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
    [Authorize]
    public class CasesTrackingController : Controller
    {
        private ICasesTracking _case;
        private ISurvey _survey;

        public CasesTrackingController(ICasesTracking casemodel, ISurvey survey)
        {
            _case = casemodel;
            _survey = survey;
        }
        public IActionResult CasesIndex()
        {
            ViewBag.Status = _case.GetStatus(ModelState);


            return View();
        }



        public IActionResult DisplayCases(int Status=0)
        {
            var result = _case.GetAllCases(ModelState, this.User, Status);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_DisplayCases", result);
        }


        public IActionResult OpenRepresentativeModal(int Id)
        {
            ViewBag.CaseId = Id;
            ViewBag.Users = _case.GetRepresentative(ModelState,Id);

            return PartialView("_SendCase");
        }


        public async Task<IActionResult> SendCase(int Id, string UserId)
        {
            try
            {

                var result = await _case.SendCase(ModelState, Id, UserId);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return Json(result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }


        }

        

        public async Task<IActionResult> AcceptCase(int Id)
        {
            try
            {

                var result = await _case.AcceptCase(ModelState, Id);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return Json(result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }


        }




        public IActionResult RateModal(int Id)
        {
            ViewBag.CaseId = Id;

            return PartialView("_RateUser");
        }








        public async Task<IActionResult> SendRate(int Id, int Rate,string Message)
        {
            try
            {

                var result = await _case.SendRate(ModelState, Id, Rate, Message);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return Json(result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }


        }




        public IActionResult OpenUploadModal(int Id)
        {
            ViewBag.CaseId = Id;

            return PartialView("_UploadFile");
        }



        public async Task<IActionResult> UploadFile(int Id, IFormFile file)
        {
            try
            {

                var result = await _case.UploadFile(ModelState, Id, file);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return Json(result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }


        }



        public IActionResult GetFiles(int Id)
        {

            var result = _case.GetFilesFromRep(ModelState, Id);

            return PartialView("_FilesFromRep", result);
        }
        
        public IActionResult ReviewCaseModal(int Id)
        {

            var result = _case.ReviewCase(ModelState, Id);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_CaseRevesion", result);
        }

        public IActionResult UploadAnswerForBank(int id)
        {
            ViewBag.POt = id;
            return PartialView("_FileBank");

        }  
        [HttpPost]
        public IActionResult UploadAnswerForBankPost(UploadfileAnswer model)
        {
            var result = _case.UploadAnswerForBank(ModelState, model);

            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }

            return Json(result);

        }
        [HttpPost]
        public async Task<IActionResult> EditCaseForm( List<AnswerVm> model)
        {
            try
            {

                var result = await _case.EditCaseForm(ModelState, model);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }


        }
        public IActionResult GetFormFile(int Id)
        {

            var result = _case.GetFormFile(ModelState, Id);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_FileForm", result);
        }

        public IActionResult Userlocation(int Id)
        {

            var result = _case.Userlocation(ModelState, Id);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_Location", result);
        }

        [HttpGet]
        public async Task<IActionResult>  GenerateResponseWordDocumentCase(int id)
        {

            try
            { var word = await _survey.GenerateResponseWordDocumentCase(id);
                if (word == null)
                {
                    return RedirectToAction("CasesIndex", "CasesTracking");

                }

                return File(word.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{id}.docx");
            }
            catch (Exception ex)
            {
                return RedirectToAction("CasesIndex", "CasesTracking");

            }
        }


    }
}
