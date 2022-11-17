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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace MyEnquiry_BussniessLayer.Bussniess.BussniessApi
{
    public class AuthBussniess : IAuth
    {
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public AuthBussniess(UserManager<ApplicationUser> userManager, IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _configuration = configuration;

            _context = context;
            _environment = environment;
        }
        public async Task<dynamic> RegisterUserAsync(ModelStateDictionary modelState, RegisterRM model , bool isAbi)
        {
            if (model.password != model.ConfirmPassword)
            {
                modelState.AddModelError("كلمة المرور", "كلمة المرور ليست متطابقة");
                return null;
            }


            var checkphoneexist = _context.Users.FirstOrDefault(s => s.PhoneNumber == model.phoneNumber);

            if (checkphoneexist != null)
            {
                modelState.AddModelError("رقم الهاتف", "هذا الرقم موجود من قبل");
                return null;
            }
            var checkvodafonephoneexist = _context.Users.FirstOrDefault(s => s.CashNumber == model.cashNumber);

            if (checkvodafonephoneexist != null)
            {
                modelState.AddModelError("رقم الهاتف الفودافون", "هذا الرقم موجود من قبل");
                return null;
            }
            var checkvNationalId = _context.Users.FirstOrDefault(s => s.NationalId == model.nationalId);

            if (checkvNationalId != null)
            {
                modelState.AddModelError("رقم البطاقه", "هذا رقم البطاقه موجود من قبل");
                return null;
            }
            var checkvEmail = _context.Users.FirstOrDefault(s => s.Email == model.email);

            if (checkvEmail != null)
            {
                modelState.AddModelError("إيميل", "هذا الإيميل موجود من قبل");
                return null;
            }
            string code = RandomHelper.RandonString(5);


            

            ApplicationUser applicationUser;

            applicationUser = new ApplicationUser()
            {
                Email = model.email,
                UserName = model.phoneNumber,
                FullName = model.fullName,
                //FullName = HttpUtility.UrlDecode(model.fullName, Encoding.UTF8),
                CashNumber = model.cashNumber,
                cityId = model.cityid,
                Education = model.education,
                //Education = HttpUtility.UrlDecode(model.education, Encoding.UTF8),
                Address = model.address,
                //Address = HttpUtility.UrlDecode(model.address, Encoding.UTF8),
                //AddressDetails = HttpUtility.UrlDecode(model.addressDetails, Encoding.UTF8),
                AddressDetails = model.addressDetails,
                NationalId = model.nationalId,
                PasswordHash = model.password,
                SecurityCode= code,
                //LocationId = getlocation.Id,
                UserType = 4,
                CompanyId = model.companyId,
                PhoneNumber = model.phoneNumber,
              /*  price = model.price,*/
                FromDash = false

            };


            foreach (var item in model.coverageArea)
            {

                applicationUser.CoverageArea.Add(new CoverageArea
                {
                    RegionId = item,


                });
            }




            #region UserFiles

           

            if (model.nationalIdFront == null || model.nationalIdBack == null || model.criminalFish == null || model.academicQualification == null)
            {
                modelState.AddModelError("صورة ", "يجب اختيار جميع الصور المطلوبة");
                return null;
            }

            try
            {
               var profileimage = Files.SaveImage(model.profileImage, _environment, isAbi);

                applicationUser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.profile, url = profileimage });

            }
            catch (Exception)
            {

               
            }


            try
            {
              var  nationalmediafront = Files.SaveImage(model.nationalIdFront, _environment, isAbi);

                applicationUser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.NationalFront, url = nationalmediafront });

            }
            catch (Exception ex)
            {

                modelState.AddModelError(ex.Message, _environment.WebRootPath);
                return null;
            }

            try
            {
              var  nationalmediaback = Files.SaveImage(model.nationalIdBack, _environment, isAbi);
                applicationUser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.NationalBack, url = nationalmediaback });

            }
            catch (Exception)
            {

                modelState.AddModelError("صورة البطاقة", "يجب اختيار صورة البطاقة الخلفية");
                return null;
            }
            try
            {
              var  criminalFish = Files.SaveImage(model.criminalFish, _environment, isAbi);
                applicationUser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.CriminalFish, url = criminalFish });

            }
            catch (Exception)
            {

                modelState.AddModelError("الفيش الجنائى", "يجب اختيار صورة الفيش الجنائى");
                return null;
            }

            try
            {
               var acadimicQualification = Files.SaveImage(model.academicQualification, _environment, isAbi);
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

            someonetorefer.FullName = model.someOneFullName;//HttpUtility.UrlDecode(model.someOneFullName, Encoding.UTF8);
            someonetorefer.PhoneNumber = model.someOnePhoneNumber;
            someonetorefer.RelationShip = model.someOneRelationShip;
            someonetorefer.Description = model.someOneDescription;



            applicationUser.UserRefer = someonetorefer;
            //applicationUser.ReferId = someonetorefer.Id;
            #endregion


            var result = await _userManager.CreateAsync(applicationUser, model.password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(error.Code, error.Description);
                    return null;
                }
            }

            try
            {
                HttpWebResponse response = HTTPRequasts.Get("https://send.whysms.com/sms/api?action=send-sms&api_key=ZXN0M2xhbXk6RSRUM2xATXkhRkAhUg==&to=" + "+2" + applicationUser.PhoneNumber + "&from=" + "&sms=" + "مرحبا بك في استعلامى رمز التحقق هو " + code);

            }
            catch (Exception)
            {

                
            }


            return new
            {
                result = new
                {
                    code,
                    user = new
                    {
                        
                        name = applicationUser.FullName,
                        email = applicationUser.Email,                       
                        phone = applicationUser.PhoneNumber
                    }
                },
                msg = "Successfully Message"
            };


        }

        public async Task<dynamic> LoginAsync(ModelStateDictionary modelState, LoginRM model)
        {
            model.phoneNumber = StringOpriations.ConvertToEnglishNumber(model.phoneNumber);
            var User = await _userManager.FindByNameAsync(model.phoneNumber);

            if (User == null) 
            {
                modelState.AddModelError("رقم الهاتف", "هذ الرقم غير صحيح");
                return null;
            }
            if (User.Deleted)
            {
                modelState.AddModelError("رقم الهاتف", "هذا المستخدم محذوف");
                return null;
            }
            if (!User.IsAccepted)
            {
                modelState.AddModelError("رقم الهاتف", " جاري دراسة طلبك ");
                return null;
            }
            if (User.Blocked== true)
            {
                modelState.AddModelError("رقم الهاتف", "تم حظر هذا المستخدم");
                return null;
            }

            var result = await _userManager.CheckPasswordAsync(User, model.password);
            if (!result)
            {
                modelState.AddModelError("كلمة السر", "كلمة السر غير صحيحة");
                return null;
            }
            string token = JWTHelper.GetToken(User.Id, _configuration);



            return new
            {
                Result = new
                {
                    
                     token,
                    model.phoneNumber
                },
                Message = "Successfully"
            };

        }

        public async Task<dynamic> CIIES()
        {

            var Cities = await _context.Cities.Where(s => !s.Deleted).ToListAsync();


            return Cities;
        
        }
        public async Task<dynamic> EditProfile(ModelStateDictionary modelState, string Authorization, RegisterRM model, bool isAbi)
        {

            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            var getuser = _context.Users.Include(a=>a.UserRefer).Where(u => u.Id == UserId).FirstOrDefault();

            if (getuser == null)
            {
                modelState.AddModelError("Not Found", "This User Is Not Exist");
                return null;
            }

            if (await Task.Run(() => _userManager.Users.Any(item => (item.PhoneNumber == model.phoneNumber) && (item.Id != UserId))))
                return new  { msg =  "هذا الرقم المحمول مستخدم من قبل" };
            if (await Task.Run(() => _userManager.Users.Any(item => (item.Email == model.email) && (item.Id != UserId))))
                return new  { msg = "هذا البريد الالكتروني مستخدم من قبل" };

            getuser.FullName = model.fullName;
            getuser.UserName = model.phoneNumber;
            getuser.Email = model.email;
            getuser.cityId = model.cityid;
            getuser.Education = model.education ;
            getuser.Address = model.address;
            // getuser.AddressDetails = HttpUtility.UrlDecode(model.addressDetails, Encoding.UTF8) ;
            getuser.AddressDetails = model.addressDetails;
            getuser.NationalId = model.nationalId;
            getuser.CompanyId = model.companyId;
            getuser.UserType = 4;
            getuser.PhoneNumber = model.phoneNumber;
            getuser.CashNumber = model.cashNumber;


            #region UserFiles



            if (model.profileImage != null)
            {
                try
                {
                    var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == UserId && m.Type == (int)UserMediaTypes.profile);
                    if (getusermedia != null)
                    {
                        getusermedia.url = Files.SaveImage(model.profileImage, _environment, isAbi);
                    }
                    else
                    {
                        var url = Files.SaveImage(model.profileImage, _environment,isAbi);
                        getuser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.profile, url = url });

                    }


                }
                catch (Exception)
                {


                }
            }

            if (model.nationalIdFront != null)
            {
                try
                {
                    var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == UserId && m.Type == (int)UserMediaTypes.NationalFront);
                    if (getusermedia != null)
                    {
                        getusermedia.url = Files.SaveImage(model.nationalIdFront, _environment, isAbi);
                    }
                    else
                    {
                        var url = Files.SaveImage(model.nationalIdFront, _environment, isAbi);
                        getuser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.NationalFront, url = url });

                    }


                }
                catch (Exception)
                {


                }
            }

            if (model.nationalIdBack != null)
            {
                try
                {
                    var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == UserId && m.Type == (int)UserMediaTypes.NationalBack);
                    if (getusermedia != null)
                    {
                        getusermedia.url = Files.SaveImage(model.nationalIdBack, _environment, isAbi);
                    }
                    else
                    {
                        var url = Files.SaveImage(model.nationalIdBack, _environment, isAbi);
                        getuser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.NationalBack, url = url });

                    }


                }
                catch (Exception)
                {


                }
            }



            if (model.criminalFish != null)
            {
                try
                {
                    var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == UserId && m.Type == (int)UserMediaTypes.CriminalFish);
                    if (getusermedia != null)
                    {
                        getusermedia.url = Files.SaveImage(model.criminalFish, _environment, isAbi);
                    }
                    else
                    {
                        var url = Files.SaveImage(model.criminalFish, _environment, isAbi);
                        getuser.UserMedia.Add(new UserMedia { Type = (int)UserMediaTypes.CriminalFish, url = url });

                    }


                }
                catch (Exception)
                {


                }
            }


            if (model.academicQualification != null)
            {
                try
                {
                    var getusermedia = _context.UserMedia.FirstOrDefault(m => m.UserId == UserId && m.Type == (int)UserMediaTypes.AcadimicQualification);
                    if (getusermedia != null)
                    {
                        getusermedia.url = Files.SaveImage(model.academicQualification, _environment, isAbi);
                    }
                    else
                    {
                        var url = Files.SaveImage(model.academicQualification, _environment, isAbi);
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


                getuser.UserRefer.FullName = model.someOneFullName;
                getuser.UserRefer.PhoneNumber = model.someOnePhoneNumber;
                getuser.UserRefer.RelationShip = model.someOneRelationShip ;
                getuser.UserRefer.Description = model.someOneDescription;

            }

            var getusercoverageareas = _context.CoverageArea.Where(s => s.UserId == UserId).ToList();
            _context.CoverageArea.RemoveRange(getusercoverageareas);
            foreach (var item in model.coverageArea)
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




        public async Task<dynamic> VerfiyAsync(ModelStateDictionary modelState, Verfication VerficationModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(VerficationModel.phone);
                if (user == null)
                {
                    modelState.AddModelError("الهاتف", "لا يوجد مستخدم بهذا الرقم");
                    return null;
                }

                if (user.SecurityCode != VerficationModel.code)
                {
                    modelState.AddModelError("الكود", "الكود غير صحيح");
                    return null;
                }

                user.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    modelState.AddModelError("الهاتف", "لم يتم التفعيل");
                    return null;
                }



                return new
                {
                    result = new
                    {
                        activate = user.EmailConfirmed,
                        user = new
                        {
                            
                            name = user.FullName,
                            email = user.Email,
                            phone = user.PhoneNumber,
                            
                        }
                    },
                    msg = "Successfully Message"
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }

        }




        public async Task<dynamic> ForgetPasswordAsync(ModelStateDictionary modelState, ForgetPassword userModel)
        {
            try
            {
                userModel.Phone = StringOpriations.ConvertToEnglishNumber(userModel.Phone);
                var user = await _userManager.FindByNameAsync(userModel.Phone);

                if (user == null)
                {
                    modelState.AddModelError("رقم الهاتف", "هذ الرقم غير صحيح");
                    return null;
                }

             

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                var activateCode = RandomHelper.RandonString(5);

                ///ToDo Send mail
                try
                {
                    HttpWebResponse response = HTTPRequasts.Get("https://send.whysms.com/sms/api?action=send-sms&api_key=ZXN0M2xhbXk6RSRUM2xATXkhRkAhUg==&to=" + "+2" + user.PhoneNumber + "&from=" + "&sms=" + "مرحبا بك في استعلامى رمز التحقق هو " + activateCode);

                }
                catch (Exception)
                {


                }

                user.SecurityCode = activateCode;
                var result = await _userManager.UpdateAsync(user);
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
                        code = activateCode
                    }
                };
            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

        public async Task<dynamic> RestPasswordAsync(ModelStateDictionary modelState, ResetPassword userModel)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userModel.phone);
                if (user == null)
                {
                    modelState.AddModelError("الهاتف", "هذا الرقم غير موجود");
                    return null;
                }

                if (userModel.password != userModel.confirmPassword)
                {
                    modelState.AddModelError("كلمة المرور", "كلمتا المرور غير متطابقين");
                    return null;
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, userModel.password);
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
                    result = new {
                        
                    }
                };

            }
            catch (Exception ex)
            {
                modelState.AddModelError(string.Join(",", ex.Data), string.Join(",", ex.InnerException));
                return null;
            }
        }

        public async Task<dynamic> UpdateLocation(ModelStateDictionary modelState, string Authorization, UpdateUserLocation model)
        {
            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            var getuser = _userManager.Users.Where(u => u.Id == UserId).FirstOrDefault();

            if (getuser == null)
            {
                modelState.AddModelError("غير موجود", "هذا المستخدم غير موجود");
                return null;
            }

            getuser.lat = model.lat;
            getuser.lng = model.lng;

            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {
                    
                },
                msg = "Successfully Message"
            };
        }

        public dynamic GetProfileAsync(ModelStateDictionary modelState, string Authorization)
        {
            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);
            var user = _context.Users.Include(u=>u.CoverageArea).Include(u=>u.UserRefer).Where(u => u.Id == UserId).FirstOrDefault();

            if (user == null)
            {
                modelState.AddModelError("غير موجود", "هذا المستخدم غير موجود");
                return null;
            }


            var usermedia = _context.UserMedia.Where(s => s.UserId == UserId).ToList();


            return new
            {
                result = new
                {
                    id = user.Id,
                    fullName = user.FullName,
                    cityid = user.cityId,
                    CreatedTime = user.Createdon,
                    education = user.Education,
                    address = user.Address,
                    addressDetails = user.AddressDetails,
                    nationalId = user.NationalId,
                    companyId = user.CompanyId,
                    coverageIds = user.CoverageArea.Select(s => s.RegionId).ToArray(),
                    email = user.Email,
                    cashPhone = user.CashNumber,
                    phone = user.PhoneNumber,
                    referFullName = user.UserRefer.FullName,
                    referPhoneNumber = user.UserRefer.PhoneNumber,
                    referRelationShip = user.UserRefer.RelationShip,
                    referDescription = user.UserRefer.Description,
                    userImage = usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.profile) != null ? usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.profile).url : "",
                    nationalIdFront = usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.NationalFront) != null ? usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.NationalFront).url : "",
                    nationalIdBack = usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.NationalBack) != null ? usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.NationalBack).url : "",
                    criminalFish = usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.CriminalFish) != null ? usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.CriminalFish).url : "",
                    academicQualification = usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.AcadimicQualification) != null ? usermedia.FirstOrDefault(s => s.Type == (int)UserMediaTypes.AcadimicQualification).url : "",
                    lat = user.lat,
                    lng = user.lng,
                    blocked = user.Blocked,
                    wallet = _context.UserWallet.Where(s => s.UserId == user.Id&&!s.Paid).Sum(s => s.Amount),
                    finish=_context.CasesOrders.Include(s=>s.Cases).Where(s=>s.UserId==user.Id&&s.Status==2&&s.Cases.CaseStatusId==(int)CaseEnumStatus.AcceptedFromBank).Count(),
                    rejected=_context.CasesOrders.Where(s=>s.UserId==user.Id&&s.Status==3).Count(),

                },
                msg = "Successfully Message"
            };

        }

        public async Task<dynamic> SendComplaint(ModelStateDictionary modelState, string Authorization, string message)
        {
            string UserId = JWTHelper.GetPrincipal(Authorization, _configuration);

            var complain = new Complaint() { 
            
            Message=message,
            userId=UserId,
            
            };

            _context.Complaint.Add(complain);
            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {
                    
                },
                msg = "Successfully"
            };
        }

        public async Task<dynamic> Privacy()
        {

            var Cities = await _context.Privacy.Where(s => !s.IsDeleted).FirstOrDefaultAsync();


            return Cities;
        }

        public async Task<dynamic> Help()
        {

            var Cities = await _context.Helps.Where(s => !s.IsDeleted).FirstOrDefaultAsync();


            return Cities;
        }
    }
}
