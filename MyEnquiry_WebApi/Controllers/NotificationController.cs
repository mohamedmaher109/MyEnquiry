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
    public class NotificationController : ControllerBase
    {
        private INotification _notification;
        
        private readonly ILogger<OrdersController> _logger;

        public NotificationController(INotification notification, ILogger<OrdersController> logger)
        {
            _notification = notification;
            _logger = logger;
        }



        [Authorize]
        [HttpGet("Get")]
        public ActionResult Get(int Page = 1)
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result = _notification.GetNotification(ModelState, Authorization, Page);
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
