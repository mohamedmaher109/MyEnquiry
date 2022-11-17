using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IRoles
    {
       
        dynamic GetRoles(ModelStateDictionary modelState);
        dynamic GetRole(ModelStateDictionary modelState,string Id);
        Task<dynamic> AddRole(ModelStateDictionary modelState, RoleView model);
        Task<dynamic> EditRole(ModelStateDictionary modelState, RoleView model);
        Task<dynamic> DeleteRole(ModelStateDictionary modelState, string Id);

        
    }
}
