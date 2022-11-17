using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class Help : IHelp
    {
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public Help(IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }

        public async Task<dynamic> Add(ModelStateDictionary modelState, Helps model)
        {
            _context.Entry(model).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<dynamic> Add1(ModelStateDictionary modelState, Privacy model)
        {
            _context.Entry(model).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

 

    
        public dynamic Get(ModelStateDictionary modelState)
        {
            var Helps = _context.Helps.Where(a => !a.IsDeleted).FirstOrDefault();
            return Helps;
        }

        public dynamic Get1(ModelStateDictionary modelState)
        {

            var Privacy = _context.Privacy.Where(a=>!a.IsDeleted).FirstOrDefault();
            return Privacy;
        }

       
    }
}
