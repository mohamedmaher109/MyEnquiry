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
using OfficeOpenXml;
using System.Reflection;
using AspNetCore.Reporting;
using MyEnquiry_BussniessLayer.ViewModels;

namespace MyEnquiry_BussniessLayer.Bussniess
{
    public class CasesBussniess : ICases
    {
      
        private IConfiguration _configuration;
        private MyAppContext _context;
        public static IWebHostEnvironment _environment;

        public CasesBussniess( IConfiguration configuration, MyAppContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            
            _context = context;
            _environment = environment;
        }


        public dynamic GetBanks(ModelStateDictionary modelState)
        {
            var banks = _context.Banks.Where(b => b.Active && !b.Deleted).Select(b => new SelectView { Id = b.Id, Name = b.NameAr }).ToList();

            return banks;
        }

        public dynamic CasesFromBank(ModelStateDictionary modelState,int BankId,ClaimsPrincipal user)
        {

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }

            var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);

            if ((checkuser.UserType != 2 || checkuser.CompanyId == null))
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }
            var type = _context.Cases.Include(s=>s.Bank).Include(s=>s.CaseStatus).Include(s=>s.CaseFiles).Include(s=>s.CaseType).Where(s=>s.BankId==BankId&&!s.Deleted&&s.CompanyId== checkuser.CompanyId && !s.IsMain).Select(s => new Cases
            {
                Id = s.Id,
                Bank=s.Bank,
                CaseType=s.CaseType,
                CaseStatus=s.CaseStatus,
                FileToShow= s.CaseFiles.FirstOrDefault()!=null? s.CaseFiles.FirstOrDefault().ExcelSheet:"",                
                CreatedAt =s.CreatedAt

            }).ToList();
            return type;
        }

        public async Task<dynamic> ReciveCase(ModelStateDictionary modelState, int Id,int type)
        {
            var getcase = _context.Cases.FirstOrDefault(s => s.Id == Id);

            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "هذه الحاله غير موجوده");
                return null;
            }
            if (type == 1)
            {
                getcase.CaseStatusId = (int)CaseEnumStatus.RecivedFromCompany;
                await _context.SaveChangesAsync();
                return new
                {
                    result = new
                    {

                    },
                    msg = "تم ااستلام الحالة بنجاح"
                };
            }
            else
            {
                getcase.CaseStatusId = (int)CaseEnumStatus.SentFromBank;
                await _context.SaveChangesAsync();
                return new
                {
                    result = new
                    {

                    },
                    msg = "تم الغاء استلام الحالة بنجاح"
                };
            }
        }

        public async Task<dynamic> UploadFile(ModelStateDictionary modelState, int Id, IFormFile file, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                modelState.AddModelError("خطأ", "برجاء تسجيل الدخول");
                return null;
            }
            var getcase = _context.Cases.FirstOrDefault(r => r.Id == Id&&!r.Deleted);
            var GeUser = _context.Users.FirstOrDefault(r => r.Id == userId && !r.Deleted);

            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "لم نستطيع إيجاد هذه الحالة");
                return null;
            }
            var excelurl = Files.SaveExcel(file, _environment);
            #region readfromexel

            if (file == null || file.Length == 0)
            {
                modelState.AddModelError("تحذير", "يوجد مشكلة في الملف");

                return null;
            }


            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                modelState.AddModelError("تحذير", "يوجد مشكلة في الملف");

                return null;
            }


            var fileName = file.FileName;
            //var filePath = Path.Combine(_environment.WebRootPath, excelurl);
            //var fileLocation = new FileInfo(filePath);

            using (var fileStream = File.Create(_environment.WebRootPath + excelurl))
            {
                await file.CopyToAsync(fileStream);
            }

            if (file.Length <= 0)
            {
                modelState.AddModelError("تحذير", "يوجد مشكلة في الملف");

                return null;
            }
            var EnquirerName = "";
            var ReviewerName = "";
            var regiontosearch = "";
            var getreviwer = _context.Users.Where(s => s.CompanyId == getcase.CompanyId && !s.Deleted && s.Reviewr).ToList();

            using (ExcelPackage package = new ExcelPackage(_environment.WebRootPath + excelurl))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                //var workSheet = package.Workbook.Worksheets.First();
                int totalRows = workSheet.Dimension.Rows;
                int counter = 2;
                int reper = 2;
                int reper1 = 2;

                for (int i = 2; i <= totalRows; i++, counter++, reper++, reper1++)
                {
                    var Model = new Cases();
                    Model.ClientName = workSheet.Cells[i, 1].Value.ToString();
                    Model.ClientNumbers = workSheet.Cells[i, 2].Value.ToString();
               
                    Model.NationalId = workSheet.Cells[i, 3].Value.ToString();
                    /*  Model.WorkGovernorate = workSheet.Cells[i, 4].Value.ToString();
                      Model.WorkAddress = workSheet.Cells[i, 5].Value.ToString();*/
                    Model.HomeAddress = workSheet.Cells[i, 4].Value.ToString();
                    Model.HomeGovernorate = workSheet.Cells[i, 5].Value.ToString();
                    if(!_context.Cities.Any(a=>!a.Deleted&&a.NameAr.Contains(workSheet.Cells[i, 5].Value.ToString())))
                    {
                        modelState.AddModelError("تحذير", "هذه المحافظه غير موجوده");
                        return null;
                    }
                    regiontosearch = workSheet.Cells[i, 6].Value.ToString();
                    Model.lat = workSheet.Cells[i, 7].Value.ToString();
                    Model.lng = workSheet.Cells[i, 8].Value.ToString();
                    /* ReviewerName = workSheet.Cells[i, 9].Value.ToString();*/
                    var casefile = new CaseFiles();
                    casefile.ExcelSheet = excelurl;
                    casefile.Type = 2;
                    Model.BankId = getcase.BankId;
                    Model.CompanyId = getcase.CompanyId;
                    Model.CaseTypeId = getcase.CaseTypeId;
                    Model.SuperVisorId = userId;
                    Model.CasesId = Id;
                    Model.IsMain = true;
                    Model.CaseFiles.Add(casefile);
                    Model.CaseStatusId = (int)CaseEnumStatus.WaitingForRecivers;
                    if ((getreviwer.Count()+2) <= counter)
                    {
                        counter = 2;
                        Model.ReviewerId = getreviwer.Select(a => a.Id).ElementAtOrDefault((counter - 2));
                        _context.Cases.Add(Model);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        Model.ReviewerId = getreviwer.Select(a => a.Id).ElementAtOrDefault(counter - 2);
                        _context.Cases.Add(Model);
                        await _context.SaveChangesAsync();
                    }
                    #region GetNearestRepresentative
                    var nearestRepresentative = _context.Users.Include(s => s.CoverageArea).ThenInclude(s => s.Region)
                        .Where(s => s.UserType == 4 && !s.Deleted && s.CoverageArea.Select(s => s.Region.NameAr).Contains(regiontosearch)&&s.CompanyId== GeUser.CompanyId).ToList();

                    if (nearestRepresentative.Count < 1)
                    {

                        //get nearest by lat and lng
                        var addresslatlng = await Location.GetByAddress(Model.HomeAddress);
                        var minDistance = _context.Users.Min(x => Math.Sqrt(Math.Pow((double)addresslatlng.lat, (double)x.lat) + Math.Pow((double)addresslatlng.lng, (double)x.lng)));
                        var closestLocation = _context.Users.Where(x => Math.Sqrt(Math.Pow((double)addresslatlng.lat, (double)x.lat)) + Math.Pow((double)addresslatlng.lng, (double)x.lng) >= minDistance&&x.CompanyId== GeUser.CompanyId).ToList();
                        if (closestLocation != null)
                        {
                            if ((closestLocation.Count()+2) == reper)
                            {
                                reper = 2;
                                var order = new CasesOrders();
                                order.UserId = closestLocation.Select(a => a.Id).ElementAtOrDefault(reper - 2);
                                order.Status = 1;
                                Model.CasesOrders.Add(order);
                                Model.CaseStatusId = (int)CaseEnumStatus.AssignedToRecivers;
                            }
                            else
                            {
                                var order = new CasesOrders();
                                order.UserId = closestLocation.Select(a => a.Id).ElementAtOrDefault(reper - 2);
                                order.Status = 1;
                                Model.CasesOrders.Add(order);
                                Model.CaseStatusId = (int)CaseEnumStatus.AssignedToRecivers;
                            }

                        }


                    }
                    else
                    {
                        foreach (var item in nearestRepresentative)
                        {

                            Model.CasesOrders.Add(new CasesOrders
                            {
                                Status = 1,
                                UserId = item.Id,

                            });
                        }
                        Model.CaseStatusId = (int)CaseEnumStatus.AssignedToRecivers;

                    }

                    #endregion


                    await _context.SaveChangesAsync();


                }
                package.Dispose();
            }
            // var Role =await  _context.UserRoles.Where(s => s.RoleId== "18e0a3b3-6359-4cf0-ac63-345a50f2b711").ToListAsync();
            /*        if (getreviwer == null)
                    {
                        modelState.AddModelError("غير موجود", "لا يوجد مراجع بهذا الاسم");
                        return null;
                    } 
                    var Gover = _context.Cities.FirstOrDefault(s => s.NameAr.Equals(getcase.HomeGovernorate));
                    if (getreviwer == null)
                    {
                        modelState.AddModelError("غير موجود", "لا توجد دوله بهذا الاسم");
                        return null;
                    }*/
            /*       foreach(var item in getreviwer)
                    {
                        var casefile = new CaseFiles();
                        casefile.ExcelSheet = excelurl;
                        casefile.Type = 2;
                        getcase.CaseFiles.Add(casefile);
                        getcase.CaseStatusId = (int)CaseEnumStatus.WaitingForRecivers;
                        getcase.ReviewerId= item.Id;
                    }*/

            /* getcase.ReviewerId = getreviwer.Id;*/

            #endregion


            getcase.CaseStatusId = (int)CaseEnumStatus.WaitingForRecivers;
             _context.Cases.Update(getcase);
            _context.SaveChanges();

            return new
            {
                result = new
                {

                },
                msg = "تم تسجيل الحاله بنجاح وارسالها للمناديب"
            };
        }

        public async Task<dynamic> RefusedFile(ModelStateDictionary modelState, int Bank, IFormFile file)
        {
            
            var excelurl= Files.SaveExcel(file, _environment);
            #region readfromexel

         
              
            if (file == null || file.Length == 0)
            {
                modelState.AddModelError("تحذير", "يوجد مشكلة في الملف");

                return null;
            }
                

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                modelState.AddModelError("تحذير", "يوجد مشكلة في الملف");

                return null;
            }
                

            var fileName = file.FileName;
            //var filePath = Path.Combine(_environment.WebRootPath, excelurl);
            //var fileLocation = new FileInfo(filePath);

            using (var fileStream = File.Create(_environment.WebRootPath + excelurl))
            {
                await file.CopyToAsync(fileStream);
            }

            if (file.Length <= 0)
            {
                modelState.AddModelError("تحذير", "يوجد مشكلة في الملف");

                return null;
            }

            var refusedcases = new List<RefusedCases>();
            using (ExcelPackage package = new ExcelPackage(_environment.WebRootPath + excelurl))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                //var workSheet = package.Workbook.Worksheets.First();
                int totalRows = workSheet.Dimension.Rows;

                
                for (int i = 2; i <= totalRows; i++)
                {
                    refusedcases.Add(new RefusedCases
                    {
                        CaseNumber=int.Parse(workSheet.Cells[i, 1].Value.ToString()),
                        Resoan= workSheet.Cells[i, 2].Value.ToString(),
                        Solved=false,
                        BankId=Bank


                    });
                   
                   
                }
                package.Dispose();
            }

            _context.RefusedCases.AddRange(refusedcases);
            #endregion


            await _context.SaveChangesAsync();
            return new
            {
                result = new
                {

                },
                msg = "تم ارسال الحالات المرفوضه الى البنك بنجاح"
            };
        }

        public dynamic GetRefusedCases(ModelStateDictionary modelState, ClaimsPrincipal user)
        {

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }

            var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);

            if ((checkuser.UserType != 1 || checkuser.BankId == null))
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }
            var type = _context.RefusedCases.Where(s => s.BankId == checkuser.BankId).Select(s => new RefusedCases
            {
                Id = s.Id,
                BankId = s.BankId,
                Solved=s.Solved,
                CaseNumber=s.CaseNumber,
                Resoan=s.Resoan

            }).ToList();
            return type;
        }

        public async Task<dynamic> Solved(ModelStateDictionary modelState, int Id)
        {
            var getcase = _context.RefusedCases.FirstOrDefault(s => s.Id == Id);

            

            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "هذه الحاله غير موجوده");
                return null;
            }
            getcase.Solved = true;
            await _context.SaveChangesAsync();

            return new
            {
                result = new
                {

                },
                msg = "تم تغيير الحالة بنجاح"
            };


        }

        public async Task<dynamic> GetPDF(ModelStateDictionary modelState, int Id)
        {
            //var getcase = _context.Cases.Include(s=>s.Bank).Include(s=>s.CaseType).FirstOrDefault(s => s.Id == Id);
            //if (getcase == null)
            //{
            //    modelState.AddModelError("غير مسموح", "هذة الحالة غير موجودة");
            //    return null;
            //}



            //string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("MyEnquiry_BussniessLayer.dll", string.Empty);
            //string rdlcFilePath = string.Format("{0}Reports\\{1}.rdlc", fileDirPath, "Case");
            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //Encoding.GetEncoding("windows-1252");

            //LocalReport report = new LocalReport(rdlcFilePath);

            //var _Pdf = new PDF();
            //var _Pdflist = new List<PDF>();
            //var Answers = new List<QuestionAndAnswers>();
            //var answersFiles = new List<AnswerFiles>();

            //_Pdf.BankName = getcase.Bank.NameAr;
            //_Pdf.CaseNumber = getcase.Id;
            //_Pdf.CaseType = getcase.CaseType.NameAr;
            //_Pdf.ClienName = getcase.ClientName;
            //_Pdf.ClienNumber = getcase.ClientNumbers;
            //_Pdf.WorkGov = getcase.WorkGovernorate;
            //_Pdf.WorkAddre = getcase.WorkAddress;
            //_Pdf.HomeAddre = getcase.HomeAddress;
            //_Pdf.HomeGov = getcase.HomeGovernorate;


            //var getforms = _context.CasesTypeForms.Where(s => s.CaseTypeId == getcase.CaseTypeId).ToList();
            //var getanswers = _context.CaseFormAnswers.Where(s => getforms.Select(a => a.Id).Contains(s.FormId)).ToList();
            //var getanswersfiles = _context.FormAnswersFiles.Where(s => getanswers.Select(a => a.Id).Contains(s.CaseFormAnsweId)).ToList();
            //foreach (var item in getforms)
            //{

            //    Answers.Add(new QuestionAndAnswers { 

            //    Question=item.Question,
            //    Answer=getanswers.FirstOrDefault(s=>s.FormId==item.Id)!=null? getanswers.FirstOrDefault(s => s.FormId == item.Id).Answer:"",
            //    AnswerId= getanswers.FirstOrDefault(s => s.FormId == item.Id) != null ? getanswers.FirstOrDefault(s => s.FormId == item.Id).Id :0,
            //    HasFile=item.HasFile,


            //    });
            //}

            ////foreach (var item in getanswersfiles)
            ////{
            ////    string pathPhoto = new Uri(_environment.WebRootPath+item.Url).AbsoluteUri;
            ////    byte[] fileBytes = System.IO.File.ReadAllBytes(pathPhoto);
            ////    MemoryStream rs = new MemoryStream(fileBytes);

            ////    answersFiles.Add(new AnswerFiles
            ////    {
            ////        AnswerId=item.CaseFormAnsweId,
            ////        Url= fileBytes/*pathPhoto*/,


            ////    });
            ////}

            //_Pdflist.Add(_Pdf);
            ////ReservationList.Add(ReservationListone);

            ////report.AddDataSource("Reservation", ReservationList);
            //report.AddDataSource("Case", _Pdflist);
            //report.AddDataSource("AnswerFiles", answersFiles);
            //report.AddDataSource("QuestionAndAnswers", Answers);

            //var result = report.Execute(GetRenderType("pdf"), 1, parameters);
            //return result.MainStream;
            return null;





            

        }

        public dynamic GetDoneCases(ModelStateDictionary modelState, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }

            var checkuser = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (checkuser.UserType == 1 || checkuser.UserType == 5)
            {
                var type = _context.Cases.Include(s => s.Company).Include(s => s.CaseType).Include(s => s.CaseStatus).Where(s => s.BankId == checkuser.BankId && s.CaseStatusId >= (int)CaseEnumStatus.AcceptedFromSupervisor&&!s.Deleted).Select(s => new Cases
                {
                    Id = s.Id,
                    ClientName = s.ClientName,
                    ClientNumbers = s.ClientNumbers,
                    CaseType = s.CaseType,
                    CaseStatus = s.CaseStatus,
                    Company = s.Company,
                    FilesANswer=s.FilesANswer

                }).ToList();
                return type;
            }
            if (checkuser.UserType != 1 || checkuser.BankId == null || checkuser.UserType != 5)
            {
                modelState.AddModelError("غير مسموح", "غير مسموح لك باتمام هذه العملية");
                return null;
            }
            return null;


        }





        //public byte[] GenerateMailPdfAsync(string reportName, int Id)
        //{
        //    try
        //    {
                




        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}



        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            switch (reportType.ToLower())
            {
                default:
                case "pdf":
                    renderType = RenderType.Pdf;
                    break;
                case "word":
                    renderType = RenderType.Word;
                    break;
                case "excel":
                    renderType = RenderType.Excel;
                    break;
            }

            return renderType;
        }

        public async Task<dynamic> ChangeStatus(ModelStateDictionary modelState, int Id, int type,string reson)
        {
            var getcase = _context.Cases.Include(s=>s.CasesOrders).FirstOrDefault(s => s.Id == Id);



            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "هذه الحاله غير موجوده");
                return null;
            }
            if (type == 1)
            {
                var userwallets = new List<UserWallet>();

                userwallets.Add(new UserWallet { 
                Amount=10,
                CaseId=getcase.Id,
                UserId=getcase.SuperVisorId,
                
                
                });
                userwallets.Add(new UserWallet { 
                Amount=10,
                CaseId=getcase.Id,
                UserId=getcase.ReviewerId,
                
                
                });
                userwallets.Add(new UserWallet { 
                Amount=10,
                CaseId=getcase.Id,
                UserId=getcase.CasesOrders.FirstOrDefault(s=>s.Status==2).UserId,
                
                
                });

                getcase.CaseStatusId =(int)CaseEnumStatus.AcceptedFromBank;
                getcase.DoneFromBank = DateTime.Now;
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم مراجعة الحالة بنجاح"
                };
            }
            else
            {
                getcase.CaseStatusId =(int)CaseEnumStatus.RejectedFromBank;
                getcase.RejectResion = reson;
                await _context.SaveChangesAsync();

                return new
                {
                    result = new
                    {

                    },
                    msg = "تم رفض الحالة"
                };
            }

        }

        public async Task<dynamic> deleteAllCases(ModelStateDictionary modelState, int Id)
        {
            var getcase = _context.Cases.Include(s => s.CasesOrders).FirstOrDefault(s => s.Id == Id);
            var Allcasses = _context.Cases.Where(a => !a.Deleted && a.CasesId == Id && a.IsMain).ToList();
            if (getcase == null)
            {
                modelState.AddModelError("غير موجود", "هذه الحاله غير موجوده");
                return null;
            } 
            if (Allcasses == null)
            {
                modelState.AddModelError("غير موجود", "لاتوجد حالات  ");
                return null;
            }
            foreach(var item in Allcasses)
            {
                item.Deleted = true;
                _context.Cases.Update(item);
                _context.SaveChanges();

            }
            try
            {
                getcase.CaseStatusId = (int)CaseEnumStatus.SentFromBank;
                _context.Cases.Update(getcase);
                _context.SaveChanges();
            }
            catch
            {
                modelState.AddModelError("خطأ", "حدث خطأ ما");
                return null;
            }
            return new
            {
                result = new
                {

                },
                msg = "تم حذف كل الحالات بنجاح"
            };
        }
    }

}
