using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using System;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
    [Authorize]
    public class DuesController : Controller
    {
        private IDues _Duies;


        public DuesController(IDues Duies)
        {
            _Duies = Duies;

        }
        public IActionResult BankIndex()
        {
            return View();
        }

        public IActionResult DisplayGrid()
        {

            var result = _Duies.GetBanks(ModelState,this.User);

            return PartialView("_DisplayBanks", result);
        }
        public IActionResult CompanyIndex()
        {
            return View();
        }

        public IActionResult DisplayGrid2()
        {
            var result = _Duies.GetCompany(ModelState,this.User);
            return PartialView("_DisplayBanks2", result);
        }
        public async Task<IActionResult> GetTotalCompany(int Id)
        {
            try
            {
                var result = await _Duies.GetTotal(ModelState, Id,this.User);

               
                return Json(result);
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }

        }
 
        public IActionResult CompIndex2(int Id)
        {
            ViewBag.id = Id;
            return View();
        }


        public IActionResult DisplayAllCompany2(int Id)
        {
            ViewBag.id = Id;

            var result = _Duies.GetById(ModelState, Id, this.User);

            return PartialView("_DisplayAllCompany", result);
        }
        [HttpPost]
        public IActionResult Export(int Id)
        {
            try
            {
                var result =  _Duies.ExportToExcel(ModelState, Id, this.User);


                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "تقرير.xlsx");
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }

        }

        /*  public async Task<IActionResult> GetGetCompany()
          {
              try
              {
                  var result = await _Duies.GetCompany(ModelState,this.User);


                  return Json(result);
              }
              catch (Exception ex)
              {
                  return CustomBadRequest.CustomExErrorResponse(ex);

              }*/

    }

}


