using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
	public partial class MyAppContext : IdentityDbContext<ApplicationUser>
	{

		public MyAppContext()
		{

		}

		public MyAppContext(DbContextOptions<MyAppContext> options)
			: base(options)
		{

		}

		public DbSet<PermissionPages> PermissionPages { get; set; }
		public DbSet<PermissionPagesRoles> PermissionPagesRoles { get; set; }
		public DbSet<Banks> Banks { get; set; }
		public DbSet<Companies> Companies { get; set; }
		public DbSet<Cities> Cities { get; set; }
		public DbSet<Regions> Regions { get; set; }
		public DbSet<CaseTypes> CaseTypes { get; set; }
		public DbSet<CaseFiles> CaseFiles { get; set; }
		public DbSet<Cases> Cases { get; set; }
		public DbSet<CaseStatus> CaseStatus { get; set; }
		public DbSet<CoverageArea> CoverageArea { get; set; }
		public DbSet<UserMedia> UserMedia { get; set; }
		public DbSet<UserRefer> UserRefer { get; set; }
		public DbSet<CasesOrders> CasesOrders { get; set; }
		public DbSet<OrderFiles> OrderFiles { get; set; }
		public DbSet<OrderReview> OrderReview { get; set; }
		public DbSet<CasesTypeForms> CasesTypeForms { get; set; }
		public DbSet<CaseFormAnswers> CaseFormAnswers { get; set; }
		public DbSet<FormAnswersFiles> FormAnswersFiles { get; set; }
		public DbSet<RefusedCases> RefusedCases { get; set; }
		public DbSet<UserWallet> UserWallet { get; set; }
		public DbSet<Questions> Questions { get; set; }
		public DbSet<Answers> Answers { get; set; }
		public DbSet<Notifications> Notifications { get; set; }
		public DbSet<Complaint> Complaint { get; set; }
		public DbSet<BankCompany> BankCompany { get; set; }
		public DbSet<BankCompany2> BankCompany2 { get; set; }
		public DbSet<Helps> Helps { get; set; }
		public DbSet<Privacy> Privacy { get; set; }
		public DbSet<SurveyForm> SurveyForms { get; set; }
		public DbSet<SurveyFormElement> SurveyFormElements { get; set; }
		public DbSet<SurveyFormElementItem> SurveyFormElementItems { get; set; }
		public DbSet<SurveyFormResponse> SurveyFormResponses { get; set; }













		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=157.90.215.53\\MSSQLSERVER2017;User Id=dsadwswq;Password=Admin@123***;Initial Catalog=ast3lamy_gmail_com_Mysdsadas;");


				//optionsBuilder.UseSqlServer("Server=DESKTOP-9LD7KCK;Initial Catalog=ast3lamy_gmail_com_MyEnquiry;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
			}
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");


			base.OnModelCreating(modelBuilder);

			OnModelCreatingPartial(modelBuilder);

		}





		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
