using Microsoft.AspNetCore.Http;
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
    public interface ICases
    {
       
        dynamic CasesFromBank(ModelStateDictionary modelState,int BankId,ClaimsPrincipal user);
        dynamic GetRefusedCases(ModelStateDictionary modelState,ClaimsPrincipal user);

        dynamic GetBanks(ModelStateDictionary modelState);
        Task<dynamic> ReciveCase(ModelStateDictionary modelState,int Id,int type);
        Task<dynamic> deleteAllCases(ModelStateDictionary modelState,int Id);
        Task<dynamic> Solved(ModelStateDictionary modelState,int Id);
        Task<dynamic> UploadFile(ModelStateDictionary modelState, int Id, IFormFile file, ClaimsPrincipal user);
        Task<dynamic> RefusedFile(ModelStateDictionary modelState, int Bank, IFormFile file);
        Task<dynamic> GetPDF(ModelStateDictionary modelState, int Id);
        Task<dynamic> ChangeStatus(ModelStateDictionary modelState, int Id,int type, string reson);
        dynamic GetDoneCases(ModelStateDictionary modelState, ClaimsPrincipal user);



    }
}
