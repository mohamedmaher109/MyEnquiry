using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
    [Authorize]
    public class CasesController : Controller
    {
        private ICases _case;


        public CasesController(ICases casemodel)
        {
            _case = casemodel;

        }
        public IActionResult CasesFromBankIndex()
        {
            ViewBag.Banks = _case.GetBanks(ModelState);
            return View();
        }
        public IActionResult DisplayCasesFromBank(int BankId)
        {
            var result = _case.CasesFromBank(ModelState, BankId,this.User);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_DisplayCasesFromBank", result);
        }
        public async Task<IActionResult> Recive(int Id,int type)
        {
            try
            {

                var result = await _case.ReciveCase(ModelState, Id, type);

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


        public async Task<IActionResult> deleteAllCases(int Id)
        {
            try
            {
                var result = await _case.deleteAllCases(ModelState, Id);

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
        

        public IActionResult OpenRefuseModal()
        {
            ViewBag.Banks = _case.GetBanks(ModelState);

            return PartialView("_RefuseFile");
        }



        public async Task<IActionResult> UploadFile(int Id,IFormFile file)
        {
            try
            {

                var result = await _case.UploadFile(ModelState, Id,file,this.User);

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
        
        public async Task<IActionResult> RefusedFile(int Bank,IFormFile file)
        {
            try
            {

                var result = await _case.RefusedFile(ModelState, Bank, file);

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



        [HttpGet("download")]
        public IActionResult GetBlobDownload([FromQuery] string link)
        {
            var net = new System.Net.WebClient();
            var data = net.DownloadData(link);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "something.bin";
            return File(content, contentType, fileName);
        }


    }
}
