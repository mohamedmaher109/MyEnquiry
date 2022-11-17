using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class Company : IComp
    {

        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public Company(IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }
        public dynamic GetCompanies(ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var usera = _context.Users.Where(a => a.Id == userId && !a.Deleted);
            var BnjkId = _context.Users.Where(a => a.Id == userId && !a.Deleted).FirstOrDefault().BankId;
            if (usera != null && usera.Any(a => a.BankId != null))
            {
                /*  var use2r = _context.Users.Where(a => !a.Deleted&&a.BankId== BnjkId).ToList();
                  foreach(var item in use2r)
                  {*/
                var Duies = _context.Cases.Where(a => !a.Deleted && a.BankId == usera.FirstOrDefault().BankId).OrderBy(a => a.HomeGovernorate).Distinct().Include(a => a.Bank).Include(a => a.Reviewer).Include(a => a.CaseStatus).Include(a => a.SuperVisor).Include(a => a.CaseType).Select(s => new DuiesBank
                {
                    Id = s.Id,
                    NameCompany = s.Company.NameAr ?? "",
                    TypeOfCases = s.CaseType.NameAr ?? "",
                    City = s.HomeGovernorate,
                    NumberOfCases = _context.Cases.Where(c => c.HomeGovernorate == s.HomeGovernorate && c.Company.NameAr == s.Company.NameAr).Count(),
                    Price = s.Reviewer != null && s.SuperVisor != null ? (s.Reviewer.price) + (s.SuperVisor.price) : 0,
                    Total = s.Reviewer != null && s.SuperVisor != null ? (((s.Reviewer.price) + (s.SuperVisor.price)) * (_context.Cases.Where(c => c.HomeGovernorate == s.HomeGovernorate && c.Company.NameAr == s.Company.NameAr).ToList().Count() )): 0
                }).ToList();
                return Duies;

            }

            return null;
        }

        public dynamic GetCompany(ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> GetTotal(ModelStateDictionary modelState, int Id)
        {
            throw new NotImplementedException();
        }

    }
}
