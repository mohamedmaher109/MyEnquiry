using Microsoft.AspNetCore.Mvc;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface;
using System;
using System.Threading.Tasks;

namespace MyEnquiry.Controllers
{
    public class ForgetController : Controller
    {
        private IForgetPassword _IForgetPassword;
        public ForgetController(IForgetPassword IForgetPassword)
        {
            _IForgetPassword = IForgetPassword;

        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendCode(string Phone)
        {
            try
            {
                var code =await _IForgetPassword.SendCode(ModelState, Phone);
                if (code == null)
                {
                    return RedirectToAction(nameof(ForgetPassword));
                }
                else
                {
                    return RedirectToAction(nameof(Verifiy),new { UserId= code } );

                }
            }
            catch(Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }

        }
        public IActionResult Verifiy(string UserId)
        {
            ViewBag.UserId = UserId;
            return View();
        } 
        [HttpPost]
        public async Task<IActionResult> Verifiy(string UserId,string code)
        {
            try
            {
                ViewBag.UserId = UserId;
                var verfy = await _IForgetPassword.Verify(ModelState, UserId, code);
                if (UserId == null)
                {
                    ModelState.AddModelError("Model", "هذا المستخدم أو الكود غير صحيح");
                    return View();
                }
                else
                {
                    if (verfy != null)
                    {
                        return RedirectToAction(nameof(NewPassword), new { UserId= verfy });
                    }
                    else
                    {
                        return RedirectToAction(nameof(Verifiy), new { UserId = verfy });

                    }
                }
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }
          public IActionResult NewPassword(string UserId)
        {
            ViewBag.UserId = UserId;
            return View();
        }
        [HttpPost]
        public IActionResult NewPassword( string UserId, string Password, string ConfirmPassword)
        {
            ViewBag.UserId = UserId;
            try
            {
                var ChangePasswprd = _IForgetPassword.ChangePasswprd(ModelState, UserId, Password, ConfirmPassword);
                if (ChangePasswprd==null)
                {
                    ModelState.AddModelError("Model", "حدث خطأ ما");
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "User");

                }
            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }
    }
}
