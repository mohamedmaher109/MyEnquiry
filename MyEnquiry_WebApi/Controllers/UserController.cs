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
    public class UserController : ControllerBase
    {
        private IAuth _auth;
        
        private readonly ILogger<UserController> _logger;

        public UserController(IAuth auth, ILogger<UserController> logger)
        {
            _auth = auth;
            _logger = logger;
        }
        [HttpGet("Cities")]
        public async Task<ActionResult> Cities()
        {
            try
            {

                var result = await _auth.CIIES();
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
          [HttpGet("Privacy")]
        public async Task<ActionResult> Privacy()
        {
            try
            {

                var result = await _auth.Privacy();
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

          [HttpGet("Help")]
        public async Task<ActionResult> Help()
        {
            try
            {

                var result = await _auth.Help();
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

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromForm] RegisterRM userModel)
        {
            try
            {
                var result = await _auth.RegisterUserAsync(ModelState, userModel,true);
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
        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginRM model)
        {
            try
            { 
                var result = await _auth.LoginAsync(ModelState, model);
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
        [HttpPut("Profile")]
        public async Task<ActionResult> UpdateProfileAsync([FromForm] RegisterRM model)
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result = await _auth.EditProfile(ModelState, Authorization, model, true);
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
        [HttpGet("Profile")]
        public ActionResult GetProfile()
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result = _auth.GetProfileAsync(ModelState, Authorization);
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



        [HttpPost("ForgetPassowrd")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPassword model)
        {
            try
            {
                var result = await _auth.ForgetPasswordAsync(ModelState, model);
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

        [HttpPost("RestPassowrd")]
        public async Task<IActionResult> RestPassword([FromBody] ResetPassword model)
        {
            try
            {
                var result = await _auth.RestPasswordAsync(ModelState, model);
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

        [HttpPost("Verfication")]
        public async Task<ActionResult> VerficationAsync([FromBody] Verfication VerficationModel)
        {
            try
            {
                var result = await _auth.VerfiyAsync(ModelState, VerficationModel);
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
        [HttpPut("Location")]
        public async Task<ActionResult> UpdateLocationAsync([FromForm] UpdateUserLocation model)
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result = await _auth.UpdateLocation(ModelState, Authorization, model);
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
        [HttpPost("Complaint")]
        public async Task<ActionResult> SendComplaint([FromQuery]string message)
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result = await _auth.SendComplaint(ModelState, Authorization, message);
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
