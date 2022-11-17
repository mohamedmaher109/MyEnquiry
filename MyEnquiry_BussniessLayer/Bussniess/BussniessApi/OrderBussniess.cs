using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MyEnquiry.Helper;
using MyEnquiry_BussniessLayer.Helper;
using MyEnquiry_BussniessLayer.Interface.InterfaceApi;
using MyEnquiry_BussniessLayer.ViewModels.Api;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess.BussniessApi
{
    public class OrderBussniess : IOrder
    {
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public OrderBussniess(UserManager<ApplicationUser> userManager, IConfiguration configuration, MyAppContext context, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }
        public async Task<dynamic> GetAllListCurrent(ModelStateDictionary modelState, string Authorization)
        {
            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            var user = _context.Users.Include(u => u.Company).Where(u => u.Id == UserId && !u.Deleted).FirstOrDefault();

            if (user == null)
            {
                modelState.AddModelError("غير موجود", "هذا المستخدم غير موجود");
                return null;
            }

            var orders = _context.CasesOrders.Where(s => s.UserId == UserId && ( s.Cases.CaseStatusId == (int)CaseEnumStatus.StartFromRecivers || s.Cases.CaseStatusId == (int)CaseEnumStatus.ArrivedToClient|| s.Cases.CaseStatusId == (int)CaseEnumStatus.DoneFromReciver || s.Cases.CaseStatusId == (int)CaseEnumStatus.AssignedToRecivers|| s.Cases.CaseStatusId == (int)CaseEnumStatus.StartFromRepres) &&(s.Status==2||s.Status==1)&&!s.Cases.Deleted).Include(s => s.Cases).ThenInclude(s => s.Company).Include(a=>a.Cases).Include(s => s.Cases.Bank).Include(a=>a.Cases.Reviewer).Select(s => new {
                companyLogo = s.Cases.Company != null ? s.Cases.Company.Logo : "",
                bankLogo = s.Cases.Bank != null ? s.Cases.Bank.Logo : "",
                clientName = s.Cases.ClientName,
                orders = s.Id,
                date = s.Cases.CreatedAt,
                clientWorkLocation = new
                {
                    workAddress = s.Cases.WorkAddress,
                    workGovernorate = s.Cases.WorkGovernorate
                },
                clientHomeLocation = new
                {
                    homeAddress = s.Cases.HomeAddress,
                    homeGovernorate = s.Cases.HomeGovernorate,
                },
                caseStatus = s.Cases.CaseStatusId,
                status = s.Status,
                bankId = s.Cases.BankId,
                CasesId = s.Cases.Id,
                CaseStatusId = s.Cases.CaseStatusId,
                CaseTypeId = s.Cases.CaseTypeId,
                CompanyId = s.Cases.CompanyId,
                FormLink =
                $"/Surveys/Form?formId={_context.SurveyForms.Where(x =>x.BankId == s.Cases.BankId && x.CaseTypeId== s.Cases.CaseTypeId).FirstOrDefault().LinkIdentifier}&userId={user.Id}&CaseId={s.CaseId}&companyId={s.Cases.CompanyId}",
                ReviwerNumber=s.Cases.Reviewer.PhoneNumber??""
                
            }).ToList();
            foreach (var item in orders)
            {
               
            }
            return new
            {
                result = new
                {
                    orders,
                    
                },
                msg = "Successfully Message"
            };
        }
        public async Task<dynamic> GetAllListDone(ModelStateDictionary modelState, string Authorization)
        {

            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            var user = _context.Users.Include(u => u.Company).Where(u => u.Id == UserId&&!u.Deleted).FirstOrDefault();

            if (user == null)
            {
                modelState.AddModelError("غير موجود", "هذا المستخدم غير موجود");
                return null;
            }

            var orders = _context.CasesOrders.Where(s => s.UserId == UserId && (  s.Cases.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank)&&(s.Status==2 ) && !s.Cases.Deleted).Include(s => s.Cases).ThenInclude(s => s.Company).Include(s => s.Cases.Bank).Include(s => s.Cases.Reviewer).Select(s => new {
                companyLogo = s.Cases.Company != null ? s.Cases.Company.Logo : "",
                bankLogo = s.Cases.Bank != null ? s.Cases.Bank.Logo : "",
                clientName = s.Cases.ClientName,
                orders = s.Id,
                date = s.Cases.CreatedAt,
                clientWorkLocation = new
                {
                    workAddress = s.Cases.WorkAddress,
                    workGovernorate = s.Cases.WorkGovernorate
                },
                clientHomeLocation = new
                {
                    homeAddress = s.Cases.HomeAddress,
                    homeGovernorate = s.Cases.HomeGovernorate,
                },
                caseStatus = s.Cases.CaseStatusId,
                status = s.Status,
                ReviwerNumber = s.Cases.Reviewer.PhoneNumber ?? ""

            }).ToList();
            return new
            {
                result = new
                {
                    orders
                },
                msg = "Successfully Message"
            };
        }   
        public async Task<dynamic> GetAllListFromReviewer(ModelStateDictionary modelState, string Authorization)
        {
            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            var user = _context.Users.Include(u => u.Company).Where(u => u.Id == UserId).FirstOrDefault();

            if (user == null)
            {
                modelState.AddModelError("غير موجود", "هذا المستخدم غير موجود");
                return null;
            }

            var orders = _context.CasesOrders.Where(s => s.UserId == UserId && (s.Cases.CaseStatusId == (int)CaseEnumStatus.WaitingForRecivers || s.Cases.CaseStatusId==(int)CaseEnumStatus.DoneFromReciver||  s.Cases.CaseStatusId == (int)CaseEnumStatus.AcceptedFromReviewer || s.Cases.CaseStatusId == (int)CaseEnumStatus.AcceptedFromSupervisor || s.Cases.CaseStatusId == (int)CaseEnumStatus.CaseDone ) && s.Status == 2 && !s.Cases.Deleted).Include(s => s.Cases).ThenInclude(s => s.Company).Include(s => s.Cases.Bank).Include(s => s.Cases.Reviewer).Select(s => new {
                companyLogo = s.Cases.Company != null ? s.Cases.Company.Logo : "",
                bankLogo = s.Cases.Bank != null ? s.Cases.Bank.Logo : "",
                clientName = s.Cases.ClientName,
                orders = s.Id,
                date = s.Cases.CreatedAt,
                clientWorkLocation = new
                {
                    workAddress = s.Cases.WorkAddress,
                    workGovernorate = s.Cases.WorkGovernorate
                },
                clientHomeLocation = new
                {
                    homeAddress = s.Cases.HomeAddress,
                    homeGovernorate = s.Cases.HomeGovernorate,
                },
                caseStatus = s.Cases.CaseStatusId,
                status=s.Status,
                ReviwerNumber = s.Cases.Reviewer.PhoneNumber ?? ""
            }).ToList();

            return new
            {
                result = new
                {
                    orders
                },
                msg = "Successfully Message"
            };
        }
           public async Task<dynamic> GetOrdersDetails(ModelStateDictionary modelState, int orderId)
        {


          
            var orders = _context.CasesOrders.Where(s=>s.Id==orderId && !s.Cases.Deleted).Include(s => s.Cases).ThenInclude(s=>s.Company).Include(s=>s.Cases.Bank).Select(s => new {
                companyLogo = s.Cases.Company != null ? s.Cases.Company.Logo : "",
                bankLogo = s.Cases.Bank != null ? s.Cases.Bank.Logo : "",
                clientName = s.Cases.ClientName,
                orders = s.Id,
                date = s.Cases.CreatedAt,
                clientWorkLocation = new
                {
                    workAddress = s.Cases.WorkAddress,
                    workGovernorate = s.Cases.WorkGovernorate
                },
                clientHomeLocation = new
                {
                    homeAddress = s.Cases.HomeAddress,
                    homeGovernorate = s.Cases.HomeGovernorate,
                },
                caseStatus = s.Cases.CaseStatusId,
                status = s.Status

            }).ToList();

            return new
            {
                result = new
                {
                    orders
                },
                msg = "Successfully Message"
            };

        }
        public async Task<dynamic> GetOrdersAsync(ModelStateDictionary modelState, string Authorization)
        {

            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            var user = _context.Users.Include(u=>u.Company).Where(u => u.Id == UserId).FirstOrDefault();

            if (user == null)
            {
                modelState.AddModelError("غير موجود", "هذا المستخدم غير موجود");
                return null;
            }

            var orders = _context.CasesOrders.Where(s=>!s.Cases.Deleted).Include(s=>s.Cases).Where(s => s.UserId == UserId).Select(s=>new { 
            companyLogo=user.Company!=null?user.Company.Logo:"",
            clientName=s.Cases.ClientName,
            orders=s.Id,
            date= s.Cases.CreatedAt,
                clientWorkLocation = new {
                    workAddress=s.Cases.WorkAddress,
                    workGovernorate=s.Cases.WorkGovernorate
                },
                clientHomeLocation = new {
                    homeAddress=s.Cases.HomeAddress,
                    homeGovernorate=s.Cases.HomeGovernorate,
                },
            caseStatus=s.Cases.CaseStatusId,
                status = s.Status

            }).ToList();

            return new
            {
                result = new
                {
                    orders
                },
                msg = "Successfully Message"
            };

        }
        public async Task<dynamic> changeCaseStatus(ModelStateDictionary modelState, int orderId,int status, string Authorization)
        {
            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            

            var getorder = _context.CasesOrders.Include(a=>a.Cases).FirstOrDefault(s => s.Id == orderId);
            if (getorder == null)
            {
                modelState.AddModelError("غير موجود", "هذا الطلب غير موجود");
                return null;
            }

            var getuserorders = _context.CasesOrders.Include(s => s.Cases)
               .Where(s => s.Status == 2 && s.UserId == UserId && s.Cases.CaseStatusId < (int)CaseEnumStatus.ArrivedToClient && !s.Cases.Deleted).Include(a=>a.Cases).ToList();

            var getotherorders = _context.CasesOrders.Where(s => s.CaseId == getorder.CaseId && s.Id != getorder.Id && !s.Cases.Deleted).Include(a => a.Cases).ToList();


            switch (status)
            {
                case 1:
                   

                    if (getuserorders.Count >= 6)
                    {
                        modelState.AddModelError("غير مسموح", "غير مسموح بقبول اكثر من 6 حالات");
                        return null;
                    }
                    if (getorder.Status == 3)
                    {
                        modelState.AddModelError("غير مسموح", "تم الغاء هذا الطلب");
                        return null;
                    }
                    
                    if (getotherorders.Count > 1)
                    {
                        getotherorders.ForEach(s => s.Status = 3);
                    }

                    getorder.Status = 2;
                   
                    await _context.SaveChangesAsync();
                    getorder.Cases.CaseStatusId = 6;
                    await _context.SaveChangesAsync();
                    break;

                case 2:
                    getorder.Status = 3;

                    await _context.SaveChangesAsync();

                    break;
                case 3:
                    getorder.Cases.CaseStatusId=(int)CaseEnumStatus.StartFromRepres;

                    await _context.SaveChangesAsync();

                    break;
                case 4:
                    getorder.Cases.CaseStatusId=(int)CaseEnumStatus.ArrivedToClient;

                    await _context.SaveChangesAsync();

                    break;
                case 5:
                    getorder.Cases.CaseStatusId=(int)CaseEnumStatus.DoneFromReciver;

                    await _context.SaveChangesAsync();

                    break;
                default:
                    break;
            }

            
            
            
            
           
            



            return new
            {
                result = new
                {

                },
                msg = "Successfully Message"
            };
        }

     
    }
}
