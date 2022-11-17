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
    public class SuperVisorCasesController : Controller
    {
        private ISuperVisorCases _case;
        private ISurvey _survey;


        public SuperVisorCasesController(ISuperVisorCases casemodel, ISurvey survey)
        {
            _survey = survey;
            _case = casemodel;

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

                var result = await _case.AcceptCase(ModelState, Id,this.User);

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



        public IActionResult GetFiles(int Id,int type)
        {

            var result = _case.GetFilesFromRep(ModelState, Id, type);

            return PartialView("_FilesFromRep", result);
        }


        [HttpGet]
        public async Task<IActionResult> GenerateResponseWordDocumentCase(int id)
        {

            try
            {
                var word = await _survey.GenerateResponseWordDocumentCase(id);
                if (word == null)
                {
                    return RedirectToAction("CasesIndex", "SuperVisorCases");

                }

                return File(word.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{id}.docx");
            }
            catch (Exception ex)
            {
                return RedirectToAction("CasesIndex", "SuperVisorCases");

            }
        }


        [HttpPost]
        public async Task<IActionResult> EditCaseForm(List<AnswerVm> model)
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

    }
}
