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
    public class DropdownBussniess : IDropdown
    {
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public DropdownBussniess(UserManager<ApplicationUser> userManager, IConfiguration configuration, MyAppContext context, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }

        public  dynamic GetCompanies(ModelStateDictionary modelState)
        {

            var companies = _context.Companies.Where(c => !c.Deleted && c.Active).Select(c => new
            {

                id=c.Id,
                name=c.NameAr

            }).ToList();
            return new
            {
                result = new
                {
                    companies
                },
                msg = "Successfully Message"
            };

        }
        public  dynamic GetRegions(ModelStateDictionary modelState,int id)
        {

            var regions = _context.Regions.Where(c => !c.Deleted && c.Active&&c.CitiesId==id).Select(c => new
            {

                id=c.Id,
                name= c.NameAr

            }).ToList();
            return new
            {
                result = new
                {
                    regions
                },
                msg = "Successfully Message"
            };

        }

        
    }
}
