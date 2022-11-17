using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyEnquiry_BussniessLayer.AppsettingsModels;
using MyEnquiry_BussniessLayer.Bussniess;
using MyEnquiry_BussniessLayer.Interface;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEnquiry
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyAppContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyConnection")));


            services.Configure<SurveysConfig>(Configuration.GetSection("Surveys"));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = null;

            }).AddEntityFrameworkStores<MyAppContext>()
                .AddDefaultTokenProviders();



            services.ConfigureApplicationCookie(options => options.LoginPath = "/User/login");


            // Adding Authentication  

            services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)
  .AddOAuthValidation();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddTransient<IRoles, RoleBussniess>();
            services.AddTransient<IUsers, UserBussniess>();
            services.AddTransient<IBanks, BanksBussniess>();
            services.AddTransient<ICompany,CompanyBussniess>();
            services.AddTransient<IRegion, RegionBussniess>();
            services.AddTransient<ICaseTypes, CaseTypesBussniess>();
            services.AddTransient<IBankCases, BankCasesBussniess>();
            services.AddTransient<IRepresentative, RepresentativeBussniess>();
            services.AddTransient<ICases, CasesBussniess>();
            services.AddTransient<ICasesTracking, CasesTrackingBussniess>();
            services.AddTransient<ISuperVisorCases, SuperVisorCasesBussniess>();
            services.AddTransient<ICaseForms, CaseFormsBussniess>();
            services.AddTransient<ICity, CitiesBusiness>();
            services.AddTransient<IDues, DuiesBusiness>();
            services.AddTransient<IBankCompanyCase, BankCompanyCase>();
            services.AddTransient<IBusinessCompanyBankCase, BusinessCompanyBankCase>();
            services.AddTransient<IBills, Bills>();
            services.AddTransient<IHelp, Help>();
            services.AddTransient<ISurvey, SurveysBusiness>();
            services.AddTransient<IForgetPassword, FogetPassword>();
            services.AddTransient<IStatstics, Statstics>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //  endpoints.MapControllerRoute(
                //  name: "Admin",
                //  pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                //);
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=HomeIndex}/{id?}");


                endpoints.MapRazorPages();
            });
        }
    }
}
