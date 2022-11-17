using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_BussniessLayer.ViewModel;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class RoleBussniess : IRoles
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _rolemanger;
        private SignInManager<ApplicationUser> _signInManager;
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public RoleBussniess(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolemanger, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, MyAppContext context, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _rolemanger = rolemanger;
            _configuration = configuration;
            
            _signInManager = signInManager;
            _context = context;
            _environment = environment;
        }

        public dynamic GetRoles(ModelStateDictionary modelState)

        {
            var game = _context.Roles.Select(s => new RoleView
            {
                Id = s.Id,
                Name = s.Name ?? ""

            }).ToList();
            return game;
        }
        public dynamic GetRolesForUser(ModelStateDictionary modelState,string Id)

        {
            var game = _context.Roles.Select(s=>new { RoleId=s.Id,Name=s.Name}).ToList();
          
            return game;
        }



        public async Task<dynamic> AddRole(ModelStateDictionary modelState, RoleView model)
        {
            try
            {
                bool x = await _rolemanger.RoleExistsAsync(model.Name);
                if (!x)
                {
                    // first we create Admin rool    
                    var role = new IdentityRole();
                    role.Name = model.Name;
                    await _rolemanger.CreateAsync(role);


                    return new
                    {
                        result = new
                        {

                        },
                        msg = "تم اضافة المجموعه بنجاح"
                    };

                }
                else
                {
                    modelState.AddModelError("تداخل بيانات", "هذة المجموعه موجوده من قبل");
                    return null;
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;

            }



        }

        public dynamic GetRole(ModelStateDictionary modelState, string Id)
        {
            try
            {
                var role = _context.Roles.FirstOrDefault(r => r.Id == Id);
                
                if (role==null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذة المجموعة");
                    return null;
                }

                var roleview = new RoleView
                {
                    Id = role.Id,
                    Name = role.Name
                };

                return roleview;

            }
            catch (Exception ex)
            {

                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            };
        }

        public async Task<dynamic> EditRole(ModelStateDictionary modelState, RoleView model)
        {
            try
            {

                var role = _context.Roles.FirstOrDefault(r => r.Id == model.Id);
                if (role == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذة المجموعة");
                    return null;
                }

                role.Name = model.Name;
               await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم تعديل المجموعة بنجاح"
                };

            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

        public async Task<dynamic>  DeleteRole(ModelStateDictionary modelState, string Id)
        {
            try
            {
                var role = _context.Roles.FirstOrDefault(r => r.Id == Id);
                if (role == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذة المجموعة");
                    return null;
                }


                var rolepages = _context.PermissionPagesRoles.Where(s => s.RoleId == role.Id).ToList();
                    if (rolepages.Count > 0)
                    {
                        modelState.AddModelError("غير مسموح", "يجب الغاء جميع صلاحيات هذة المجموعة قبل حذفها ");
                        return null;
                    }
              //var res=  await _rolemanger.DeleteAsync(role);

                 _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف المجموعة بنجاح"
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }




    }

}
