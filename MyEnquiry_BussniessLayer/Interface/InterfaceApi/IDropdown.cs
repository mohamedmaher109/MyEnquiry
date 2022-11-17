using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface.InterfaceApi
{
    public interface IDropdown
    {
        dynamic GetCompanies(ModelStateDictionary modelState);
        dynamic GetRegions(ModelStateDictionary modelState,int id);
       

        
    }
}
