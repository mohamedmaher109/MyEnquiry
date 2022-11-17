using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IDues
    {
        dynamic GetBanks(ModelStateDictionary modelState,ClaimsPrincipal user);
        dynamic ExportToExcel(ModelStateDictionary modelState, int Id, ClaimsPrincipal user);
        dynamic GetCompany(ModelStateDictionary modelState, ClaimsPrincipal user);
     /*   dynamic GetCompany1(ModelStateDictionary modelState, ClaimsPrincipal user);
        dynamic GetCompany2(ModelStateDictionary modelState, ClaimsPrincipal user);*/
        dynamic GetById(ModelStateDictionary modelState, int Id, ClaimsPrincipal user);
        Task<dynamic> GetTotal(ModelStateDictionary modelState, int Id, ClaimsPrincipal user);
    }
}
