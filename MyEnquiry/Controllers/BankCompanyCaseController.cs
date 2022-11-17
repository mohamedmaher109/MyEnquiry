using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
    public class BankCompanyCaseController : Controller
    {

        private IBankCompanyCase _bank;
        private IBankCases _bankq;


        public BankCompanyCaseController(IBankCompanyCase bank, IBankCases bankq)
        {
            _bank = bank;
            _bankq = bankq;
        }
        public IActionResult Index()
        {
            return View();
        }
  
        public IActionResult DisplayGrid()
        {

            var result = _bank.Get(ModelState, this.User);

            return PartialView("_DisplayBanks", result);
        }
        public IActionResult IndexBanc(int id)
        {
            ViewBag.id = id;
            ViewBag.Companies = _bank.GetCities(ModelState);

            ViewBag.CaseTypes = _bank.GetCaseTypes(ModelState);
            return View();
        }

        public IActionResult DisplayGridBanc(int id)
        {
            ViewBag.id = Convert.ToInt32(id);
            var result = _bank.GetBankCompany(ModelState, this.User, id);

            return PartialView("_DisplayBanks3", result);
        }

        public IActionResult CompanyIndex()
        {
            return View();
        }
  
        public IActionResult DisplayGridCompany()
        {

            var result = _bank.GetCompany(ModelState, this.User);

            return PartialView("_DisplayCompanyIndex", result);
        }  
        public IActionResult IndexBancCompany(int id)
        {
            ViewBag.id = id;
            ViewBag.Companies = _bank.GetCities(ModelState);
     
            ViewBag.CaseTypes = _bank.GetCaseTypes(ModelState);
            return View();
        }
  
        public IActionResult DisplayGridBancCompany(int id)
        {
            ViewBag.id = Convert.ToInt32(id);
           var result = _bank.GetAllCompany(ModelState, this.User,id);

            return PartialView("_DisplayBanksCompany3", result);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BankCompanyVmAdd model)
        {
            try
            {

                var result = await _bank.Add(ModelState, model, this.User);

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
        [HttpGet]
        public dynamic GetbankCase(int Id)
        {
            try
            {

                var result = _bank.GetById(ModelState, Id);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }
                ViewBag.Companies = _bank.GetCities(ModelState);
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
        public async Task<IActionResult> Edit(BankCompanyVmAdd model)
        {
            try
            {

                var result = await _bank.Edit(ModelState, model,this.User);

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
