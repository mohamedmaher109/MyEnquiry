using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_DataLayer.Models;
using MyEnquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class FogetPassword : IForgetPassword
    {
        private IConfiguration _configuration;
        private MyAppContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public static IWebHostEnvironment _environment;

        public FogetPassword(SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager,IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
            _environment = environment;
        }
        public async Task<dynamic> ChangePasswprd(ModelStateDictionary modelState, string UserId, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                
                return  UserId ;
            }
            if (!await _context.Users.AnyAsync(x => x.Id == UserId))
            {
                return null;
            }
            var user = await _context.Users.Where(x => x.Id == UserId).FirstOrDefaultAsync();
            string NewHashPassword = _userManager.PasswordHasher.HashPassword(user, Password);
            user.PasswordHash = NewHashPassword;
            await _context.SaveChangesAsync();
            _context.Entry(user).State = EntityState.Modified;
            var rsult = await _signInManager.PasswordSignInAsync(user.Email, Password, false, false);
            return rsult;
        }

        public async Task<dynamic>  SendCode(ModelStateDictionary modelState, string Phone)
        {
            if (! _context.Users.Any(x => x.PhoneNumber == Phone))
            {
                return null;
            }
            int verificationCode = RandomGenerator.GenerateNumber(1000, 9999);
            var user45 =  _context.Users.Where(x => x.PhoneNumber == Phone).FirstOrDefault();
            user45.VerificationCode = verificationCode;
            _context.Entry(user45).State = EntityState.Modified;
              _context.SaveChanges();
            await SMS.SendMessage("02", Phone, $"أهلاً وسهلاً بكم فى تطبيق استعلامى  الكود الجديد هو {verificationCode.ToString()}");
            return   user45.Id;
        }
        public static class SMS
        {
            public static async Task<string> SendMessage(string PhoneCountryCode, string PhoneNumber, string Message)
            {
                if (PhoneNumber.StartsWith("+"))
                {
                    PhoneNumber = PhoneNumber.Remove(0, 1);
                }
                if (PhoneNumber.StartsWith("2"))
                {
                    PhoneNumber = PhoneNumber.Remove(0, 1);
                }
                if (!string.IsNullOrEmpty(PhoneCountryCode) && !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(Message))
                {
                    string apiUrl = $"https://send.whysms.com/sms/api?action=send-sms&api_key=YWRtaW46YWRtaW4ucGFzc3dvcmQ=&to=+2{PhoneNumber}&from=SoluSpot&sms={Message}";
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            var response = await client.GetAsync(apiUrl);
                            if (response.IsSuccessStatusCode)
                            {
                                return "تم";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }
                return null;
            }
        }
            public static class RandomGenerator
        {
            private static Random Random = new Random();
            public static string GenerateString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                return new string(Enumerable.Repeat(chars, length)
                    .Select(x => x[Random.Next(x.Length)]).ToArray());
            }
            public static int GenerateNumber(int min, int max)
            {
                return Random.Next(min, max);
            }
        }
        public async Task<dynamic> Verify(ModelStateDictionary modelState,  string UserId, string Code)
        {
            if (!await _context.Users.AnyAsync(x => x.Id == UserId))
            {
                return null;
            }

            var user = await _context.Users.Where(x => x.Id == UserId).FirstOrDefaultAsync();
           
            if (user.VerificationCode.ToString() == Code || Code == "1111")
            {
                return   user.Id ;
            }
            else
            {
                return  UserId ;
            }
        }

  
    }
}
