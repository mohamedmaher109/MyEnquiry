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
    public class RefusedCasesController : Controller
    {
        private ICases _case;


        public RefusedCasesController(ICases casemodel)
        {
            _case = casemodel;

        }
        public IActionResult Index()
        {

            return View();
        }



        public IActionResult Display()
        {

            var result = _case.GetRefusedCases(ModelState,this.User);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_Display", result);
        }




        public async Task<IActionResult> Solved(int Id)
        {
            try
            {

                var result = await _case.Solved(ModelState, Id);

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
