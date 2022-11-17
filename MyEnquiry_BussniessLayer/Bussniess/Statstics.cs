using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyEnquiry.Helper;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModels;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class Statstics : IStatstics
    {
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public Statstics(IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _context = context;
            _environment = environment;
        }
        public dynamic GetStatic()
        {
            var survy = _context.Cases.Include(a => a.Bank).Include(a => a.Company).Where(a => !a.Deleted).Select(a => new StatsticsVm
            {
                NameBank = a.Bank.NameAr,
                NameCompany = a.Company.NameAr,
                NumberOfCasesDone = _context.Cases.Where(s => !s.Deleted && s.BankId == a.BankId && s.CompanyId == a.CompanyId && s.CaseStatusId == (int)CaseEnumStatus.AcceptedFromBank).Count(),
                NumberOfCasesWaiting=_context.Cases.Where(s => !s.Deleted && s.BankId == a.BankId && s.CompanyId == a.CompanyId && s.CaseStatusId != (int)CaseEnumStatus.AcceptedFromBank).Count()
            }).Distinct().ToList();
            return survy;
        }
    }
}
