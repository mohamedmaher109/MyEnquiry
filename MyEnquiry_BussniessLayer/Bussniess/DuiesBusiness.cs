using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyEnquiry.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class DuiesBusiness : IDues
    {
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public DuiesBusiness(IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }
        public async Task<dynamic> GetTotal(ModelStateDictionary modelState, int id, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var usera4 = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault();
            var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted);
            //var Casesse = _context.Cases.Include(a => a.Company).FirstOrDefault(r => r.CompanyId == id && !r.PaidFromBank).CompanyId;
            var Casesse1 = _context.Cases.Include(a => a.Company).Where(a => a.CompanyId == id && !a.Deleted && !a.PaidFromBank && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && a.BankId == usera.FirstOrDefault().BankId).FirstOrDefault();
            var Count = _context.Cases.Where(a => a.CompanyId == id && !a.Deleted && !a.PaidFromBank && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank&& a.BankId == usera.FirstOrDefault().BankId).Count();
            var bank = _context.BankCompany.Include(a => a.Cities).Where(a => a.Cities.NameAr == Casesse1.HomeGovernorate && a.CaseTypesId == Casesse1.CaseTypeId && a.BanksId == usera.FirstOrDefault().BankId && a.CompaniesId == id & !a.Deleted).ToList().Sum(a => a.PriceCase);

            if (id == null)
            {
                modelState.AddModelError("غير موجود", "هذا المستخدم غير موجود");
                return null;
            }
            /*          var AllCasesCount = _context.Cases.Where(a => a.CompanyId == Casesse).Count();
                      var AllCasesStatusReview1 = _context.Cases.Include(a=>a.Reviewer).Where(a => a.CompanyId == Casesse&&a.Reviewer!=null).Select(a=>new  { a.Reviewer.price }).ToList();
                      var AllCasesStatusReview =AllCasesStatusReview1.Sum(a => (a.price));
                      var AllCasesStatusSuperVisor1 = _context.Cases.Include(a => a.SuperVisor).Where(a => a.CompanyId == Casesse && a.SuperVisor != null).Select(a => new { a.SuperVisor.price }).ToList();
                      var AllCasesStatusSuperVisor = AllCasesStatusSuperVisor1.Sum(a => (a.price));*/
            var totalh = Count * bank;

            return new
            {
                result = new
                {

                },
                msg = totalh
            };


        }
        public dynamic GetBanks(ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted);
            if (usera != null && usera.Any(a => a.BankId != null))
            {
                if ((usera.FirstOrDefault().UserType != 1 || usera.FirstOrDefault().BankId == null))
                {
                    modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                    return null;
                }
                var BillBank = _context.Cases.Include(a => a.Company).Where(a => !a.Deleted && a.BankId == usera.FirstOrDefault().BankId && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !a.PaidFromBank).Select(q => new DuiesBank
                {

                    Id = q.CompanyId,
                    NameCompany = q.Company.NameAr

                }).Distinct().Select(q => new DuiesBank
                {
                    Id = q.Id,
                    NameCompany = q.NameCompany

                }).ToList();

                return BillBank;
            }
            return null;
        }
