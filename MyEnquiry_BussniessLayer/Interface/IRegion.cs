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
    public interface IRegion
    {
       
        dynamic Get(ModelStateDictionary modelState);
        dynamic GetCitiies(ModelStateDictionary modelState);
        dynamic GetById(ModelStateDictionary modelState,int Id);
        Task<dynamic> Add(ModelStateDictionary modelState, Regions model);
        Task<dynamic> Edit(ModelStateDictionary modelState, Regions model);
        Task<dynamic> Delete(ModelStateDictionary modelState, int Id);

        
    }
}
