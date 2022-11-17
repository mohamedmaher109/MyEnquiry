using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyEnquiry_BussniessLayer.ViewModel;
using MyEnquiry_BussniessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IRepresentative
    {


        Task<dynamic> Changepassword(ModelStateDictionary modelState, Changepassword model);

        dynamic Get(ModelStateDictionary modelState,ClaimsPrincipal user);
        dynamic GetBlocked(ModelStateDictionary modelState);
        dynamic GetFiles(ModelStateDictionary modelState,string Id);
        dynamic GetDeleted(ModelStateDictionary modelState);
        Task<dynamic> BackFromDeleted(ModelStateDictionary modelState, string Id);
        Task<dynamic> ChangeStatus(ModelStateDictionary modelState, string Id);
        
        dynamic GetCompanies();
        SelectList GetAreas();
        SelectList GetCityAreas(int cityid);
        dynamic GetLocation(ModelStateDictionary modelState,string id);
        dynamic GetLocation3(ModelStateDictionary modelState,long id);

        Task<dynamic> Add(ModelStateDictionary modelState, UserView model);
        dynamic GetGovernment( long id);
        dynamic GetById(ModelStateDictionary modelState, string Id);

        Task<dynamic> Edit(ModelStateDictionary modelState, UserView model);
        Task<dynamic> Block(ModelStateDictionary modelState, string id);

        dynamic GetCitiies();
        Task<dynamic> Delete(ModelStateDictionary modelState, string Id);

        public  Task<dynamic> ChangePassword(ModelStateDictionary modelState, ForgetPassword model);
   



    }
}
