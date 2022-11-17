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
    public class RegionController : Controller
    {
        private IRegion _region;


        public RegionController(IRegion region)
        {
            _region = region;

        }
        public IActionResult RegionIndex()
        {
            ViewBag.Cities = _region.GetCitiies(ModelState);
            return View();
        }
        public IActionResult DisplayGrid()
        {
            ViewBag.Cities = _region.GetCitiies(ModelState);
            var result = _region.Get(ModelState);

            return PartialView("_DisplayRegions", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Regions model)
        {
            try
            {

                var result = await _region.Add(ModelState, model);

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




        public dynamic GetRegion(int Id)
        {
            try
            {
                ViewBag.Cities = _region.GetCitiies(ModelState);
                var result = _region.GetById(ModelState, Id);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return PartialView("_EditRegion", result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Regions model)
        {
            try
            {

                var result = await _region.Edit(ModelState, model);

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

                var result = await _region.Delete(ModelState, Id);

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
