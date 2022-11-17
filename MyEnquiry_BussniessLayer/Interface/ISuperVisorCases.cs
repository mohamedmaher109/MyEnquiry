using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModel;
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
    public interface ISuperVisorCases
    {

        dynamic GetRepresentative(ModelStateDictionary modelState, int id);

        dynamic GetStatus(ModelStateDictionary modelState);
        dynamic GetAllCases(ModelStateDictionary modelState, ClaimsPrincipal user, int status);
        Task<dynamic> SendCase(ModelStateDictionary modelState, int Id, string UserId);
        Task<dynamic> SendRate(ModelStateDictionary modelState, int Id, int Rate, string Message);
        Task<dynamic> AcceptCase(ModelStateDictionary modelState, int Id, ClaimsPrincipal user);

        Task<dynamic> UploadFile(ModelStateDictionary modelState, int Id, IFormFile file);
        dynamic GetFilesFromRep(ModelStateDictionary modelState, int Id,int type);


        Task<dynamic> EditCaseForm(ModelStateDictionary modelState, List<AnswerVm> model);

    }
}
