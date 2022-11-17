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
    public class OrdersController : ControllerBase
    {
        private IOrder _order;
        
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrder order, ILogger<OrdersController> logger)
        {
            _order = order;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("Orders")]
        public async Task<ActionResult> GetOrders()
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result =await _order.GetOrdersAsync(ModelState, Authorization);
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
        [HttpGet("GetCurrentOrders")]
        public async Task<ActionResult> GetCurrentOrders()
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result =await _order.GetAllListCurrent(ModelState, Authorization);
                
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
        [HttpGet("GetReviwerOrders")]
        public async Task<ActionResult> GetReviwerOrders()
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result =await _order.GetAllListFromReviewer(ModelState, Authorization);
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
        }        [Authorize]
        [HttpGet("GetDoneOrders")]
        public async Task<ActionResult> GetDoneOrders()
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result =await _order.GetAllListDone(ModelState, Authorization);
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
        [HttpGet("OrdersDetails")]
        public async Task<ActionResult> GetOrdersDetails( int orderId)
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result =await _order.GetOrdersDetails(ModelState, orderId);
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
        [HttpPut("changeCaseStatus")]
        public async Task<ActionResult> changeCaseStatus([FromQuery]int orderId,int status)
        {
            try
            {
                string Authorization = Request.Headers.GetCommaSeparatedValues("Authorization").FirstOrDefault();
                var result =await _order.changeCaseStatus(ModelState, orderId, status, Authorization);
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
