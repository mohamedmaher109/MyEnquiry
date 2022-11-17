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
    public class CaseTypesController : Controller
    {
        private ICaseTypes _type;


        public CaseTypesController(ICaseTypes type)
        {
            _type = type;

        }
        public IActionResult CaseTypesIndex()
        {
            return View();
        }



        public IActionResult DisplayGrid()
        {

            var result = _type.Get(ModelState);

            return PartialView("_DisplayCaseTypes", result);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CaseTypes model)
        {
            try
            {

                var result = await _type.Add(ModelState, model);

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




        public dynamic GetCaseTypes(int Id)
        {
            try
            {

                var result = _type.GetById(ModelState, Id);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return PartialView("_EditCaseType", result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CaseTypes model)
        {
            try
            {

                var result = await _type.Edit(ModelState, model);

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

                var result = await _type.Delete(ModelState, Id);

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
