using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IBills
    {
        dynamic Get(ModelStateDictionary modelState, ClaimsPrincipal user);
        dynamic GetById(ModelStateDictionary modelState, int Id, ClaimsPrincipal user);
        Task<dynamic> Pay(ModelStateDictionary modelState, int Id, int date, ClaimsPrincipal user);

    }
}
