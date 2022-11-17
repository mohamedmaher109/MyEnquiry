using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModel;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IBankCases
    {
       
        dynamic Get(ModelStateDictionary modelState, ClaimsPrincipal user);
        dynamic GetById(ModelStateDictionary modelState,int Id);
        Task<dynamic> Add(ModelStateDictionary modelState, Cases model,ClaimsPrincipal user);
        Task<dynamic> Edit(ModelStateDictionary modelState, Cases model);
        Task<dynamic> Delete(ModelStateDictionary modelState, int Id);

        dynamic GetCompanies(ModelStateDictionary modelState);
        dynamic GetCaseTypes(ModelStateDictionary modelState);

    }
}
