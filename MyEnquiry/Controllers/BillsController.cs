using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using System;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
    public class BillsController : Controller
    {
        private IBills _bill;


        public BillsController(IBills bill)
        {
            _bill = bill;

        }
        public IActionResult Index()
        {
            return View();
        }
   
        public IActionResult DisplayGrid()
        {

            var result = _bill.Get(ModelState, this.User);

            return PartialView("_DisplayBanks", result);
        }  
        [HttpGet]
      
        public IActionResult Index2(int Id)
        {
            ViewBag.id = Id;
            var result = _bill.GetById(ModelState, Id, this.User);

            return View(result);
        }
        [HttpGet]
     
        public IActionResult DisplayGrid2(int Id)
        {
            ViewBag.id = Id;

            var result = _bill.GetById(ModelState, Id, this.User);

            return PartialView("_DisplayBills", result);
        }

        public async Task<IActionResult> Pay(int Id,int date)
        {
            try
            {

                var result = await _bill.Pay(ModelState, Id, date, this.User);

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
