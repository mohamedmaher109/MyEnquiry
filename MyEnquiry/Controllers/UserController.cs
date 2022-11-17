using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class UserController : Controller
    {


        private IRoles _role;
        private IUsers _user;


        public UserController(IRoles role, IUsers user)
        {
            _role = role;
            _user = user;

        }

        public IActionResult Index()
        {
            return View();
        }


        #region Roles
        [HttpGet]
        //[Authorize(PermissionItem.Roles)]
        public IActionResult Roles()
        {
            return View();
        }

        public IActionResult DisplayRoleGrid()
        {

            var result = _role.GetRoles(ModelState);
            return PartialView("_DisplayRoles", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RoleView model)
        {
            try
            {

                var result = await _role.AddRole(ModelState, model);

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


        public dynamic GetRole(string Id)
        {
            try
            {

                var result = _role.GetRole(ModelState, Id);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return PartialView("_EditRole", result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RoleView model)
        {
            try
            {

                var result = await _role.EditRole(ModelState, model);

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

        public async Task<IActionResult> DeleteRole(string Id)
        {
            try
            {

                var result = await _role.DeleteRole(ModelState, Id);

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


        #endregion


        #region Users
        [HttpGet]
        //[Authorize(PermissionItem.Roles)]
        public IActionResult CashIndex()
        {
            return View();
        }

        public IActionResult DisplayCashGrid()
        {

            var result = _user.GetUsersCash(ModelState);
            return PartialView("_DisplayCashs", result);
        }

        [HttpGet]
        //[Authorize(PermissionItem.Roles)]
        public IActionResult Users()
        {
            ViewBag.Roles = _role.GetRoles(ModelState);
            ViewBag.Banks = _user.GetBanks(ModelState);
            ViewBag.Companies = _user.GetCompanies(ModelState);
            return View();
        }

        public IActionResult DisplayUserGrid()
        {

            var result = _user.GetUsers(ModelState);
            return PartialView("_DisplayUsers", result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserView model)
        {
            try
            {

                var result = await _user.AddUser(ModelState, model);

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

        [HttpGet]
        public dynamic Changepassword(string Id)
        {
            try
            {

                var result = _user.GetUser(ModelState, Id);

                

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
                var result = await _user.ChangePassword(ModelState, model);
                return Json(result);
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

                var result = _user.GetUser(ModelState, Id);

                ViewBag.Roles = _role.GetRoles(ModelState);

                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }


                return PartialView("_EditUser", result);


            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);

            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserView model)
        {
            try
            {

                var result = await _user.EditUser(ModelState, model);

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

        public async Task<IActionResult> DeleteUser(string Id)
        {
            try
            {
                var result = await _user.DeleteUser(ModelState, Id);

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

        #endregion



        #region Permissions

        public IActionResult PermissionsIndex()
        {
            ViewBag.Roles = _role.GetRoles(ModelState);
            return View();
        }


        [HttpGet]
        public IActionResult Permissions(string Id)
        {
            try
            {
                var result = _user.GetPermissionPages(ModelState, Id);


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


        [HttpPost]
        public IActionResult PostPermissions(string[] Ids, string roleid)
        {
            try
            {
                var result = _user.PostPermissions(ModelState, Ids, roleid);


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


        #endregion





        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        public async Task<ActionResult> Logout()
        {
            _user.SignUserOutAsync(ModelState);
            return RedirectToAction("Login", "User");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginRq userModel)
        {
            try
            {
                var result = await _user.WebLoginUserAsync(ModelState, userModel);
                if (!ModelState.IsValid)
                {
                    return CustomBadRequest.CustomModelStateErrorResponse(ModelState);
                }

                return Ok(new
                {
                    rout = Url.Action("HomeIndex", "Home")
                });

            }
            catch (Exception ex)
            {
                return CustomBadRequest.CustomExErrorResponse(ex);
            }


        }

    }
}
