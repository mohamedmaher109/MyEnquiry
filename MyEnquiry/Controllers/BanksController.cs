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
    public class BanksController : Controller
    {
        private IBanks _bank;


        public BanksController(IBanks bank)
        {
            _bank = bank;

        }
        public IActionResult BankIndex()
        {
            return View();
        }



        public IActionResult DisplayGrid()
        {

            var result = _bank.Get(ModelState);

            return PartialView("_DisplayBanks", result);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Banks model)
        {
            try
            {

                var result = await _bank.Add(ModelState, model);

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




        public dynamic Getbank(int Id)
        {
            try
            {

                var result = _bank.GetById(ModelState, Id);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return PartialView("_EditBank", result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Banks model)
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
