using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface.InterfaceApi
{
    public interface INotification
    {
        dynamic GetNotification(ModelStateDictionary modelState,string Authorization,int Page);
       

        
    }
}
