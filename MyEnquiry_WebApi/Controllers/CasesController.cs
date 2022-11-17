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
    public class CasesController : ControllerBase
    {
        private ICases _cases;
        
        private readonly ILogger<OrdersController> _logger;

        public CasesController(ICases cases, ILogger<OrdersController> logger)
        {
            _cases = cases;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetCaseForm")]
        public ActionResult GetCaseForm([FromQuery]int caseId)
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result =  _cases.GetCaseForm(ModelState, caseId);
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
        
        [Authorize]
        [HttpPut("PostForm")]
        public async Task<ActionResult> PostForm([FromForm] List<PostForm> model)
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result = await _cases.PostForm(ModelState, model);
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
