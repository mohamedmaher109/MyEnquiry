using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IBankCompanyCase
    {
        dynamic Get(ModelStateDictionary modelState, ClaimsPrincipal user);
        dynamic GetCompany(ModelStateDictionary modelState, ClaimsPrincipal user);
        dynamic GetBankCompany(ModelStateDictionary modelState, ClaimsPrincipal user, int Id);
        dynamic GetAllCompany(ModelStateDictionary modelState, ClaimsPrincipal user, int Id);
        dynamic GetById(ModelStateDictionary modelState, int Id);
        Task<dynamic> Add(ModelStateDictionary modelState, BankCompanyVmAdd model, ClaimsPrincipal user);
        Task<dynamic> Edit(ModelStateDictionary modelState, BankCompanyVmAdd model, ClaimsPrincipal user);
        Task<dynamic> Delete(ModelStateDictionary modelState, int Id);

        dynamic GetCities(ModelStateDictionary modelState);
        dynamic GetCaseTypes(ModelStateDictionary modelState);
    }
}
