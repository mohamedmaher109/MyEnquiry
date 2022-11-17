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
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MyEnquiry.Helper;
using MyEnquiry_BussniessLayer.ViewModels.ResponseModels;
using MyEnquiry_BussniessLayer.ViewModels;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class RepresentativeBussniess : IRepresentative
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _rolemanger;
        private SignInManager<ApplicationUser> _signInManager;
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public RepresentativeBussniess(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolemanger, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, MyAppContext context, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _rolemanger = rolemanger;
            _configuration = configuration;
            
            _signInManager = signInManager;
            _context = context;
            _environment = environment;
        }
        public dynamic GetFiles(ModelStateDictionary modelState,string Id)

        {
            var files = _context.UserMedia.Where(s => s.UserId == Id).Select(s => new SelectView
            {

                Id=s.Id,
                Name=s.url,


            }).ToList();
            return files;
        }

        
        public dynamic Get(ModelStateDictionary modelState,ClaimsPrincipal user) 
             
        {

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = _context.Users.Include(s => s.Company).Include(s => s.city).Include(s => s.CoverageArea).ThenInclude(s => s.Region).Where(s => s.UserType == 4 && !s.Deleted && !s.Blocked).ToList();
         

                var usercompany = _context.Users.Include(a=>a.city).FirstOrDefault(s => s.Id == userId);
                if (usercompany.CompanyId != null)
                {
                    var result = users.Where(s => s.CompanyId==usercompany.CompanyId).Select(s => new UserView
                    {
                        Id = s.Id,
                        FullName = s.FullName ?? "",
                        Email = s.Email,
                        Phone = s.PhoneNumber,
                        Usertype = s.UserType,
                        City = s.city != null ? s.city.NameAr : "",
                        Education = s.Education,
                        Address = s.Address,
                        NationalId = s.NationalId,
                        Coveragenames=s.CoverageArea.ToList().SelectMany(a=>a.Region.NameAr).ToString(),
                        CompanyName = s.Company.NameAr,
                        //RoleId=_context.UserRoles.FirstOrDefault(r=>r.UserId==s.Id).RoleId,
                       Blocked=s.Blocked,
                       IsAccepted=s.IsAccepted,
                       lat =s.lat,
                       lng = s.lng
                    }).ToList();
                return result;
            }
                else
                {
                var result = users.Select(s => new UserView
                {
                    Id = s.Id,
                    FullName = s.FullName ?? "",
                    Email = s.Email,
                    Phone = s.PhoneNumber,
                    Usertype = s.UserType,
                    City = s.city!=null?s.city.NameAr : "",
                    Education = s.Education,
                    Address = s.Address,
                    NationalId = s.NationalId,
                    Coveragenames=s.CoverageArea.ToList().SelectMany(a=>a.Region.NameAr).ToString(),
                    CompanyName =s.Company!=null? s.Company.NameAr:"",
                    //RoleId=_context.UserRoles.FirstOrDefault(r=>r.UserId==s.Id).RoleId,
                    Blocked = s.Blocked,
                    IsAccepted = s.IsAccepted,
                    Price = s.price


                }).ToList();
                return result;
            }
                
                
        
            
        }




        public async Task<dynamic> Add(ModelStateDictionary modelState, UserView model)
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
                var checkvodafonephoneexist = _context.Users.FirstOrDefault(s => s.CashNumber == model.CashPhone);

                if (checkvodafonephoneexist != null)
                {
                    modelState.AddModelError("رقم الهاتف الفودافون", "هذا الرقم موجود من قبل");
                    return null;
                }  
                var checkvNationalId = _context.Users.FirstOrDefault(s => s.NationalId == model.NationalId);

                if (checkvNationalId != null)
                {
                    modelState.AddModelError("رقم البطاقه", " رقم البطاقه موجود من قبل");
                    return null;
                }      
                var checkvEmail = _context.Users.FirstOrDefault(s => s.Email == model.Email);

                if (checkvEmail != null)
                {
                    modelState.AddModelError("إيميل", "هذا الإيميل موجود من قبل");
                    return null;
                }




                ApplicationUser applicationUser;

                applicationUser = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Phone,
                    FullName = model.FullName,
                    CashNumber = model.CashPhone,
                   /* city = model.City,*/
                    Education=model.Education,
                    Address=model.Address,
                    AddressDetails=model.AddressDetails,
                    NationalId=model.NationalId,
                    PasswordHash = model.Password,
                    //LocationId = getlocation.Id,
                    UserType=4,
                    CompanyId = model.CompanyId,
                    cityId = model.CitiyId,
                    PhoneNumber = model.Phone,
                    UserImg = "",
                    FromDash = true,
                    IsAccepted = model.IsAccepted,
                    price =model.Price
                    
                };


                foreach (var item in model.CoverageIds)
                {
                    
                    applicationUser.CoverageArea.Add(new CoverageArea
                    {
                        RegionId = item,



                    });
                }


                #region UserFiles

                var nationalmediafront = "";
                var nationalmediaback = "";
                var criminalFish = "";
                var acadimicQualification = "";

                if (model.NationalMediaFront == null || model.NationalMediaBack == null || model.CriminalFish == null || model.AcadimicQualification == null)
                {
                    modelState.AddModelError("صورة ", "يجب اختيار جميع الصور المطلوبة");
                    return null;
                }
                try
                {
                    nationalmediafront = Files.SaveImage(model.NationalMediaFront, _environment);

                    applicationUser.UserMedia.Add(new UserMedia { Type=(int)UserMediaTypes.NationalFront,url=nationalmediafront});

                }
                catch (Exception)
                {

                    modelState.AddModelError("صورة البطاقة", "يجب اختيار صورة البطاقة الامامية");
                    return null;
                }

                try
                {
                    nationalmediaback = Files.SaveImage(model.NationalMediaBack, _environment);
                    applicationUser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.NationalBack, url = nationalmediaback });

                }
                catch (Exception)
                {

                    modelState.AddModelError("صورة البطاقة", "يجب اختيار صورة البطاقة الخلفية");
                    return null;
                }
                try
                {
                    criminalFish = Files.SaveImage(model.CriminalFish, _environment);
                    applicationUser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.CriminalFish, url = criminalFish });

                }
                catch (Exception)
                {

                    modelState.AddModelError("الفيش الجنائى", "يجب اختيار صورة الفيش الجنائى");
                    return null;
                }

                try
                {
                    acadimicQualification = Files.SaveImage(model.AcadimicQualification, _environment);
                    applicationUser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.AcadimicQualification, url = acadimicQualification });

                }
                catch (Exception)
                {

                    modelState.AddModelError("المؤهل", "يجب اختيار صورة المؤهل");
                    return null;
                }

                #endregion

                #region SomeOneToRefer

                var someonetorefer = new UserRefer();

                someonetorefer.FullName = model.ReferFullName;
                someonetorefer.PhoneNumber = model.ReferPhoneNumber;
                someonetorefer.RelationShip = model.ReferRelationShip;
                someonetorefer.Description = model.ReferDescription;



                applicationUser.UserRefer = someonetorefer;
                //applicationUser.ReferId = someonetorefer.Id;
                #endregion


                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        modelState.AddModelError(error.Code, error.Description);
                        return null;
                    }
                }
                //else
                //{
                //    if (role != null)
                //    {
                //        await _userManager.AddToRoleAsync(applicationUser, role.Name);
                //    }

                //}

                

                return new
                {
                    result = new
                    {


                    },
                    msg = "تم اضافة المندوب بنجاح"
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }





        public dynamic GetById(ModelStateDictionary modelState, string Id)
        {
            try
            {
                var user = _context.Users.Include(s=>s.CoverageArea).ThenInclude(a=>a.Region).Include(s=>s.UserRefer).Include(s => s.Company).Include(s => s.city).FirstOrDefault(r => r.Id == Id);
                if (user == null)
                {
                    modelState.AddModelError("Not Found", "This User Is Not Exist");
                    return null;
                }

                var userview = new UserView
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    /* City = user.City,*/
                    CitiyId = (int)user.cityId,
                    Education = user.Education,
                    Address = user.Address,
                    AddressDetails = user.AddressDetails,
                    NationalId = user.NationalId,
                    Price=user.price,
                    CompanyId = user.CompanyId,
                     CompanyName = user.Company.NameAr,
                     City = user.city.NameAr,
                    CoverageIds=user.CoverageArea.Select(s=> s.RegionId).ToList(),
                    Usertype = user.UserType,
                    Email = user.Email,
                    CashPhone = user.CashNumber,
                    Phone = user.PhoneNumber,
                    ReferFullName=user.UserRefer.FullName,
                    ReferPhoneNumber=user.UserRefer.PhoneNumber,
                    ReferRelationShip=user.UserRefer.RelationShip,
                    ReferDescription=user.UserRefer.Description,
                    IsAccepted=user.IsAccepted,
                    price = user.price
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




        public async Task<dynamic> Edit(ModelStateDictionary modelState, UserView model)
        {
            try
            {
                var getuser = _context.Users.Include(s=>s.UserRefer).Include(s=>s.UserMedia).Include(s=>s.CoverageArea).FirstOrDefault(s => s.Id == model.Id);
                if (getuser == null)
                {
                    modelState.AddModelError("Not Found", "This User Is Not Exist");
                    return null;
                }
                var checkphoneexist = _context.Users.FirstOrDefault(s => s.PhoneNumber == model.Id);

                if (checkphoneexist != null&& checkphoneexist.Id!=model.Id)
                {
                    modelState.AddModelError("رقم الهاتف", "هذا الرقم موجود من قبل");
                    return null;
                }
                var checkvodafonephoneexist = _context.Users.FirstOrDefault(s => s.CashNumber == model.CashPhone);

                if (checkvodafonephoneexist != null&& checkvodafonephoneexist.Id!=model.Id)
                {
                    modelState.AddModelError("رقم الهاتف الفودافون", "هذا الرقم موجود من قبل");
                    return null;
                }
                var checkvNationalId = _context.Users.FirstOrDefault(s => s.NationalId == model.NationalId);

                if (checkvNationalId != null&& checkvNationalId.Id!=model.Id)
                {
                    modelState.AddModelError("رقم البطاقه", " رقم البطاقه موجود من قبل");
                    return null;
                }
                var checkvEmail = _context.Users.FirstOrDefault(s => s.Email == model.Email);

                if (checkvEmail != null&& checkvEmail.Id!=model.Id)
                {
                    modelState.AddModelError("إيميل", "هذا الإيميل موجود من قبل");
                    return null;
                }
                 
                getuser.FullName = model.FullName;
                getuser.UserName = model.Phone;
                getuser.NormalizedUserName = model.Phone;
                getuser.Email = model.Email;
               /* getuser.c = model.City;*/
                getuser.Education = model.Education;
                getuser.Address = model.Address;
                getuser.AddressDetails = model.AddressDetails;
                getuser.NationalId = model.NationalId;
                getuser.CompanyId = model.CompanyId;
                getuser.UserType =4;
                getuser.PhoneNumber = model.Phone;
                getuser.CashNumber = model.CashPhone;
                getuser.cityId = model.CitiyId;
                getuser.price = model.Price;
                getuser.IsAccepted = model.IsAccepted;
                getuser.price = model.Price;



                #region UserFiles



                if (model.NationalMediaFront != null)
                {
                    try
                    {
                        var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == model.Id && m.Type == (int)UserMediaTypes.NationalFront);
                        if (getusermedia != null)
                        {
                            getusermedia.url = Files.SaveImage(model.NationalMediaFront, _environment);
                        }
                        else
                        {
                            var url= Files.SaveImage(model.NationalMediaFront, _environment);
                            getuser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.NationalFront, url = url });

                        }


                    }
                    catch (Exception)
                    {

                       
                    }
                }

                if (model.NationalMediaBack != null)
                {
                    try
                    {
                        var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == model.Id && m.Type == (int)UserMediaTypes.NationalBack);
                        if (getusermedia != null)
                        {
                            getusermedia.url = Files.SaveImage(model.NationalMediaBack, _environment);
                        }
                        else
                        {
                            var url= Files.SaveImage(model.NationalMediaBack, _environment);
                            getuser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.NationalBack, url = url });

                        }


                    }
                    catch (Exception)
                    {

                       
                    }
                }
                
                

                if (model.CriminalFish != null)
                {
                    try
                    {
                        var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == model.Id && m.Type == (int)UserMediaTypes.CriminalFish);
                        if (getusermedia != null)
                        {
                            getusermedia.url = Files.SaveImage(model.CriminalFish, _environment);
                        }
                        else
                        {
                            var url= Files.SaveImage(model.CriminalFish, _environment);
                            getuser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.CriminalFish, url = url });

                        }


                    }
                    catch (Exception)
                    {

                       
                    }
                }
                

                if (model.AcadimicQualification != null)
                {
                    try
                    {
                        var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == model.Id && m.Type == (int)UserMediaTypes.AcadimicQualification);
                        if (getusermedia != null)
                        {
                            getusermedia.url = Files.SaveImage(model.AcadimicQualification, _environment);
                        }
                        else
                        {
                            var url= Files.SaveImage(model.AcadimicQualification, _environment);
                            getuser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.AcadimicQualification, url = url });

                        }


                    }
                    catch (Exception)
                    {

                       
                    }
                }



                #endregion




                #region SomeOneToRefer

                if (getuser.ReferId != null)
                {


                    getuser.UserRefer.FullName = model.ReferFullName;
                    getuser.UserRefer.PhoneNumber = model.ReferPhoneNumber;
                    getuser.UserRefer.RelationShip = model.ReferRelationShip;
                    getuser.UserRefer.Description = model.ReferDescription;

                }

                var getusercoverageareas = _context.CoverageArea.Where(s => s.UserId == model.Id).ToList();
                _context.CoverageArea.RemoveRange(getusercoverageareas);
                foreach (var item in model.CoverageIds)
                {
                   
                    getuser.CoverageArea.Add(new CoverageArea
                    {
                        RegionId = item,


                    });
                }



                #endregion
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم تعديل المندوب بنجاح"
                };

            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

        public async Task<dynamic> Delete(ModelStateDictionary modelState, string Id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(r => r.Id == Id);
                if (user == null)
                {
                    modelState.AddModelError("غير موجود", "هذا المندوب غير موجود");
                    return null;
                }

                user.Deleted = true;
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم حذف المندوب بنجاح"
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError("غير مسموح", "يجب حذف كل البيانات المتعلقة بهذا المندوب اولا");
                return null;
            }
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




        public dynamic GetCompanies()
        {
            var companies = _context.Companies.Where(b => b.Active && !b.Deleted).Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();
            var comp = new SelectList(companies, "Id", "Name");
            return comp;
        }   
        public dynamic GetCitiies()
        {
            var companies = _context.Cities.Where(b =>  !b.Deleted).ToList();
            var comp = new SelectList(companies, "Id", "NameAr");
            return comp;
        }

        

        public SelectList GetAreas()
        {
            List<Regions> areas = _context.Regions.Where(b => b.Active && !b.Deleted).ToList();
            
          /*  var area = new SelectList(areas, "Id", "NameAr");*/
            return new SelectList(areas, "Id", "NameAr");
        }
        public SelectList GetCityAreas(int cityid)
        {
            List<Regions> areas = _context.Regions.Where(b => b.CitiesId == cityid && b.Active && !b.Deleted).ToList();
            return new SelectList(areas, "Id", "NameAr");
        }
       
        public async Task<dynamic> Block(ModelStateDictionary modelState, string id)
        {
            var getuser = _context.Users.FirstOrDefault(s => s.Id == id);

            if (getuser.Blocked == true)
            {
                getuser.Blocked = false;
            }
            else
            {
                getuser.Blocked = true;
            }
            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تمت العملية  بنجاح"
            };
        }

        public dynamic GetBlocked(ModelStateDictionary modelState)
        {
            var result = _context.Users.Where(s => s.Blocked).Select(s => new UserView
            {
                Id = s.Id,
                FullName = s.FullName ?? "",
                Email = s.Email,
                Phone = s.PhoneNumber,
                Usertype = s.UserType,
               /* City = s.City,*/
                Education = s.Education,
                Address = s.Address,
                NationalId = s.NationalId,
              //Coveragenames=s.CoverageArea.ToList().SelectMany(a=>a.Region.NameAr).ToString(),
                CompanyName = s.Company.NameAr,
                //RoleId=_context.UserRoles.FirstOrDefault(r=>r.UserId==s.Id).RoleId,
                Blocked = s.Blocked,
                IsAccepted = s.IsAccepted,
                Price = s.price,




            }).ToList();
            return result;

         }


        public dynamic GetGovernment( long id)
        {
            var getregion =  _context.Regions.Where(a => !a.Deleted && a.CitiesId == id).ToList();
            var regp = new SelectList(getregion, "Id", "NameAr");
            return regp;
        }

        public dynamic GetLocation(ModelStateDictionary modelState, string Id)
        {
            var location = new latAndlng();

            var getuser = _context.Users.FirstOrDefault(s => s.Id== Id );

            if (getuser == null)
            {
                modelState.AddModelError("غير موجود", "لم يتم ايجاد المندوب بعد");
                return null;
            }

            location.lat = getuser.lat != null ? (decimal)getuser.lat : 0;
            location.lng = getuser.lng != null ? (decimal)getuser.lng : 0;
            return location;
        }
        public dynamic GetLocation3(ModelStateDictionary modelState, long Id)
        {
            var location = new latAndlng();

            var getuser = _context.CasesOrders.Where(a=>!a.Cases.Deleted).Include(a=>a.Cases).Include(a=>a.User).FirstOrDefault(s => s.CaseId == Id);

            if (getuser == null)
            {
                modelState.AddModelError("غير موجود", "لم يتم ايجاد المندوب بعد");
                return null;
            }

            location.lat = getuser.User.lat != null ? (decimal)getuser.User.lat : 0;
            location.lng = getuser.User.lng != null ? (decimal)getuser.User.lng : 0;
            return location;
        }
        public dynamic GetDeleted(ModelStateDictionary modelState)
        {
            var result = _context.Users.Where(s => s.UserType == 4 && s.Deleted).Select(s => new UserView
            {
                Id = s.Id,
                FullName = s.FullName ?? "",
                Email = s.Email,
                Phone = s.PhoneNumber,
                Usertype = s.UserType,
                /* City = s.City,*/
                Education = s.Education,
                Address = s.Address,
                NationalId = s.NationalId,
                //Coveragenames=s.CoverageArea.ToList().SelectMany(a=>a.Region.NameAr).ToString(),
                CompanyName = s.Company.NameAr,
                //RoleId=_context.UserRoles.FirstOrDefault(r=>r.UserId==s.Id).RoleId,
                Blocked = s.Blocked,
                IsAccepted = s.IsAccepted,
                Price = s.price,



            }).ToList();
            return result;
        }

        public async Task<dynamic> BackFromDeleted(ModelStateDictionary modelState, string id)
        {
            var getuser = _context.Users.FirstOrDefault(s => s.Id == id);

            if (getuser.Deleted == true)
            {
                getuser.Deleted = false;
            }
            else
            {
                getuser.Deleted = true;
            }
            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تمت العملية  بنجاح"
            };
        }

        public async Task<dynamic> ChangeStatus(ModelStateDictionary modelState, string Id)
        {
            var getuser = _context.Users.FirstOrDefault(s => s.Id == Id);

            if (getuser.IsAccepted == false)
            {
                getuser.IsAccepted = true;
            }
            else
            {
                getuser.IsAccepted = true;
            }
            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تمت العملية  بنجاح"
            };
        }
    }

}
