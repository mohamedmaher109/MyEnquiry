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
    public class BankCasesController : Controller
    {
        private IBankCases _bank;


        public BankCasesController(IBankCases bank)
        {
            _bank = bank;

        }
        public IActionResult BankCaseIndex()
        {
            ViewBag.Companies = _bank.GetCompanies(ModelState);
            ViewBag.CaseTypes = _bank.GetCaseTypes(ModelState);
            return View();
        }
        public IActionResult DisplayGrid()
        {
            var result = _bank.Get(ModelState,this.User);
            return PartialView("_DisplayBankCases", result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Cases model)
        {
            try
            {

                var result = await _bank.Add(ModelState, model,this.User);

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




        public dynamic GetbankCase(int Id)
        {
            try
            {

                var result = _bank.GetById(ModelState, Id);
                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }
                ViewBag.Companies = _bank.GetCompanies(ModelState);
                ViewBag.CaseTypes = _bank.GetCaseTypes(ModelState);

                return PartialView("_EditBankCase", result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cases model)
        {
            try
            {

                var result = await _bank.Edit(ModelState, model);

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

                var result = await _bank.Delete(ModelState, Id);

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
