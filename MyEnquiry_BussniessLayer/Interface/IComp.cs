using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IComp
    {
        dynamic GetCompanies(ModelStateDictionary modelState, ClaimsPrincipal user);
        dynamic GetCompany(ModelStateDictionary modelState, ClaimsPrincipal user);
        Task<dynamic> GetTotal(ModelStateDictionary modelState, int Id);
    }
}
