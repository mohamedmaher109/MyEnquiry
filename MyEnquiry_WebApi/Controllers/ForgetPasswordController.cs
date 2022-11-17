using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Interface;

namespace MyEnquiry_WebApi.Controllers
{
    public class ForgetPasswordController : Controller
    {
        private IForgetPassword _IForgetPassword;
        public ForgetPasswordController(IForgetPassword IForgetPassword)
        {
            _IForgetPassword = IForgetPassword;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
