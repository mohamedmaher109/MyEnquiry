using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface.InterfaceApi
{
    public interface IOrder
    {
        Task<dynamic> GetOrdersAsync(ModelStateDictionary modelState, string Authorization);
        Task<dynamic> GetAllListCurrent(ModelStateDictionary modelState, string Authorization);
        Task<dynamic> GetAllListDone(ModelStateDictionary modelState, string Authorization);
        Task<dynamic> GetAllListFromReviewer(ModelStateDictionary modelState, string Authorization);
        Task<dynamic> GetOrdersDetails(ModelStateDictionary modelState, int orderId);
        Task<dynamic> changeCaseStatus(ModelStateDictionary modelState,int orderId,int status, string Authorization);

        
    }
}
