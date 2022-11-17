using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface.InterfaceApi;
using System;

namespace MyEnquiry_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : Controller
    {
        private Iform _Iform;

        public FormController(Iform Iform)
        {
            _Iform = Iform;
        }
   
        [Authorize]
        [HttpGet]
        public IActionResult Index(string UserId, int CompanyId, string formId, int CaseId)
        {
            try
            {
                var Survy = _Iform.GetSurvey(UserId, CompanyId, formId, CaseId);
                return Ok(Survy);
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);
            }
        }
    }
}