/*        public dynamic GetBanks(ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted);
            //var Uyf = _context.BankCompany.Where(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId).FirstOrDefault();
            var BnjkId = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault().BankId;
            if (usera != null && usera.Any(a => a.BankId != null))
            {
                *//*  var use2r = _context.Users.Where(a => !a.Deleted&&a.BankId== BnjkId).ToList();
                  foreach(var item in use2r)
                  {*//*
                var Duies = _context.Cases.Where(a => !a.Deleted && a.BankId == usera.FirstOrDefault().BankId && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !a.PaidFromBank).OrderBy(a => a.HomeGovernorate).OrderBy(a => a.CaseTypeId).Distinct().Include(a => a.Bank).Include(a => a.Reviewer).Include(a => a.CaseStatus).Include(a => a.SuperVisor).Include(a => a.CaseType).Select(s => new DuiesBank
                {
                    
                    NameCompany = s.Company.NameAr ?? "",
                    TypeOfCases = s.CaseType.NameAr ?? "",
                    City = s.HomeGovernorate,
                    NumberOfCases = _context.Cases.Where(c => c.HomeGovernorate == s.HomeGovernorate && c.CompanyId == s.CompanyId&& s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !s.PaidFromBank&&s.BankId== usera.FirstOrDefault().BankId&&!c.Deleted).Count(),
                    Price = _context.BankCompany.Include(a => a.Cities).Any(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId) != false ? _context.BankCompany.Include(a => a.Cities).Where(a => !a.Deleted && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId && s.BankId == usera.FirstOrDefault().BankId).FirstOrDefault().PriceCase : 0,
                    Total = _context.BankCompany.Include(a=>a.Cities).Any(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId) != false ? (_context.BankCompany.Include(a => a.Cities).Where(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId).FirstOrDefault().PriceCase * _context.Cases.Include(a => a.Company).Where(c => c.HomeGovernorate == s.HomeGovernorate&&!c.Deleted && c.Company.NameAr == s.Company.NameAr && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !s.PaidFromBank && s.BankId == usera.FirstOrDefault().BankId).Count()) : 0
                }).Distinct().ToList();
                return Duies;


            }

            return null;
        }*/

        public dynamic GetCompany(ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted);
            var user1a = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault().CompanyId;
            var casw = _context.Cases.Where(a => !a.Deleted && a.CompanyId == user1a).FirstOrDefault();
            //var Uyf = _context.BankCompany.Where(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId).FirstOrDefault();
            var BnjkId = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault().BankId;
            if (usera != null && usera.Any(a => a.CompanyId != null))
            {
                if ((usera.FirstOrDefault().UserType != 2 || usera.FirstOrDefault().CompanyId == null))
                {
                    modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                    return null;
                }
                /*  var use2r = _context.Users.Where(a => !a.Deleted&&a.BankId== BnjkId).ToList();
                  foreach(var item in use2r)
                  {*/
                var ListCompanyVm2 = new ListCompanyVm();
                ListCompanyVm2.GeCompanyVms1 = _context.Cases.Where(a => !a.Deleted && a.CompanyId == usera.FirstOrDefault().CompanyId && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank&&!a.PaidFromCompany).Distinct().Include(a => a.Bank).Include(a => a.Reviewer).Include(a => a.CaseStatus).Include(a => a.SuperVisor).Include(a => a.CaseType).Select(s => new GeCompanyVm
                {
                    Id1 = s.SuperVisorId,
                    Name = s.SuperVisor.FullName ?? "",
                    title = s.SuperVisor != null ? "سوبر فايزر"  : "",
                    /*   TypeOfCases = s.CaseType.NameAr ?? "",*/
                    NumberOfCash = s.SuperVisor.CashNumber  ?? "",
                    NoOfCount = s.SuperVisor != null ? _context.Cases.Where(c => c.SuperVisorId == s.SuperVisorId && c.Company.NameAr == s.Company.NameAr && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !s.PaidFromCompany).Count()  : 0,
                    Price = s.SuperVisor != null ? s.SuperVisor.price : 0,
                    Total = s.SuperVisor != null ? s.SuperVisor.price * _context.Cases.Where(c => c.SuperVisorId == s.SuperVisorId && c.Company.NameAr == s.Company.NameAr && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !s.PaidFromCompany).Count()  : 0
                }).Distinct().ToList();
                ListCompanyVm2.GeCompanyVms2 = _context.Cases.Where(a => !a.Deleted && a.CompanyId == usera.FirstOrDefault().CompanyId && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !a.PaidFromCompany).Distinct().Include(a => a.Bank).Include(a => a.Reviewer).Include(a => a.CaseStatus).Include(a => a.SuperVisor).Include(a => a.CaseType).Select(s => new GeCompanyVm
                {
                    Id1 = s.ReviewerId,
                    Name = s.Reviewer.FullName  ?? "",
                    title = s.Reviewer != null ? "مراجع": "",
                    /*   TypeOfCases = s.CaseType.NameAr ?? "",*/
                    NumberOfCash = s.Reviewer.CashNumber?? "",
                    NoOfCount = s.Reviewer != null ? _context.Cases.Where(c => c.ReviewerId == s.ReviewerId && c.Company.NameAr == s.Company.NameAr && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !s.PaidFromCompany) .Count() : 0,
                    Price = s.Reviewer != null ? s.Reviewer.price  : 0,
                    Total = s.Reviewer != null ? s.Reviewer.price * _context.Cases.Where(c => c.ReviewerId == s.ReviewerId && c.Company.NameAr == s.Company.NameAr && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !s.PaidFromCompany).Count() : 0
                }).Distinct().ToList();
                ListCompanyVm2.GeCompanyVms3 = _context.CasesOrders.Where(a => a.User.CompanyId == usera.FirstOrDefault().CompanyId && a.Status == 2&& a.Cases.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !a.Cases.PaidFromCompany).Distinct().Include(a => a.User).Include(a=>a.Cases).Select(s => new GeCompanyVm
                {
                    Id1 = s.UserId,
                    Name = s.User.FullName,
                    title = "مستعلم",
                    /*   TypeOfCases = s.CaseType.NameAr ?? "",*/
                    NumberOfCash = s.User.CashNumber,
                    NoOfCount = _context.CasesOrders.Where(a => a.User.CompanyId == usera.FirstOrDefault().CompanyId && a.Status == 2 && a.UserId == s.UserId && !s.Cases.PaidFromCompany).Count(),
                    Price = s.User.price,
                    Total = s.User.price * _context.CasesOrders.Where(a => a.User.CompanyId == usera.FirstOrDefault().CompanyId && a.Status == 2 && a.UserId == s.UserId && !s.Cases.PaidFromCompany).Count()
                }).Distinct().ToList();
                return ListCompanyVm2;

          


            }

            return null;
        }

        /*        public dynamic GetCompany1(ModelStateDictionary modelState, ClaimsPrincipal user)
                {
                    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted);
                    //var Uyf = _context.BankCompany.Where(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId).FirstOrDefault();
                    var BnjkId = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault().BankId;
                    if (usera != null && usera.Any(a => a.CompanyId != null))
                    {
                        *//*  var use2r = _context.Users.Where(a => !a.Deleted&&a.BankId== BnjkId).ToList();
                          foreach(var item in use2r)
                          {*//*
                        var Duies = _context.Cases.Where(a => !a.Deleted && a.CompanyId == usera.FirstOrDefault().CompanyId && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank).Distinct().Include(a => a.Bank).Include(a => a.Reviewer).Include(a => a.CaseStatus).Include(a => a.SuperVisor).Include(a => a.CaseType).Select(s => new GeCompanyVm
                        {
                            Id = s.Id,
                            Name = s.Reviewer.FullName ?? _context.CasesOrders.Include(a => a.User).Where(a => a.CaseId == s.Id && a.Status == 2).FirstOrDefault().User.FullName ?? "",
                            title = s.Reviewer != null ? "مراجع" : _context.CasesOrders.Include(a => a.User).Any(a => a.CaseId == s.Id) != false ? "مستعلم" : "",
                            *//*   TypeOfCases = s.CaseType.NameAr ?? "",*//*
                            NumberOfCash = s.Reviewer.CashNumber ?? _context.CasesOrders.Include(a => a.User).Where(a => a.CaseId == s.Id && a.Status == 2).FirstOrDefault().User.CashNumber ?? "",
                            NoOfCount = s.Reviewer != null ? _context.Cases.Where(c => c.SuperVisorId == s.SuperVisorId && c.Company.NameAr == s.Company.NameAr && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank).Count() : _context.CasesOrders.Include(a => a.User).Any(a => a.CaseId == s.Id && a.Status == 2) != false ? _context.CasesOrders.Include(a => a.User).Where(a => a.CaseId == s.Id && a.Status == 2).Count() : 0,
                            Price = s.Reviewer != null ? s.Reviewer.price : _context.CasesOrders.Include(a => a.User).Any(a => a.CaseId == s.Id && a.Status == 2) != false ? _context.CasesOrders.Include(a => a.User).Where(a => a.CaseId == s.Id && a.Status == 2).FirstOrDefault().User.price : 0,
                            Total = s.Reviewer != null ? s.Reviewer.price * _context.Cases.Where(c => c.ReviewerId == s.SuperVisorId && c.Company.NameAr == s.Company.NameAr && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank).Count() : _context.CasesOrders.Include(a => a.User).Any(a => a.CaseId == s.Id && a.Status == 2) != false ? _context.CasesOrders.Include(a => a.User).Where(a => a.CaseId == s.Id && a.Status == 2).FirstOrDefault().User.price * _context.CasesOrders.Include(a => a.User).Where(a => a.CaseId == s.Id && a.Status == 2).Count() : 0
                        }).ToList();
                        return Duies;


                    }

                    return null;
                }

                public dynamic GetCompany2(ModelStateDictionary modelState, ClaimsPrincipal user)
                {
                    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    var usera1 = _context.Users.Where(a => a.Id == userId && !a.Deleted);
                    var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault().CompanyId;
                    var casw = _context.Cases.Where(a => !a.Deleted && a.CompanyId == usera).FirstOrDefault();
                    //var Uyf = _context.BankCompany.Where(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId).FirstOrDefault();
                    var BnjkId = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault().BankId;
                    if (usera != null && usera1.Any(a => a.CompanyId != null))
                    {
                        *//*  var use2r = _context.Users.Where(a => !a.Deleted&&a.BankId== BnjkId).ToList();
                          foreach(var item in use2r)
                          {*//*
                        var Duies = _context.CasesOrders.Where(a => a.CaseId == casw.Id && a.Status == 2).Distinct().Include(a => a.User).Select(s => new GeCompanyVm
                        {
                            Id = s.Id,
                            Name = s.User.FullName,
                            title = "مستعلم",
                            *//*   TypeOfCases = s.CaseType.NameAr ?? "",*//*
                            NumberOfCash = s.User.CashNumber,
                            NoOfCount = _context.CasesOrders.Where(a => a.CaseId == casw.Id && a.Status == 2 && a.UserId == s.UserId).Count(),
                            Price = s.User.price,
                            Total = s.User.price * _context.CasesOrders.Where(a => a.CaseId == casw.Id && a.Status == 2 && a.UserId == s.UserId).Count()
                        }).ToList();
                        return Duies;


                    }

                    return null;
                }*/

        public dynamic GetById(ModelStateDictionary modelState, int Id, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted);
            //var Uyf = _context.BankCompany.Where(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId).FirstOrDefault();
           // var BnjkId = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault().BankId;
            if (usera != null && usera.Any(a => a.BankId != null))
            {
                 
                 var Duies = _context.Cases.Where(a => !a.Deleted && a.BankId == usera.FirstOrDefault().BankId && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !a.PaidFromBank&&a.CompanyId==Id).OrderBy(a => a.HomeGovernorate).OrderBy(a => a.CaseTypeId).Include(a => a.Bank).Include(a => a.Reviewer).Include(a => a.CaseStatus).Include(a => a.SuperVisor).Include(a => a.CaseType).Select(s => new DuiesBank
                 {
                     NameCompany = s.Company.NameAr ?? "",
                     TypeOfCases = s.CaseType.NameAr ?? "",
                     City = s.HomeGovernorate,
                     NumberOfCases = _context.Cases.Where(c => c.HomeGovernorate == s.HomeGovernorate && c.CompanyId ==Id && c.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !c.PaidFromBank && c.BankId == usera.FirstOrDefault().BankId && !c.Deleted).Count(),
                     Price = _context.BankCompany.Include(a => a.Cities).Any(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId && a.CompaniesId == Id) != false ? _context.BankCompany.Include(a => a.Cities).Where(a => !a.Deleted && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId && a.BanksId == usera.FirstOrDefault().BankId && a.CompaniesId == Id).FirstOrDefault().PriceCase : 0,
                     Total = _context.BankCompany.Include(a => a.Cities).Any(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId&&a.CompaniesId==Id) != false ? (_context.BankCompany.Include(a => a.Cities).Where(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId && a.CompaniesId == Id).FirstOrDefault().PriceCase * _context.Cases.Include(a => a.Company).Where(c => c.HomeGovernorate == s.HomeGovernorate && !c.Deleted && c.CompanyId == Id && c.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !c.PaidFromBank && c.BankId == usera.FirstOrDefault().BankId).Count()) : 0
                 }).Distinct().ToList();
                    return Duies;


                }

                return null;
            }

        public dynamic ExportToExcel(ModelStateDictionary modelState, int Id, ClaimsPrincipal user)
        {
            DataTable dt = new DataTable("تقرير");
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("الإجمالى"),
                                        new DataColumn("نوع الحاله"),
                                        new DataColumn("المحافظه"),
                                        new DataColumn("عدد الحالات"),
                new DataColumn("سعر الحاله"), new DataColumn("إسم الشركه ")
           });

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted);
            if (usera != null && usera.Any(a => a.BankId != null))
            {

                var Duies = _context.Cases.Where(a => !a.Deleted && a.BankId == usera.FirstOrDefault().BankId && a.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !a.PaidFromBank && a.CompanyId == Id).OrderBy(a => a.HomeGovernorate).OrderBy(a => a.CaseTypeId).Include(a => a.Bank).Include(a => a.Reviewer).Include(a => a.CaseStatus).Include(a => a.SuperVisor).Include(a => a.CaseType).Select(s => new DuiesBank
                {
                    NameCompany = s.Company.NameAr ?? "",
                    TypeOfCases = s.CaseType.NameAr ?? "",
                    City = s.HomeGovernorate,
                    NumberOfCases = _context.Cases.Where(c => c.HomeGovernorate == s.HomeGovernorate && c.CompanyId == Id && c.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !c.PaidFromBank && c.BankId == usera.FirstOrDefault().BankId && !c.Deleted).Count(),
                    Price = _context.BankCompany.Include(a => a.Cities).Any(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId && a.CompaniesId == Id) != false ? _context.BankCompany.Include(a => a.Cities).Where(a => !a.Deleted && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId && a.BanksId == usera.FirstOrDefault().BankId && a.CompaniesId == Id).FirstOrDefault().PriceCase : 0,
                    Total = _context.BankCompany.Include(a => a.Cities).Any(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId && a.CompaniesId == Id) != false ? (_context.BankCompany.Include(a => a.Cities).Where(a => !a.Deleted && a.BanksId == usera.FirstOrDefault().BankId && a.Cities.NameAr == s.HomeGovernorate && a.CaseTypesId == s.CaseTypeId && a.CompaniesId == Id).FirstOrDefault().PriceCase * _context.Cases.Include(a => a.Company).Where(c => c.HomeGovernorate == s.HomeGovernorate && !c.Deleted && c.CompanyId == Id && c.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank && !c.PaidFromBank && c.BankId == usera.FirstOrDefault().BankId).Count()) : 0
                }).Distinct().ToList();
             
                foreach (var customer in Duies)
                {
                    dt.Rows.Add(customer.Total, customer.TypeOfCases, customer.City, customer.NumberOfCases, customer.Price, customer.NameCompany);
                }
                /*  Response.SuppressContent = true;
                  _webHelper.IsPostBeingDone = true;
    */


                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        return stream.ToArray();
                    }
                }
            }
               return null;



        }

        
    }
}

