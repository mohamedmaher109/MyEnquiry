using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net;
using MyEnquiry_BussniessLayer.Interface.InterfaceApi;
using MyEnquiry_BussniessLayer.ViewModels.Api;
using MyEnquiry_BussniessLayer.Helper;

namespace MyEnquiry_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownController : ControllerBase
    {
        private IDropdown _dropdown;
        
        private readonly ILogger<OrdersController> _logger;

        public DropdownController(IDropdown dropdown, ILogger<OrdersController> logger)
        {
            _dropdown = dropdown;
            _logger = logger;
        }

        [HttpGet("Companies")]
        public ActionResult GetCompanies()
        {
            try
            {
                var result = _dropdown.GetCompanies(ModelState);
                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);
            }
        }
        [HttpGet("Regions")]
        public ActionResult GetRegions(int id)
        {
            try
            {
                var result = _dropdown.GetRegions(ModelState, id);
                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);
            }
        }
      



    }
}
