using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface  ICity
    {
        dynamic Get(ModelStateDictionary modelState);
        dynamic GetById(ModelStateDictionary modelState, int Id);
        Task<dynamic> Add(ModelStateDictionary modelState, Cities model);
        Task<dynamic> Edit(ModelStateDictionary modelState, Cities model);
        Task<dynamic> Delete(ModelStateDictionary modelState, int Id);
    }
}
