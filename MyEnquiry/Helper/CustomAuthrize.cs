

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEnquiry_DataLayer.Models;

namespace MyEnquiry.Helper
{

    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(PermissionItem item)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { item };
        }
    }


    public class AuthorizeActionFilter : IAuthorizationFilter
    {
        private readonly MyAppContext _context;
        private readonly PermissionItem _item;
        public AuthorizeActionFilter(PermissionItem item, MyAppContext context)
        {
            _item = item;
            _context = context;
           
        }
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
          
            bool isAuthorized = CheckAuthorize(context.HttpContext.User, _item); // :)

            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }

       
        private bool CheckAuthorize(ClaimsPrincipal user, PermissionItem item)
        {
            try
            {
                
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    
                }

                var userrole = _context.UserRoles.FirstOrDefault(s => s.UserId == userId);
                if (userrole == null || userrole.RoleId == null)
                {
                    return false;
                }
                var getitempageid = _context.PermissionPages.FirstOrDefault(s => s.PageId == (int)item);
                if (getitempageid == null)
                {
                    return false;
                }
                var getpermissionPage = _context.PermissionPagesRoles.FirstOrDefault(s =>s.PageId== getitempageid.Id && s.RoleId == userrole.RoleId);

                if (getpermissionPage == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

           
        }
    }



    public partial class AuthrizePage
    {
         
       
      

       

        public bool CanView(PermissionItem item, ClaimsPrincipal user)
        {
           
            try
            {
                var _context = new MyAppContext();
               

                if (user == null)
                {
                    return false;
                }
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                var userrole = _context.UserRoles.FirstOrDefault(s => s.UserId == userId);
                if (userrole == null || userrole.RoleId == null)
                {
                    return false;
                }
                var k = (int)item;
                var getitempageid = _context.PermissionPages.FirstOrDefault(s => s.PageId == k);
                if (getitempageid == null)
                {
                    return false;
                }
                var getpermissionPage = _context.PermissionPagesRoles.FirstOrDefault(s =>s.PageId== getitempageid.Id && s.RoleId == userrole.RoleId);

                if (getpermissionPage == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

            
        }
      


    }

    public static class UserHelper
    {
        public static string UserRoleName(ClaimsPrincipal user)
        {
            var _context = new MyAppContext();


            if (user == null)
            {
                return "";
            }
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            var userrole = _context.UserRoles.FirstOrDefault(s => s.UserId == userId);
            if (userrole == null || userrole.RoleId == null)
            {
                return "";
            }

            var role = _context.Roles.FirstOrDefault(s => s.Id == userrole.RoleId);

            return role != null ? role.Name : "";

        }

    }



}
