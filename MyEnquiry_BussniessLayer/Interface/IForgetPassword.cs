using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IForgetPassword
    {
        Task<dynamic> SendCode(ModelStateDictionary modelState, string Phone);
        Task<dynamic> Verify(ModelStateDictionary modelState, string UserId, string Code);
        Task<dynamic> ChangePasswprd(ModelStateDictionary modelState, string UserId, string Password, string ConfirmPassword);

    }
}
