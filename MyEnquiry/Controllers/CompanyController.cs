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
    public class CompanyController : Controller
    {
        private ICompany _company;


        public CompanyController(ICompany company)
        {
            _company = company;

        }
        public IActionResult CompanyIndex()
        {
            return View();
        }



        public IActionResult DisplayGrid()
        {

            var result = _company.Get(ModelState);
            return PartialView("_DisplayCompanies", result);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Companies model)
        {
            try
            {

                var result = await _company.Add(ModelState, model);

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




        public dynamic GetCompany(int Id)
        {
            try
            {

                var result = _company.GetById(ModelState, Id);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return PartialView("_EditCompany", result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Companies model)
        {
            try
            {

                var result = await _company.Edit(ModelState, model);

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

                var result = await _company.Delete(ModelState, Id);

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
