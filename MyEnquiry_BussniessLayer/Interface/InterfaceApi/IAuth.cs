using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface.InterfaceApi
{
    public interface IAuth
    {
        Task<dynamic> RegisterUserAsync(ModelStateDictionary modelState, RegisterRM userModel, bool isAbi);
        Task<dynamic> EditProfile(ModelStateDictionary modelState, string Authorization, RegisterRM userModel, bool isAbi);
        Task<dynamic> CIIES();
        Task<dynamic> Privacy();
        Task<dynamic> Help();
        Task<dynamic> LoginAsync(ModelStateDictionary modelState, LoginRM model);
        Task<dynamic> ForgetPasswordAsync(ModelStateDictionary modelState, ForgetPassword userModel);
        Task<dynamic> RestPasswordAsync(ModelStateDictionary modelState, ResetPassword userModel);

        Task<dynamic> VerfiyAsync(ModelStateDictionary modelState, Verfication VerficationModel);
        Task<dynamic> UpdateLocation(ModelStateDictionary modelState, string Authorization, UpdateUserLocation model);
        Task<dynamic> SendComplaint(ModelStateDictionary modelState, string Authorization, string message);
        dynamic GetProfileAsync(ModelStateDictionary modelState, string Authorization);

    }
}
