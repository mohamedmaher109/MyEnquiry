using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface.InterfaceApi
{
    public interface ICases
    {
       
         dynamic GetCaseForm(ModelStateDictionary modelState,int caseId);
         Task<dynamic> PostForm(ModelStateDictionary modelState, List<PostForm> model);

        
    }
}
