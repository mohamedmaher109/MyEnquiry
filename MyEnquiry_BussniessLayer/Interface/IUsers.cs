using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModel;
using MyEnquiry_BussniessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IUsers
    {


        Task<dynamic> Changepassword(ModelStateDictionary modelState, Changepassword model);

        dynamic GetUsers(ModelStateDictionary modelState);
        dynamic GetBanks(ModelStateDictionary modelState);
        dynamic GetCompanies(ModelStateDictionary modelState);

        Task<dynamic> AddUser(ModelStateDictionary modelState, UserView model);
        dynamic GetUser(ModelStateDictionary modelState, string Id);
        dynamic GetRolesForUser(ModelStateDictionary modelState, string Id);

        Task<dynamic> EditUser(ModelStateDictionary modelState, UserView model);
        Task<dynamic> ChangePassword(ModelStateDictionary modelState, ForgetPassword model);


        Task<dynamic> DeleteUser(ModelStateDictionary modelState, string Id);

        Task<dynamic> WebLoginUserAsync(ModelStateDictionary modelState, LoginRq userModel);
        Task<dynamic> SignUserOutAsync(ModelStateDictionary modelState);

        dynamic GetPermissionPages(ModelStateDictionary modelState, string Id);

        dynamic PostPermissions(ModelStateDictionary modelState, string[] Ids, string roleid);


        dynamic GetUsersCash(ModelStateDictionary modelState);

    }
}
