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
    public interface ICaseForms
    {
       
        dynamic Get(ModelStateDictionary modelState);
        Task<dynamic> Add(ModelStateDictionary modelState, string Form, int CaseTypeId);
        Task<dynamic> Delete(ModelStateDictionary modelState, int Id);

        dynamic GetCaseTypes(ModelStateDictionary modelState);

    }
}
