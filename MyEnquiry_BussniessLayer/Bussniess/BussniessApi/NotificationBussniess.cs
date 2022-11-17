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
    public class NotificationBussniess : INotification
    {
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public NotificationBussniess(UserManager<ApplicationUser> userManager, IConfiguration configuration, MyAppContext context, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }

    
        public dynamic GetNotification(ModelStateDictionary modelState, string Authorization,int page)
        {

            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            var getuser = _userManager.Users.Where(u => u.Id == UserId).FirstOrDefault();

            if (getuser == null)
            {
                modelState.AddModelError("Not Found", "This User Is Not Exist");
                return null;
            }
            NotificationManager notificationManager = new(_context);


            var notifications = _context.Notifications
                    .Where(o => o.IsDeleted == false && o.ToId == UserId && o.Status)
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .Select(Not => new
                    {
                        Not.Title,
                        Not.Message,
                        Not.OrderId,
                        CreatedAt = Not.CreatedAt
                    }).ToList();
            
            notificationManager.ReadNotification(UserId);

            return new
            {
                result = new
                {
                    notifications
                },
                msg = "Successfully"
            };


        }
    }
}
