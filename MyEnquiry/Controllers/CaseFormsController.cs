using Microsoft.AspNetCore.Authorization;
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
    public class CaseFormsController : Controller
    {
        private ICaseForms _form;


        public CaseFormsController(ICaseForms form)
        {
            _form = form;

        }
        public IActionResult CaseFormsIndex()
        {
            ViewBag.CaseTypes = _form.GetCaseTypes(ModelState);
            return View();
        }



        public IActionResult DisplayGrid()
        {

            var result = _form.Get(ModelState);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_DisplayCaseForms", result);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string Form, int CaseTypeId)
        {
            try
            {

                var result = await _form.Add(ModelState, Form, CaseTypeId);

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





        public async Task<IActionResult> Delete(int Id)
        {
            try
            {

                var result = await _form.Delete(ModelState, Id);

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
