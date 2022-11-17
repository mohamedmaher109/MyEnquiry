using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModel;
using MyEnquiry_BussniessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
    [Authorize]
    public class RepresentativeController : Controller
    {


        private IRepresentative _rep;


        public RepresentativeController(IRepresentative rep)
        {
            _rep = rep;

        }

        public IActionResult Index()
        {
            
            return View();
        }
        
        public IActionResult BlockedIndex()
        {
            
            return View();
        }

        public IActionResult DeletededIndex()
        {
            var result = _rep.GetDeleted(ModelState);
            return View(result);
        }
        public IActionResult BackFromDeleted(string id)
        {
            var r = _rep.BackFromDeleted(ModelState, id);
            return RedirectToAction("DeletededIndex");
        }
        
        public IActionResult ChangeStatus(string id)
        {
            var r = _rep.ChangeStatus(ModelState, id);
            return RedirectToAction("Index");
        }


        public IActionResult GetFiles(string Id)
        {

            var result = _rep.GetFiles(ModelState, Id);

            return PartialView("_Files", result);
        }

        
        public IActionResult DisplayGrid()
        {

            var result = _rep.Get(ModelState,this.User);

            return PartialView("_DisplayGrid", result);
        }
        
        public IActionResult DisplayBlockedGrid()
        {

            var result = _rep.GetBlocked(ModelState);

            return PartialView("_DisplayBlockedGrid", result);
        }
        [HttpGet]
        public dynamic Changepassword(string Id)
        {
            try
            {

                var result = _rep.GetById(ModelState, Id);
                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }
                return PartialView("_Changepassword");


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ForgetPassword model)
        {
            try
            {

              

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }
                var result = await _rep.ChangePassword(ModelState, model);
                return Json(result);
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }

        public IActionResult Add()
        {
            ViewBag.Companies = _rep.GetCompanies();
            ViewBag.Cities = _rep.GetCitiies();
            ViewBag.Areas = _rep.GetAreas();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserView model)
        {
            try
            {
                model.Usertype = 4;
                var result = await _rep.Add(ModelState, model);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return Ok(new
                {
                    rout = Url.Action("Index", "Representative")
                });


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }


        }
        

        
        public async Task<IActionResult> Block(string id)
        {
            try
            {

                var result = await _rep.Block(ModelState, id);

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


        public IActionResult GetGovernment(long id)
        {
            try
            {
                if (id != null)
                {
                    var c = _rep.GetGovernment(id);
                    return Json( c);

                }
                else
                {
                    return Json("");

                }
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }

        public dynamic GetUser(string Id)
        {
            try
            {

                var result = _rep.GetById(ModelState, Id);
                var list = result.CoverageIds;
                ViewBag.Companies = _rep.GetCompanies();
                ViewBag.Cities = _rep.GetCitiies();
                ViewBag.Areas = _rep.GetAreas();
                ViewBag.CityAreas = _rep.GetCityAreas(result.CitiyId);


                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return View("Edit", result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserView model)
        {
            try
            {

                var result = await _rep.Edit(ModelState, model);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }

                return Ok(new
                {
                    rout = Url.Action("Index", "Representative")
                });


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }


        }





        public async Task<IActionResult> Delete(string Id)
        {
            try
            {

                var result = await _rep.Delete(ModelState, Id);

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


        public IActionResult Userlocation(string Id)
        {

            var result = _rep.GetLocation(ModelState, Id);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_Location", result);
        }    
        public IActionResult Userlocation3(long Id)
        {

            var result = _rep.GetLocation3(ModelState, Id);
            if (!ModelState.IsValid)
            {
                return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
            }
            return PartialView("_Location", result);
        }


    }
}
