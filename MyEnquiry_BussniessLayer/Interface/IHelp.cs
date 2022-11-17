using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IHelp
    {
        dynamic Get(ModelStateDictionary modelState);

        Task<dynamic> Add(ModelStateDictionary modelState, Helps model);
        dynamic Get1(ModelStateDictionary modelState);
        Task<dynamic> Add1(ModelStateDictionary modelState, Privacy model);
      
    }
}
