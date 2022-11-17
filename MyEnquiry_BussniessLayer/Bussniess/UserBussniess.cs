using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyEnquiry_BussniessLayer.Helper;
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
using Microsoft.EntityFrameworkCore;
using MyEnquiry_BussniessLayer.ViewModels;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class UserBussniess : IUsers
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _rolemanger;
        private SignInManager<ApplicationUser> _signInManager;
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IHostingEnvironment _environment;

        public UserBussniess(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolemanger, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, MyAppContext context, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _rolemanger = rolemanger;
            _configuration = configuration;
            
            _signInManager = signInManager;
            _context = context;
            _environment = environment;
        }
        public dynamic GetUsers(ModelStateDictionary modelState)

        {
            var users = _context.Users.Where(s=>s.UserType!=4).Include(s=>s.Company).Include(s=>s.Bank).Where(s => s.FromDash == true).Select(s => new UserView
            {
                Id = s.Id,
                FullName = s.FullName ?? "",
                Email = s.Email,
                Phone = s.PhoneNumber,
                Usertype=s.UserType,
                Price= s.price!=null?s.price:0,
                place=s.Company!=null?s.Company.NameAr:s.Bank!=null?s.Bank.NameAr:"",
                //RoleId=_context.UserRoles.FirstOrDefault(r=>r.UserId==s.Id).RoleId,
                


            }).ToList();
            return users;
        }




        public async Task<dynamic> AddUser(ModelStateDictionary modelState, UserView model)
        {
            try
            {
                if (model.Password != model.confirmpassword)
                {
                    modelState.AddModelError("كلمة المرور", "كلمة المرور ليست متطابقة");
                    return null;
                }


                var checkphoneexist = _context.Users.FirstOrDefault(s => s.PhoneNumber == model.Phone);

                if (checkphoneexist != null)
                {
                    modelState.AddModelError("رقم الهاتف", "هذا الرقم موجود من قبل");
                    return null;
                }




                ApplicationUser applicationUser;

                //string userImage = "";
                //if (userModel.user_image != null)
                //{
                //    try
                //    {
                //        userImage = FileHelper.SaveImage(userModel.user_image, _environment);
                //    }
                //    catch (Exception ex)
                //    {
                //        modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                //        return null;
                //    }
                //}

                var role = _context.Roles.FirstOrDefault(s => s.Id == model.RoleId);

                applicationUser = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.FullName ,
                    FullName = model.FullName,
                    PasswordHash = model.Password,
                    //LocationId = getlocation.Id,
                    UserType=model.Usertype,
                    BankId=model.BankId,
                    CompanyId = model.CompanyId,
                    PhoneNumber = model.Phone,
                    CashNumber = model.CashPhone,
                    price=model.Price,
                    UserImg = "",
                    FromDash = true
                };



                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        modelState.AddModelError(error.Code, error.Description);
                        return null;
                    }
                }
                else
                {
                    if (role != null)
                    {
                        if(role.Id== "18e0a3b3-6359-4cf0-ac63-345a50f2b711")
                        {
                            applicationUser.Reviewr = true;
                            _context.SaveChanges();
                        }
                        await _userManager.AddToRoleAsync(applicationUser, role.Name);
                    }

                }

                

                return new
                {
                    result = new
                    {


                    },
                    msg = "تم اضافة المسخدم بنجاح"
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }





        public dynamic GetUser(ModelStateDictionary modelState, string Id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(r => r.Id == Id);
                if (user == null)
                {
                    modelState.AddModelError("Not Found", "This User Is Not Exist");
                    return null;
                }

                var userview = new UserView
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    BankId = user.BankId,
                    CompanyId = user.CompanyId,
                    Usertype = user.UserType,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    CashPhone = user.CashNumber,
                    RoleId = _context.UserRoles.FirstOrDefault(r => r.UserId == Id)!=null? _context.UserRoles.FirstOrDefault(r => r.UserId == Id).RoleId:"",
                };

                return userview;

            }
            catch (Exception ex)
            {

                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            };
        }
        public async Task<dynamic> ChangePassword(ModelStateDictionary modelState, ForgetPassword model)
        {

            var getuser = _context.Users.FirstOrDefault(s => s.Id == model.Id);
            try
            {
                if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.confirmpassword))
                {
                    getuser.PasswordHash = _userManager.PasswordHasher.HashPassword(getuser, model.Password);

                }
                await _context.SaveChangesAsync();

                return "تم تعديل الرقم السرى بنجاح";
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

       public async Task<dynamic> EditUser(ModelStateDictionary modelState, UserView model)
        {
            try
            {
                var getuser = _context.Users.FirstOrDefault(s => s.Id == model.Id);
                if (getuser == null)
                {
                    modelState.AddModelError("Not Found", "This User Is Not Exist");
                    return null;
                }

                if (string.IsNullOrEmpty(model.RoleId))
                {
                    modelState.AddModelError("Requried", "You must select a Role");
                    return null;
                }
                var checkphoneexist = _context.Users.FirstOrDefault(s => s.PhoneNumber == model.Phone&&s.PhoneNumber!=getuser.PhoneNumber);

                if (checkphoneexist != null)
                {
                    modelState.AddModelError("رقم الهاتف", "هذا الرقم موجود من قبل");
                    return null;
                }
                var checkvodafonephoneexist = _context.Users.FirstOrDefault(s => s.CashNumber == model.CashPhone&&s.CashNumber!= getuser.CashNumber);

                if (checkvodafonephoneexist != null)
                {
                    modelState.AddModelError("رقم الهاتف الفودافون", "هذا الرقم موجود من قبل");
                    return null;
                }
                var checkvNationalId = _context.Users.FirstOrDefault(s => s.NationalId == model.NationalId&&s.NationalId!=getuser.NationalId);

                if (checkvNationalId != null)
                {
                    modelState.AddModelError("رقم البطاقه", " رقم البطاقه موجود من قبل");
                    return null;
                }
                var checkvEmail = _context.Users.FirstOrDefault(s => s.Email == model.Email&&s.Email!=getuser.Email);

                if (checkvEmail != null)
                {
                    modelState.AddModelError("إيميل", "هذا الإيميل موجود من قبل");
                    return null;
                }
                getuser.FullName = model.FullName;
                getuser.UserName = model.FullName;
                getuser.Email = model.Email;
                //getuser.BankId = model.BankId;
                //getuser.CompanyId = model.CompanyId;
                //getuser.UserType = model.Usertype;
                getuser.PhoneNumber = model.Phone;
                getuser.price = model.Price;
                getuser.CashNumber = model.CashPhone;
                var role = _context.Roles.FirstOrDefault(s => s.Id == model.RoleId);
                var userrole = _context.UserRoles.FirstOrDefault(s => s.UserId == model.Id);
                if (userrole == null)
                {
                    await _userManager.AddToRoleAsync(getuser, role.Name);
                    if (role.Id == "18e0a3b3-6359-4cf0-ac63-345a50f2b711")
                    {
                        getuser.Reviewr = true;
                        _context.SaveChanges();
                    }
                }
                else if (userrole.RoleId != model.RoleId)
                {

                    _context.UserRoles.Remove(userrole);
                 
                    await _userManager.AddToRoleAsync(getuser, role.Name);
                    if (role.Id != "18e0a3b3-6359-4cf0-ac63-345a50f2b711")
                    {
                        getuser.Reviewr = false;
                        _context.SaveChanges();
                    }
                    if (role.Id == "18e0a3b3-6359-4cf0-ac63-345a50f2b711")
                    {
                        getuser.Reviewr = true;
                        _context.SaveChanges();
                    }

                }




                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم تعديل المستخدم بنجاح"
                };

            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

        public async Task<dynamic> DeleteUser(ModelStateDictionary modelState, string Id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(r => r.Id == Id);
                if (user == null)
                {
                    modelState.AddModelError("غير موجود", "هذا المستخدم غير موجود");
                    return null;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف المستخدم بنجاح"
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError("غير مسموح", "يجب حذف كل البيانات المتعلقة بهذا المستخدم اولا");
                return null;
            }
        }




        public async Task<dynamic> WebLoginUserAsync(ModelStateDictionary modelState, LoginRq userModel)
        {
                var userq = await _context.Users.AnyAsync(a => a.Email == userModel.email);
            if (userq != false)
            {
                var user =  _context.Users.Where(a => a.Email == userModel.email).FirstOrDefault();
                if (user == null)
                {
                    modelState.AddModelError("البريد الالكترونى", "لا يوجد مستخدم بهذا البريد الالكترونى");
                    return null;
                }
                var result = await _userManager.CheckPasswordAsync(user, userModel.password);
                if (!result)
                {
                    modelState.AddModelError("كلمة المرور", "كلمة المرور غير صحيحة");
                    return null;
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }
            return null;
            /*            }
                        catch (Exception ex)
                        {

                            return CustomBadRequest.CustomExErrorResponse(ex);
                        }*/
        }


        public async Task<dynamic> SignUserOutAsync(ModelStateDictionary modelState)
        {
            await _signInManager.SignOutAsync();
            return true;

        }

        public async Task<dynamic> Changepassword(ModelStateDictionary modelState, Changepassword model)
        {
            try
            {



                var user = await _userManager.FindByIdAsync(model.userid);
                if (user == null)
                {
                    modelState.AddModelError("غير موجود", "لم نستطيع ايجاد هذا المستخدم");
                    return null;
                }


                if (model.newpassword != model.newpasswordconfirm)
                {
                    modelState.AddModelError("غير متطابق", "كلمتا السر غير متطابقتين");
                    return null;
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.newpassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        modelState.AddModelError(error.Code, error.Description);
                        return null;
                    }
                }

                return new
                {
                    result = new
                    {

                    },

                };

            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }



        public dynamic GetRolesForUser(ModelStateDictionary modelState, string Id)

        {
            var Role = _context.Roles.Select(s => new { RoleId = s.Id, Name = s.Name }).ToList();

            return Role;
        }

        public dynamic GetBanks(ModelStateDictionary modelState)
        {
            var banks = _context.Banks.Where(b => b.Active && !b.Deleted).Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

            return banks;
        }

        public dynamic GetCompanies(ModelStateDictionary modelState)
        {
            var companies = _context.Companies.Where(b => b.Active && !b.Deleted).Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

            return companies;
        }







        public dynamic GetPermissionPages(ModelStateDictionary modelState, string Id)
        {
            try
            {
                var getpermissioninrole = _context.PermissionPagesRoles.Where(s => s.RoleId == Id).Select(s => s.PageId).ToList();

                var getAllPermissions = _context.PermissionPages.Select(s => new { id = s.PageId.ToString(), parent = s.ParentId != 0 ? s.ParentId.ToString() : "#", text = s.PageName, state = new { selected = getpermissioninrole.Contains(s.Id) && s.ParentId != 0 ? true : false } }).ToList();

                return getAllPermissions;
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

        public dynamic PostPermissions(ModelStateDictionary modelState, string[] Ids, string roleid)
        {

            try
            {
                var getrolepermissions = _context.PermissionPagesRoles.Where(s => s.RoleId == roleid).ToList();
                var newpermlist = new List<PermissionPagesRoles>();
                var idsarr = new List<int>();
                foreach (var item in Ids.Distinct().ToList())
                {
                    var x = int.Parse(item);
                    var page = _context.PermissionPages.FirstOrDefault(s => s.PageId == x);


                    idsarr.Add(page.Id);



                }
                var parents = _context.PermissionPages.Where(s => idsarr.Contains(s.Id) && s.ParentId != 0).Select(s => s.ParentId).ToList();






                foreach (var item in parents.Distinct().ToList())
                {

                    var page = _context.PermissionPages.FirstOrDefault(s => s.PageId == item);


                    idsarr.Add(page.Id);



                }






                foreach (var x in idsarr.Distinct().ToList())
                {
                    var permiss = new PermissionPagesRoles
                    {
                        PageId = x,
                        RoleId = roleid
                    };
                    newpermlist.Add(permiss);

                }



                //var gettoremove = getrolepermissions.Where(s => !idsarr.Contains(s.PageId)).ToList();
                if (getrolepermissions.Count > 0)
                {
                    _context.PermissionPagesRoles.RemoveRange(getrolepermissions);
                }

                if (newpermlist.Count > 0)
                {
                    _context.PermissionPagesRoles.AddRange(newpermlist);
                }

                _context.SaveChanges();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم اضافة الصلاحيات الى المجموعه"
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }

        }

        public dynamic GetUsersCash(ModelStateDictionary modelState)
        {
            var getusersmoney = _context.UserWallet.Include(s=>s.User).Where(s => !s.Paid).ToList();


            return getusersmoney;
        }
    }

}
