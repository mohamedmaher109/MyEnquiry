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
    public class DoneCasesController : Controller
    {
        private ICases _case;


        public DoneCasesController(ICases casemodel)
        {
            _case = casemodel;

        }
        public IActionResult Index()
        {

            return View();
        }



        public IActionResult Display()
        {

            var result = _case.GetDoneCases(ModelState,this.User);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_Display", result);
        }


        public async Task<IActionResult> GetPDF(int Id)
        {
            try
            {

                var result = await _case.GetPDF(ModelState, Id);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }

                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, "Case"+Id + ".pdf");

                


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }


        }
        public async Task<IActionResult> ChangeStatus(int Id,int type, string reson="")
        {
            try
            {

                var result = await _case.ChangeStatus(ModelState, Id,type, reson);

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




        public IActionResult OpenRejectModal(int Id)
        {
            ViewBag.Id = Id;

            return PartialView("_RejectCase");
        }



    }
}
